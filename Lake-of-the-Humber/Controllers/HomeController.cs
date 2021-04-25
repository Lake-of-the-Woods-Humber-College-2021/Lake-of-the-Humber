using Lake_of_the_Humber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Lake_of_the_Humber.Controllers
{
    public class HomeController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        static HomeController()
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
        public ActionResult Index()
        {
            string url = "InfoSectionsData/GetAllSections";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<InfoSectionDto> SelectedInformationSections = response.Content.ReadAsAsync<IEnumerable<InfoSectionDto>>().Result;
                return View(SelectedInformationSections);
            }
            else
            {
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}