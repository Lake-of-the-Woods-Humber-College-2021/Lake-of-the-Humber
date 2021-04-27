using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;
using Lake_of_the_Humber.Models;
using Lake_of_the_Humber.Models.ViewModels;


namespace Lake_of_the_Humber.Controllers
{
    public class StaffInfoController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static StaffInfoController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        }
        /// <summary>
        /// Grabs the authentication credentials which are sent to the Controller.
        /// This is NOT considered a proper authentication technique for the WebAPI. It piggybacks the existing authentication set up in the template for Individual User Accounts. Considering the existing scope and complexity of the course, it works for now.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: StaffInfo/List?{PageNum}
        // If the page number is not included, set it to 0
       
        public ActionResult List(int PageNum = 0)
        {   
            // Grab all staffinfos
            string url = "staffinfodata/getstaffinfos";
            // Send off an HTTP request
            // GET : /api/staffinfoata/GetSatffInfos
            // Retrieve response
            HttpResponseMessage getstaffResponse = client.GetAsync(url).Result;
            // If the response is a success, proceed
            if (getstaffResponse.IsSuccessStatusCode)
            {
                // Fetch the response content into IEnumerable<ProductDto>
                IEnumerable<StaffInfoDto> SelectedStaffs = getstaffResponse.Content.ReadAsAsync<IEnumerable<StaffInfoDto>>().Result;

                // -- Start of Pagination Algorithm --

                // Find the total number of products
                int StaffCount = SelectedStaffs.Count();
                // Number of products to display per page
                int PerPage = 12;
                // Determines the maximum number of pages (rounded up), assuming a page 0 start.
                int MaxPage = (int)Math.Ceiling((decimal)StaffCount / PerPage) - 1;

                // Lower boundary for Max Page
                if (MaxPage < 0) MaxPage = 0;
                // Lower boundary for Page Number
                if (PageNum < 0) PageNum = 0;
                // Upper Bound for Page Number
                if (PageNum > MaxPage) PageNum = MaxPage;

                // The Record Index of our Page Start
                int StartIndex = PerPage * PageNum;

                //Helps us generate the HTML which shows "Page 1 of ..." on the list view
                ViewData["PageNum"] = PageNum;
                ViewData["PageSummary"] = " " + (PageNum + 1) + " of " + (MaxPage + 1) + " ";

                // -- End of Pagination Algorithm --

                // Send back another request to get stffs, this time according to our paginated logic rules
                url = "StaffInfoData/GetStaffInfoesPage/" + StartIndex + "/" + PerPage;
                getstaffResponse = client.GetAsync(url).Result;

                // Retrieve the response of the HTTP Request
                IEnumerable<StaffInfoDto> SelectedStaffsPage = getstaffResponse.Content.ReadAsAsync<IEnumerable<StaffInfoDto>>().Result;


                //Return the paginated of products instead of the entire list
                return View(SelectedStaffsPage);

            }
            else
            {
                // If we reach here something went wrong with our list algorithm
                return RedirectToAction("Error");
            }
        }
            
        /// <summary>
        /// To gather staff and department information of particular staff with selected staff ID
        /// </summary>
        /// <param name="id">StaffId</param>
        /// <returns>Return information about slected StaffId. </returns>
        // GET: StaffInfo/Details/5
        public ActionResult Details(int id)
        {
            ShowStaff ViewModel = new ShowStaff();
            string findstaffurl = "StaffInfoData/FindStaff/" + id;
            HttpResponseMessage findstaffResponse = client.GetAsync(findstaffurl).Result;

            if (findstaffResponse.IsSuccessStatusCode)
            {
                //Sending findstaffResponse request to StaffInfo data controller, 
                //if request send succeed (status code 200), get this staff information (based on StaffInfoDto.)
                //If failed, direct to Error action (View)

                StaffInfoDto SelectedStaff = findstaffResponse.Content.ReadAsAsync<StaffInfoDto>().Result;
                ViewModel.staffinfo = SelectedStaff;

                //Find department for staff
                string finddepartmenturl = "StaffInfoData/FindDepartmentForStaff/" + id;
                HttpResponseMessage finddepertmentResponse = client.GetAsync(finddepartmenturl).Result;
                DepartmentDto DepartmentInfo = finddepertmentResponse.Content.ReadAsAsync<DepartmentDto>().Result;
                ViewModel.department = DepartmentInfo;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// Create a New Staff thru a form, includes first and last name, languages
        /// </summary>
        /// <returns>New staff request will be send and respond in POST Actions</returns>
        // GET: StaffInfo/Create 

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Get the information on Create Staff Form and update to the database
        /// </summary>
        /// <param name="StaffInfo"></param>
        /// <returns>If request succeed, New staff's information is created and added to the database.</returns>
        // POST: StaffInfo/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(StaffInfo StaffInfo)
        {
            GetApplicationCookie();
            //Sending addstaffresponse request to data controller (thru url string), 
            //If request send succeed (status code 200), add new staff information.
            // & redirect to the  Details controller method, and add id to the url parameters
            //If failed, direct to Error action (View)

            string addstaffurl = "StaffInfoData/AddStaff";

            HttpContent content = new StringContent(jss.Serialize(StaffInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage addstaffResponse = client.PostAsync(addstaffurl, content).Result;

            if (addstaffResponse.IsSuccessStatusCode)
            {

                int Staffid = addstaffResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Staffid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }
        /// <summary>
        /// Add new information of selected staff thru the GET method;
        /// Required to retrieved latest data before updates are available.
        /// </summary>
        /// <param name="id">StaffId</param>
        /// <returns>Retreive the data of selected staff and apply the changes and submit thru POST method.</returns>
        // GET: StaffInfo/Edit/5
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int id)
        {
            //Sending getupdatestaffResponse request to data controller (thru url string), 
            //If request send succeed (status code 200), get the staff information in edit view.
            //If failed, direct to Error action (View)
            UpdateStaff ViewModel = new UpdateStaff();
            GetApplicationCookie();
            string updatestaffurl = "StaffInfodata/findstaff/" + id;
            HttpResponseMessage getupdatestaffResponse = client.GetAsync(updatestaffurl).Result;

            if (getupdatestaffResponse.IsSuccessStatusCode)
            {
                //Put data into staff data transfer object
                StaffInfoDto SelectedStaff = getupdatestaffResponse.Content.ReadAsAsync<StaffInfoDto>().Result;
                ViewModel.staffinfo = SelectedStaff;

                string getdepartmenturl = " StaffInfodata/FindDepartmentForStaff/" + id;
                HttpResponseMessage getdepartmentResponse = client.GetAsync(getdepartmenturl).Result;
                DepartmentDto departments = getdepartmentResponse.Content.ReadAsAsync<DepartmentDto>().Result;
                ViewModel.department = departments;
                Debug.WriteLine(ViewModel);

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // <summary>
        /// Received the changes and apply the update to the selected staff
        /// </summary>
        /// <param name="id">staffID</param>
        /// <param name="StaffInfo"></param>
        /// <returns>THe selected staff information will be updated, include picture if have it</returns>
        // POST: StaffInfo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int id, StaffInfo StaffInfo, HttpPostedFileBase StaffPic)
        {

            string updatestaffurl = "StaffInfoData/UpdateStaff/" + id;
            GetApplicationCookie();
            HttpContent content = new StringContent(jss.Serialize(StaffInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage postupdatestaffResponse = client.PostAsync(updatestaffurl, content).Result;

            if (postupdatestaffResponse.IsSuccessStatusCode)
            {
                //only attempt to send Product picture data if we have it
                if (StaffPic != null)
                {
                    //Send over image data for staff
                    updatestaffurl = "StaffInfoData/UpdateStaffPic/" + id;

                    MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                    HttpContent imagecontent = new StreamContent(StaffPic.InputStream);
                    requestcontent.Add(imagecontent, "StaffPic", StaffPic.FileName);
                    postupdatestaffResponse = client.PostAsync(updatestaffurl, requestcontent).Result;
                }

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Delete selected staff with staffID
        /// </summary>
        /// <param name="id">staffID</param>
        /// <returns>The staff database will delete the staff record</returns>
        // GET: updatestaffurl/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();
            //Sending response request to staff data controller (thru url string), 
            //If request send succeed (status code 200), delete the staff information.
            // & redirect to the slected view.
            //If failed, direct to Error action (View)

            string getdeletestaffurl = "staffinfodata/findstaff/" + id;
            HttpResponseMessage getdeletestaffresponse = client.GetAsync(getdeletestaffurl).Result;

            if (getdeletestaffresponse.IsSuccessStatusCode)
            {
                //Put data into staff data transfer object
                StaffInfoDto SelectedStaff = getdeletestaffresponse.Content.ReadAsAsync<StaffInfoDto>().Result;
                return View(SelectedStaff);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Delete selected staff thru POST method
        /// </summary>
        /// <param name="id">StaffID</param>
        /// <returns>Selected staff record will be deleted and return to "List" Action to view the updated database</returns>
        // POST: StaffInfo/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int? id)
        {
            GetApplicationCookie();
            string url = "staffinfodata/deletestaff/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// Error Action
        /// </summary>
        /// <returns>It will display error view</returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}
