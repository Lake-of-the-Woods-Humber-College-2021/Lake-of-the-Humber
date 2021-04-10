
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


namespace Lake_of_the_Humber.Controllers
{
    public class FaqController : Controller
    {
        // GET: Faq
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        static FaqController()
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


        }

        // GET: Faq/List
        public ActionResult List()
        {
            string url = "FaqData/GetFaqs";// from data controller GetFaqs
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<FaqDto> SelectedFaq = response.Content.ReadAsAsync<IEnumerable<FaqDto>>().Result;
                return View(SelectedFaq);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}