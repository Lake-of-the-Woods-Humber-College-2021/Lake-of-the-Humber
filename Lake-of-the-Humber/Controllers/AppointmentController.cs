using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Lake_of_the_Humber.Models;
using Lake_of_the_Humber.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;

namespace Lake_of_the_Humber.Controllers
{
    public class AppointmentController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        /// <summary>
        /// Allows access to pre-defined C# HttpClient 'client' variable for sending POST/GET request to data access layer
        /// </summary>
        static AppointmentController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };

            client = new HttpClient(handler);

            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

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
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Appointment/List
        [Authorize(Roles = "Admin,User")]
        public ActionResult List()
        {
            GetApplicationCookie();

            ListAppointment ViewModel = new ListAppointment();
            ViewModel.isadmin = User.IsInRole("Admin");
            ViewModel.isuser = User.IsInRole("User");

            if (User.IsInRole("Admin"))
            {
                //Gets all appointments if admin is logged in
                string url = "appointmentdata/getappointments";
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<AppointmentDto> SelectedAppointment = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
                    ViewModel.appointment = SelectedAppointment;

                    return View(ViewModel);
                }

                else
                {
                    return RedirectToAction("Error");
                }
            }

            else if (User.IsInRole("User"))
            {
                //Only gets appointments associated with user logged in, if not an admin
                var id = User.Identity.GetUserId();

                string url = "appointmentdata/getappointmentsforuser/" + id;
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<AppointmentDto> SelectedAppointment = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
                    ViewModel.appointment = SelectedAppointment;
                    return View(ViewModel);

                }

                else
                {
                    return RedirectToAction("Error");
                }

            }

            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Appointment/Details/5
        [Authorize(Roles = "Admin,User")]
        public ActionResult Details(int id)
        {

            GetApplicationCookie();

            ShowAppointment ViewModel = new ShowAppointment();

            ViewModel.isadmin = User.IsInRole("Admin");
            ViewModel.isuser = User.IsInRole("User");

            string url = "appointmentdata/findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Puts data into appointment DTO
                AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
                ViewModel.appointment = SelectedAppointment;

                //Finds doctors associated with an appointment, puts data into user DTO
                url = "appointmentdata/findstaffforappointment/" + id;
                response = client.GetAsync(url).Result;
                StaffInfoDto StaffInfo = response.Content.ReadAsAsync<StaffInfoDto>().Result;
                ViewModel.staffInfo = StaffInfo;

                //Finds user that 'owns' appointment, puts data into user DTO
                url = "appointmentdata/finduserforappointment/" + id;
                response = client.GetAsync(url).Result;
                ApplicationUser UserInfo = response.Content.ReadAsAsync<ApplicationUser>().Result;
                ViewModel.userInfo = UserInfo;

                //Finds user that owns appointment, puts data into user DTO
                //url = "appointmentdata/findappointment/" + id;
                //response = client.GetAsync(url).Result;
                //UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                //ViewModel.user = SelectedUser;

                if (User.IsInRole("Admin"))
                {
                    return View(ViewModel);
                }

                else if (User.IsInRole("User"))
                {
                    if (User.Identity.GetUserId() == SelectedAppointment.UserId)
                    {
                        return View(ViewModel);
                    }
                    else
                    {
                        return RedirectToAction("Error");
                    }
                }
                else
                {
                    return RedirectToAction("Error");
                }

            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Appointment/Create
        public ActionResult Create()
        {
            UpdateAppointment ViewModel = new UpdateAppointment();

            //WILL UPDATE WHEN STAFF DATA CONTROLLER IS MERGED
            string url = "staffinfodata/getstaffinfoes";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<StaffInfoDto> AllStaff = response.Content.ReadAsAsync<IEnumerable<StaffInfoDto>>().Result;
            ViewModel.allstaff = AllStaff;

            return View(ViewModel);
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment AppointmentInfo)
        {
            string url = "appointmentdata/addappointment";
            Debug.WriteLine(jss.Serialize(AppointmentInfo));
            Debug.WriteLine(DateTime.Now);
            HttpContent content = new StringContent(jss.Serialize(AppointmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                int appointmentid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = appointmentid });
            }
            else
            {
                Debug.WriteLine("Unable to add appointment");
                return RedirectToAction("Error");
            }
        }

        // GET: Appointment/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateAppointment ViewModel = new UpdateAppointment();

            string url = "appointmentdata/findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
                ViewModel.appointment = SelectedAppointment;

                //Checks if appointment date has passed. Will not allow edit if appointment has passed
                if (SelectedAppointment.AppDate > DateTime.Now)
                {
                    url = "appointmentdata/getstaffforappointment/" + id;
                    response = client.GetAsync(url).Result;

                    IEnumerable<StaffInfoDto> AllStaff = response.Content.ReadAsAsync<IEnumerable<StaffInfoDto>>().Result;
                    ViewModel.allstaff = AllStaff;
                    Debug.WriteLine(ViewModel);


                    return View(ViewModel);
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Appointment AppointmentInfo)
        {
            string url = "appointmentdata/updateappointment/" + id;
            HttpContent content = new StringContent(jss.Serialize(AppointmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Appointment/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "appointmentdata/findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {

                AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;

                //Checks if appointment date has passed. Will not allow edit if appointment has passed
                if (SelectedAppointment.AppDate > DateTime.Now)
                {
                    return View(SelectedAppointment);
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Appointment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "appointmentdata/deleteappointment/" + id;

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

        public ActionResult Error()
        {
            return View();
        }
    }
}
