using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lake_of_the_Humber.Models;
using Lake_of_the_Humber.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;


namespace Lake_of_the_Humber.Controllers
{
    public class ProductController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static ProductController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        }


        // GET: Product/List
        public ActionResult List()
        {
            string url = "productdata/getproducts";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ProductDto> SelectedProducts = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;
                return View(SelectedProducts);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            ShowProduct ViewModel = new ShowProduct();
            string url = "productdata/findproduct/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Product data transfer object
                ProductDto SelectedProduct = response.Content.ReadAsAsync<ProductDto>().Result;
                ViewModel.product = SelectedProduct;

                url = "productdata/getordersforproduct/" + id;
                response = client.GetAsync(url).Result;
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                IEnumerable<OrderDto> SelectedOrders = response.Content.ReadAsAsync<IEnumerable<OrderDto>>().Result;
                ViewModel.productorders = SelectedOrders;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Product ProductInfo)
        {
            Debug.WriteLine(ProductInfo.ProductName);
            string url = "Productdata/addProduct";
            Debug.WriteLine(jss.Serialize(ProductInfo));
            HttpContent content = new StringContent(jss.Serialize(ProductInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int Productid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Productid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateProduct ViewModel = new UpdateProduct();

            string url = "productdata/findproduct/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Product data transfer object
                ProductDto SelectedProduct = response.Content.ReadAsAsync<ProductDto>().Result;
                return View(SelectedProduct);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Product ProductInfo)
        {
            Debug.WriteLine(ProductInfo.ProductName);
            string url = "productdata/updateproduct/" + id;
            Debug.WriteLine(jss.Serialize(ProductInfo));
            HttpContent content = new StringContent(jss.Serialize(ProductInfo));
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

        // GET: Product/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "productdata/findproduct/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Product data transfer object
                ProductDto SelectedProduct = response.Content.ReadAsAsync<ProductDto>().Result;
                return View(SelectedProduct);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "productdata/deleteproduct/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
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