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
    public class InvoiceController : Controller
    {

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static InvoiceController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };

            client = new HttpClient(handler);

            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        // GET: Invoice/List
        public ActionResult List()
        {
            string url = "invoicedata/getinvoices";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<InvoiceDto> SelectedInvoice = response.Content.ReadAsAsync<IEnumerable<InvoiceDto>>().Result;
                Debug.WriteLine("Successful connection");

                return View(SelectedInvoice);

                //return View(search == null ? SelectedInvoice :
                //  SelectedInvoice.Where(x => x.InvoiceTitle.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList());
            }

            else
            {
                Debug.WriteLine("Could not connect");
                return RedirectToAction("Error");
            }
        }

        // GET: Invoice/Details/5
        public ActionResult Details(int id)
        {
            ShowInvoice ViewModel = new ShowInvoice();
            string url = "invoicedata/findinvoice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Puts data into invoice DTO
                InvoiceDto SelectedInvoice = response.Content.ReadAsAsync<InvoiceDto>().Result;
                ViewModel.invoice = SelectedInvoice;

                //Finds user that owns invoice, buts data into user DTO
                url = "invoicedata/finduserforinvoice/" + id;
                response = client.GetAsync(url).Result;
                ApplicationUser UserInfo = response.Content.ReadAsAsync<ApplicationUser>().Result;
                ViewModel.userInfo = UserInfo;

                return View(ViewModel);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Invoice/Create
        public ActionResult Create()
        {
            UpdateInvoice ViewModel = new UpdateInvoice();

            return View(ViewModel);
        }

        // POST: Invoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Invoice InvoiceInfo)
        {
            Debug.WriteLine(InvoiceInfo.InvoiceTitle);
            string url = "invoicedata/addinvoice";
            Debug.WriteLine(jss.Serialize(InvoiceInfo));
            Debug.WriteLine(DateTime.Now);
            HttpContent content = new StringContent(jss.Serialize(InvoiceInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                int invoiceid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = invoiceid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Invoice/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateInvoice ViewModel = new UpdateInvoice();

            string url = "invoicedata/findinvoice/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(DateTime.Now);
                InvoiceDto SelectedInvoice = response.Content.ReadAsAsync<InvoiceDto>().Result;
                ViewModel.invoice = SelectedInvoice;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Invoice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Invoice InvoiceInfo)
        {
            Debug.WriteLine(InvoiceInfo.InvoiceTitle);
            string url = "invoicedata/updateinvoice/" + id;
            Debug.WriteLine(jss.Serialize(InvoiceInfo));
            Debug.WriteLine(DateTime.Now);
            HttpContent content = new StringContent(jss.Serialize(InvoiceInfo));
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

        // GET: Invoice/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "invoicedata/findinvoice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                InvoiceDto SelectedInvoice = response.Content.ReadAsAsync<InvoiceDto>().Result;
                return View(SelectedInvoice);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Invoice/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "invoicedata/deleteinvoice/" + id;

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
