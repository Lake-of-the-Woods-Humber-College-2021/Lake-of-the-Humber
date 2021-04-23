using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;
using Lake_of_the_Humber.Models;
using Lake_of_the_Humber.Models.ViewModels;
using System.Web.Mvc;
using System;
using System.Web;
using System.Collections.Generic;


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
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }

        // GET: StaffInfo/List
        public ActionResult List()
        {
            string url = "staffinfodata/getstaffinfos";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<StaffInfoDto> SelectedStaffInfos = response.Content.ReadAsAsync<IEnumerable<StaffInfoDto>>().Result;
                return View(SelectedStaffInfos);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// To gather information of particular staff with selected staff ID
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
                string finddepartmenturl = "departmentdata/FindDepartmentForStaff/" + id;
                HttpResponseMessage finddepertmentResponse = client.GetAsync(finddepartmenturl).Result;
                DepartmentDto SelectedDepartment = finddepertmentResponse.Content.ReadAsAsync<DepartmentDto>().Result;
                ViewModel.department = SelectedDepartment;

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
        public ActionResult Create(StaffInfo StaffInfo)
        {
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
        public ActionResult Edit(int id)
        {
            //Sending getupdatestaffResponse request to data controller (thru url string), 
            //If request send succeed (status code 200), get the staff information in edit view.
            //If failed, direct to Error action (View)
            UpdateStaff ViewModel = new UpdateStaff();

            string updatestaffurl = "StaffInfodata/findstaff/" + id;
            HttpResponseMessage getupdatestaffResponse = client.GetAsync(updatestaffurl).Result;

            if (getupdatestaffResponse.IsSuccessStatusCode)
            {
                //Put data into staff data transfer object
                StaffInfoDto SelectedStaff = getupdatestaffResponse.Content.ReadAsAsync<StaffInfoDto>().Result;
                ViewModel.staffinfo = SelectedStaff;

                string getdepartmenturl = " StaffInfo/FindDepartmentForStaff/" + id;
                HttpResponseMessage response = client.GetAsync(getdepartmenturl).Result;
                IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
                ViewModel.alldepartments = departments;

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
        public ActionResult Edit(int id, StaffInfo StaffInfo, HttpPostedFileBase StaffPic)
        {

            string updatestaffurl = "staffinfodata/updatestaff/" + id;

            HttpContent content = new StringContent(jss.Serialize(StaffInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage postupdatestaffResponse = client.PostAsync(updatestaffurl, content).Result;

            if (postupdatestaffResponse.IsSuccessStatusCode)
            {
                //only attempt to send Product picture data if we have it
                if (StaffPic != null)
                {
                    //Send over image data for staff
                    updatestaffurl = "staffinfodata/updatestaffpic/" + id;

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
        public ActionResult DeleteConfirm(int id)
        {
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
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int? id)
        {
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
