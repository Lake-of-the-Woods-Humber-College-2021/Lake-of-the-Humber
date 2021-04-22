using Lake_of_the_Humber.Models;
using Lake_of_the_Humber.Models.ViewModels.InfoSections;
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

namespace Lake_of_the_Humber.Controllers
{
    public class InfoSectionsController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        static InfoSectionsController()
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
        private void GetApplicationCookie()
        {
            string token = "";
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // GET: WellWishes/List
        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            GetApplicationCookie();
            string url = "InfoSectionsData/GetAllSections";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<InfoSectionDto> AllInfoSections = response.Content.ReadAsAsync<IEnumerable<InfoSectionDto>>().Result;
                return View(AllInfoSections);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            GetApplicationCookie();
            string url = "InfoSectionsData/GetSection/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                InfoSectionDto SelectedSection = response.Content.ReadAsAsync<InfoSectionDto>().Result;
                return View(SelectedSection);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            UpdateInfoSection ViewModel = new UpdateInfoSection();
            string url = "DepartmentData/GetDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> PotentialDepartments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewModel.allDepartments = PotentialDepartments;
            return View(ViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(InfoSectionDto Section)
        {
            Debug.WriteLine("1.DepartmentId:-- " + Section.DepartmentId);
            GetApplicationCookie();
            Section.CreatorId = User.Identity.GetUserId();
            string url = "InfoSectionsData/AddSection";
            HttpContent content = new StringContent(jss.Serialize(Section));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int SectionId = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = SectionId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();
            string url = "InfoSectionsData/GetSection/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                InfoSectionDto SelectedSection = response.Content.ReadAsAsync<InfoSectionDto>().Result;
                return View(SelectedSection);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "InfoSectionsData/DeleteSection/" + id;
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

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();
            string url = "InfoSectionsData/GetSection/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                InfoSectionDto SelectedSection = response.Content.ReadAsAsync<InfoSectionDto>().Result;
                return View(SelectedSection);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, InfoSectionDto Section)
        {
            GetApplicationCookie();
            string url = "InfoSectionsData/UpdateSection/" + id;
            HttpContent content = new StringContent(jss.Serialize(Section));
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

        public ActionResult Error()
        {
            return View();
        }
    }
}
