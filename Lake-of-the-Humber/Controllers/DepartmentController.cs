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
using Lake_of_the_Humber.Models.ViewModel;
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
            string listDepartmenturl = "Departmentdata/getDepartments";
            HttpResponseMessage Departmentlistresponse = client.GetAsync(listDepartmenturl).Result;
            if (Departmentlistresponse.IsSuccessStatusCode)
            {
                IEnumerable<DepartmentDto> SelectedDepartment = Departmentlistresponse.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
                return View(SelectedDepartment);
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
            string findDepartmenturl = "Departmentdata/findDepartment/" + id;
            HttpResponseMessage findDepartmentresponse = client.GetAsync(findDepartmenturl).Result;

            if (findDepartmentresponse.IsSuccessStatusCode)
            {
                //Sending findDepartmentresponse request to data controller, 
                //if request send succeed (status code 200), Please retienve me this Department information (based on DepartmentDto.)
                //If failed, direct to Error action (View)

                DepartmentDto SelectedDepartment = findDepartmentresponse.Content.ReadAsAsync<DepartmentDto>().Result;
                ViewModel.Department = SelectedDepartment;

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

            string addDepartmenturl = "DepartmentData/AddDepartment";

            HttpContent content = new StringContent(jss.Serialize(DepartmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage addDepartmentresponse = client.PostAsync(addDepartmenturl, content).Result;

            if (addDepartmentresponse.IsSuccessStatusCode)
            {

                int DepartmentId = addDepartmentresponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = DepartmentId });
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

            string getupdateDepartmenturl = "Departmentdata/findDepartment/" + id;
            HttpResponseMessage findDepartmentresponse = client.GetAsync(getupdateDepartmenturl).Result;

            if (findDepartmentresponse.IsSuccessStatusCode)
            {
                //Put data into DepartmentDto
                DepartmentDto SelectedDepartment = findDepartmentresponse.Content.ReadAsAsync<DepartmentDto>().Result;
                ViewModel.Department = SelectedDepartment;

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
            //Sending postupdateDepartmentresponse request to data controller (thru url string), 
            //If request send succeed (status code 200), Please save the changes (update) the Department information.
            // & redirect to the  Details controller method, and add id to the url parameters
            //If failed, direct to Error action (View)
            string postupdateDepartmenturl = "Departmentdata/updateDepartment/" + id;

            HttpContent content = new StringContent(jss.Serialize(DepartmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage updateDepartmentresponse = client.PostAsync(postupdateDepartmenturl, content).Result;

            if (updateDepartmentresponse.IsSuccessStatusCode)
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

            string getdeleteDepartmenturl = "Departmentdata/findDepartment/" + id;
            HttpResponseMessage getdeleteDepartmentresponse = client.GetAsync(getdeleteDepartmenturl).Result;

            if (getdeleteDepartmentresponse.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                DepartmentDto SelectedDepartment = getdeleteDepartmentresponse.Content.ReadAsAsync<DepartmentDto>().Result;
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
            string postdeleteDepartmenturl = "Departmentdata/deleteDepartment/" + id;

            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(postdeleteDepartmenturl, content).Result;

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

