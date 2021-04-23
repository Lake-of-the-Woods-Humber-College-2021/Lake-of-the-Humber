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
            //warning function ignoring:
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
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

        //GET Faq/Details/6
        public ActionResult Details(int id)
        {
            ShowFaq ViewModels = new ShowFaq();
            string FaqDetailurl = "Faqdata/findFaq/" + id;

            HttpResponseMessage findFaqresponse = client.GetAsync(FaqDetailurl).Result;

            if (findFaqresponse.IsSuccessStatusCode)
            {

                FaqDto SelectedFaq = findFaqresponse.Content.ReadAsAsync<FaqDto>().Result;
                ViewModels.Faq = SelectedFaq;

                return View(ViewModels);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //Get Request Faq/Delete/9
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string GetFaqDeleteUrl = "Faqdata/findFaq/" + id;//find the relevent faq
            HttpResponseMessage DeleteFaqResponse = client.GetAsync(GetFaqDeleteUrl).Result;

            if (DeleteFaqResponse.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                FaqDto SelectedFaq = DeleteFaqResponse.Content.ReadAsAsync<FaqDto>().Result;
                return View(SelectedFaq);
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
            string PostFaqDeleteUrl = "Faqdata/deleteFaq/" + id;

            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(PostFaqDeleteUrl, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //ADD/CREATE new FAQ
        // GET:Faq/Create 
        [HttpGet]
        public ActionResult Create()//FORM Imputs from view
        {
            return View();
        }
        //POST Faq/Create
        //Posts to DB and creats new FAQ if form entries are correct
        //send request to Data controller. 

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Faq FaqInfo)
        {
            string AddFaqUrl = "FaqData/AddFaq"; //AddFaq model created in data controller

            HttpContent content = new StringContent(jss.Serialize(FaqInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage AddFaqResponse = client.PostAsync(AddFaqUrl, content).Result;

            if (AddFaqResponse.IsSuccessStatusCode)
            {

                int FaqId = AddFaqResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = FaqId });
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        //EDIT FAQS callls on update 
        //GET: Faq/Edit/5
        public ActionResult Edit(int id)
        {

            UpdateFaq ViewModels = new UpdateFaq();

            string GetUpdateFaqUrl = "FaqData/findFaq/" + id;//locate faq by id
            HttpResponseMessage FindFaqResponse = client.GetAsync(GetUpdateFaqUrl).Result;

            if (FindFaqResponse.IsSuccessStatusCode)
            {
                //Put data into DepartmentDto
                FaqDto SelectedFaq = FindFaqResponse.Content.ReadAsAsync<FaqDto>().Result;
                ViewModels.Faq = SelectedFaq;

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
        public ActionResult Edit(int id, Faq FaqInfo)
        {
            string PostUpdateFaqUrl = "FaqData/UpdateFaq/" + id;
            Debug.WriteLine(FaqInfo);
            HttpContent content = new StringContent(jss.Serialize(FaqInfo));
            Debug.WriteLine(FaqInfo);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage UpdateFaqResponse = client.PostAsync(PostUpdateFaqUrl, content).Result;

            if (UpdateFaqResponse.IsSuccessStatusCode)
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