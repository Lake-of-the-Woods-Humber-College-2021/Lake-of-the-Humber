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

namespace Lake_of_the_Humber.Controllers
{
    public class AppointmentController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static AppointmentController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };

            client = new HttpClient(handler);

            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        // GET: Appointment/List
        public ActionResult List()
        {
            ListAppointment ViewModel = new ListAppointment();
            string url = "appointmentdata/getappointments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<AppointmentDto> SelectedAppointment = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
                ViewModel.appointment = SelectedAppointment;

                return View(ViewModel);

                //return View(search == null ? SelectedAppointment :
                //  SelectedAppointment.Where(x => x.AppPurpose.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList());
            }

            else
            {
                Debug.WriteLine("Could not connect");
                return RedirectToAction("Error");
            }
        }
        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
            ShowAppointment ViewModel = new ShowAppointment();
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

                url = "appointmentdata/finduserforappointment/" + id;
                response = client.GetAsync(url).Result;
                ApplicationUser UserInfo = response.Content.ReadAsAsync<ApplicationUser>().Result;
                ViewModel.userInfo = UserInfo;

                //Finds user that owns appointment, puts data into user DTO
                //url = "appointmentdata/findappointment/" + id;
                //response = client.GetAsync(url).Result;
                //UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                //ViewModel.user = SelectedUser;



                return View(ViewModel);

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
            //string url = "staffinfodata/getstaffinfo";
            //HttpResponseMessage response = client.GetAsync(url).Result;

            //IEnumerable<StaffInfoDto> AllStaff = response.Content.ReadAsAsync<IEnumerable<StaffInfoDto>>().Result;
            //ViewModel.allstaff = AllStaff;

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
                Debug.WriteLine(DateTime.Now);
                AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
                ViewModel.appointment = SelectedAppointment;

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

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Appointment AppointmentInfo)
        {
            string url = "appointmentdata/updateappointment/" + id;
            Debug.WriteLine(jss.Serialize(AppointmentInfo));
            Debug.WriteLine(DateTime.Now);
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
                return View(SelectedAppointment);
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
