using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Lake_of_the_Humber.Models.ViewModels;
using Lake_of_the_Humber.Models;

namespace Lake_of_the_Humber.Controllers
{
    public class DepartmentController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static DepartmentController()
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

        // <summary>
        /// To retrieve data of all Department in the database in a list form
        /// </summary>
        /// <returns>Return a List of Department with their information else return to Error view.</returns>
        // GET: Department/List  
        public ActionResult List()
        {
            //Sending Departmentlistresponse request to data controller, if request send succeed (status code 200), a list of Department information will displayed.
            //If failed, direct to Error action (View)
            string url = "departmentdata/getdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<DepartmentDto> SelectedDepartments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
                return View(SelectedDepartments);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// To gather information of particular Department with selected DepartmentId
        /// </summary>
        /// <param name="id">DepartmentId</param>
        /// <returns>Return particular Department information. Using View Modal, set a limit on what can be view by the client side and also able to set [DisplayName] to change the column name that will appaered in the View.  </returns>
        // GET: Department/Details/5
        public ActionResult Details(int id)
        {
            ShowDepartment ViewModel = new ShowDepartment();
            string finddepartmenturl = "departmentdata/finddepartment/" + id;
            HttpResponseMessage findDepartmentresponse = client.GetAsync(finddepartmenturl).Result;

            if (findDepartmentresponse.IsSuccessStatusCode)
            {
                //Sending findDepartmentresponse request to data controller, 
                //if request send succeed (status code 200), Please retienve me this Department information (based on DepartmentDto.)
                //If failed, direct to Error action (View)

                DepartmentDto SelectedDepartment = findDepartmentresponse.Content.ReadAsAsync<DepartmentDto>().Result;
                ViewModel.department = SelectedDepartment;

                string url = "departmentdata/getstaffsfordepartment/" + id;
                findDepartmentresponse = client.GetAsync(url).Result;
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                Debug.WriteLine(findDepartmentresponse.Content.ReadAsAsync<IEnumerable<StaffInfoDto>>().Result);
                IEnumerable<StaffInfoDto> SelectedStaffs = findDepartmentresponse.Content.ReadAsAsync<IEnumerable<StaffInfoDto>>().Result;
                ViewModel.staffInfoes = SelectedStaffs;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// Create a New Department thru a form
        /// </summary>
        /// <returns>New Department request will be send and respond in POST Actions</returns>
        // GET: Department/Create 
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Get the information on Create Department Form and update to the database
        /// </summary>
        /// <param name="DepartmentInfo"></param>
        /// <returns>If request succeed, New Department's information is created and added to the database.</returns>
        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Department DepartmentInfo)
        {
            //Sending addDepartmentresponse request to data controller (thru url string), 
            //If request send succeed (status code 200), Please add new Department information.
            // & redirect to the  Details controller method, and add id to the url parameters
            //If failed, direct to Error action (View)

            Debug.WriteLine(DepartmentInfo.DepartmentName);
            string url = "departmentdata/addDepartment";
            Debug.WriteLine(jss.Serialize(DepartmentInfo));
            HttpContent content = new StringContent(jss.Serialize(DepartmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int Departmentid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Departmentid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }
        /// <summary>
        /// Add new information of selected Department thru the GET method;
        /// Required to retrieved latest data before updates are available.
        /// </summary>
        /// <param name="id">DepartmentId</param>
        /// <returns>Retreive the data of selected Department and apply the changes and submit thru POST method.</returns>
        // GET: Department/Edit/5
        public ActionResult Edit(int id)
        {
            //Sending getupdateDepartmentresponse request to data controller (thru url string), 
            //If request send succeed (status code 200), Please retrieve the Department information in edit view.
            //If failed, direct to Error action (View)
            UpdateDepartment ViewModel = new UpdateDepartment();

            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Product data transfer object
                DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                ViewModel.department = SelectedDepartment;

                url = "departmentdata/getstaffsfordepartment/" + id;
                response = client.GetAsync(url).Result;

                IEnumerable<StaffInfoDto> AllStaff = response.Content.ReadAsAsync<IEnumerable<StaffInfoDto>>().Result;
                ViewModel.staffInfoes = AllStaff;
                Debug.WriteLine(ViewModel);

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// Received the changes and apply the update to the selected Department
        /// </summary>
        /// <param name="id">DepartmentId</param>
        /// <param name="DepartmentInfo"></param>
        /// <returns>THe selected Department information will be updated</returns>
        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Department DepartmentInfo)
        {
            Debug.WriteLine(DepartmentInfo.DepartmentName);
            string url = "departmentdata/updatedepartment/" + id;
            Debug.WriteLine(jss.Serialize(DepartmentInfo));
            HttpContent content = new StringContent(jss.Serialize(DepartmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        /// <summary>
        /// Delete selected Department with DepartmentId
        /// </summary>
        /// <param name="id">DepartmentId</param>
        /// <returns>The Department database will delete the Department record</returns>
        // GET: Department/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            //Sending getudeleteDepartmentresponse request to data controller (thru url string), 
            //If request send succeed (status code 200), delete the Department information.
            // & redirect to the slected Department view.
            //If failed, direct to Error action (View)

            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Product data transfer object
                DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                return View(SelectedDepartment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// Delete selected Department thru POST method
        /// </summary>
        /// <param name="id">DepartmentId</param>
        /// <returns>Selected Department record will be deleted and return to "List" Action to view the updated database</returns>
        // POST: Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "departmentdata/deletedepartment/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
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

