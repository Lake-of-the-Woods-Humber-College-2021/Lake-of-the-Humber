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
    public class InvoiceController : Controller
    {

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        /// <summary>
        /// Allows access to pre-defined C# HttpClient 'client' variable for sending POST/GET request to data access layer
        /// </summary>
        static InvoiceController()
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


        // GET: Invoice/List
        [Authorize(Roles = "Admin,User")]
        public ActionResult List()
        {
            GetApplicationCookie();

            ListInvoice ViewModel = new ListInvoice();
            ViewModel.isadmin = User.IsInRole("Admin");
            ViewModel.isuser = User.IsInRole("User");

            if (User.IsInRole("Admin"))
            {
                string url = "invoicedata/getinvoices";
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<InvoiceDto> SelectedInvoice = response.Content.ReadAsAsync<IEnumerable<InvoiceDto>>().Result;
                    ViewModel.invoices = SelectedInvoice;

                    return View(ViewModel);

                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            
            else if(User.IsInRole("User"))
            {
                var id = User.Identity.GetUserId();

                string url = "invoicedata/getinvoicesforuser/" + id;
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<InvoiceDto> SelectedInvoice = response.Content.ReadAsAsync<IEnumerable<InvoiceDto>>().Result;
                    Debug.WriteLine("Successful connection");
                    ViewModel.invoices = SelectedInvoice;
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


        // GET: Invoice/Details/5
        [Authorize(Roles = "Admin,User")]
        public ActionResult Details(int id)
        {
            GetApplicationCookie();

            ShowInvoice ViewModel = new ShowInvoice();
            ViewModel.isadmin = User.IsInRole("Admin");
            ViewModel.isuser = User.IsInRole("User");

            string url = "invoicedata/findinvoice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Puts data into invoice DTO
                InvoiceDto SelectedInvoice = response.Content.ReadAsAsync<InvoiceDto>().Result;
                ViewModel.invoice = SelectedInvoice;

                //Finds user that owns invoice, puts data into user DTO
                url = "invoicedata/finduserforinvoice/" + id;
                response = client.GetAsync(url).Result;
                ApplicationUser UserInfo = response.Content.ReadAsAsync<ApplicationUser>().Result;
                ViewModel.userInfo = UserInfo;

                Debug.WriteLine("First Checkpoint!");

                if (User.IsInRole("Admin"))
                {
                    return View(ViewModel);
                }

                else if (User.IsInRole("User"))
                {
                    if (User.Identity.GetUserId() == SelectedInvoice.UserId)
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


        // GET: Invoice/Create
        [Authorize(Roles = "Admin")] //Only Admins can create an invoice
        public ActionResult Create()
        {
            UpdateInvoice ViewModel = new UpdateInvoice();

            string url = "invoicedata/getusers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ApplicationUserDto> PotentialUsers = response.Content.ReadAsAsync<IEnumerable<ApplicationUserDto>>().Result;
            ViewModel.allusers = PotentialUsers;


            return View(ViewModel);
        }

        // POST: Invoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Invoice InvoiceInfo)
        {

            GetApplicationCookie();

            string url = "invoicedata/addinvoice";
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
        [Authorize(Roles="Admin")]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();

            UpdateInvoice ViewModel = new UpdateInvoice();

            string url = "invoicedata/findinvoice/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
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
        [Authorize(Roles="Admin")]
        public ActionResult Edit(int id, Invoice InvoiceInfo)
        {

           GetApplicationCookie();

            string url = "invoicedata/updateinvoice/" + id;
            Debug.WriteLine(jss.Serialize(InvoiceInfo));
            HttpContent content = new StringContent(jss.Serialize(InvoiceInfo));
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

        // GET: Invoice/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {

            GetApplicationCookie();

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
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {

            GetApplicationCookie();

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
