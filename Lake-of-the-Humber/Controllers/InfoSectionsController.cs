using Lake_of_the_Humber.Models;
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
                AllowAutoRedirect = false
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
        [Authorize]
        public ActionResult List()
        {
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

        [Authorize]
        public ActionResult Details(int id)
        {
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

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(InfoSectionDto Section)
        {
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
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
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
        [Authorize]
        public ActionResult Delete(int id)
        {
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

        [Authorize]
        public ActionResult Edit(int id)
        {

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
        [Authorize]
        public ActionResult Edit(int id, InfoSectionDto Section)
        {
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
