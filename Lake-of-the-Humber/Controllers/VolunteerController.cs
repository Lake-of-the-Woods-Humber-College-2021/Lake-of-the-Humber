using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Lake_of_the_Humber.Models.ViewModels;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;
using Lake_of_the_Humber.Models;
using System.Net;
using Microsoft.AspNet.Identity;

namespace Lake_of_the_Humber.Controllers
{
    public class VolunteerController : Controller
    {
        // GET: Volunteer
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        static VolunteerController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //My Port # is 44336
            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //warning function ignoring:
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }
        // GET: Volunteer/List
        public ActionResult List()
        {
            string url = "VolunteerData/GetVolunteers";// from data controller GetVolunteers

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<VolunteerDto> SelectedVolunteer = response.Content.ReadAsAsync<IEnumerable<VolunteerDto>>().Result;
                return View(SelectedVolunteer);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Volunteer/Details/2
       
        public ActionResult Details(int id)
        {
            ShowVolunteer ViewModels = new ShowVolunteer();
            string VolunteerDetailurl = "Volunteerdata/findVolunteer/" + id;

            HttpResponseMessage findVolunteerresponse = client.GetAsync(VolunteerDetailurl).Result;

            if (findVolunteerresponse.IsSuccessStatusCode)
            {

                VolunteerDto SelectedVolunteer = findVolunteerresponse.Content.ReadAsAsync<VolunteerDto>().Result;
                ViewModels.Volunteer = SelectedVolunteer;

                return View(ViewModels);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

      

        //Get Request Volunteer/Delete/3
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string GetVolunteerDeleteUrl = "Volunteerdata/findVolunteer/" + id;//find the relevent Volunteer
            HttpResponseMessage DeleteVolunteerResponse = client.GetAsync(GetVolunteerDeleteUrl).Result;

            if (DeleteVolunteerResponse.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                VolunteerDto SelectedVolunteer = DeleteVolunteerResponse.Content.ReadAsAsync<VolunteerDto>().Result;
                return View(SelectedVolunteer);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //POST REQUEST TO DELETE
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string PostVolunteerDeleteUrl = "Volunteerdata/deleteVolunteer/" + id;

            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(PostVolunteerDeleteUrl, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //ADD/CREATE new Volunteer
        // GET:Volunteer/Create 
        [HttpGet]
        public ActionResult Create()//FORM Imputs from view
        {
            return View();
        }
        //POST Volunteer/Create
        //Posts to DB and creats new Volunteer if form entries are correct
        //send request to Data controller. 

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Volunteer VolunteerInfo)
        {
            string AddVolunteerUrl = "VolunteerData/AddVolunteer"; //AddVolunteer model created in data controller

            VolunteerInfo.CreatorId = User.Identity.GetUserId();

            HttpContent content = new StringContent(jss.Serialize(VolunteerInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage AddVolunteerResponse = client.PostAsync(AddVolunteerUrl, content).Result;

            if (AddVolunteerResponse.IsSuccessStatusCode)
            {

                int VolunteerId = AddVolunteerResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = VolunteerId });
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        //EDIT Volunteer callls on update 
        //GET: Volunteer/Edit/5
        public ActionResult Edit(int id)
        {

            UpdateVolunteer ViewModels = new UpdateVolunteer();

            string GetUpdateVolunteerUrl = "VolunteerData/findVolunteer/" + id;//locate Volunteer by id
            HttpResponseMessage FindVolunteerResponse = client.GetAsync(GetUpdateVolunteerUrl).Result;

            if (FindVolunteerResponse.IsSuccessStatusCode)
            {
                //Put data into DepartmentDto
                VolunteerDto SelectedVolunteer = FindVolunteerResponse.Content.ReadAsAsync<VolunteerDto>().Result;
                ViewModels.Volunteer = SelectedVolunteer;

                return View(ViewModels);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //POST UPDATE
        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Volunteer VolunteerInfo)
        {
            string PostUpdateVolunteerUrl = "VolunteerData/UpdateVolunteer/" + id;
            VolunteerInfo.CreatorId = User.Identity.GetUserId();
            Debug.WriteLine(VolunteerInfo);
            HttpContent content = new StringContent(jss.Serialize(VolunteerInfo));
            Debug.WriteLine(VolunteerInfo);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage UpdateVolunteerResponse = client.PostAsync(PostUpdateVolunteerUrl, content).Result;

            if (UpdateVolunteerResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

    }
}
