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


        // GET: Product/List?{PageNum}
        // If the page number is not included, set it to 0
        public ActionResult List(int PageNum = 0)
        {
            // Grab all products
            string url = "productdata/getproducts";
            // Send off an HTTP request
            // GET : /api/productdata/getproducts
            // Retrieve response
            HttpResponseMessage response = client.GetAsync(url).Result;
            // If the response is a success, proceed
            if (response.IsSuccessStatusCode)
            {
                // Fetch the response content into IEnumerable<ProductDto>
                IEnumerable<ProductDto> SelectedProducts = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;

                // -- Start of Pagination Algorithm --

                // Find the total number of products
                int ProductCount = SelectedProducts.Count();
                // Number of products to display per page
                int PerPage = 8;
                // Determines the maximum number of pages (rounded up), assuming a page 0 start.
                int MaxPage = (int)Math.Ceiling((decimal)ProductCount / PerPage) - 1;

                // Lower boundary for Max Page
                if (MaxPage < 0) MaxPage = 0;
                // Lower boundary for Page Number
                if (PageNum < 0) PageNum = 0;
                // Upper Bound for Page Number
                if (PageNum > MaxPage) PageNum = MaxPage;

                // The Record Index of our Page Start
                int StartIndex = PerPage * PageNum;

                //Helps us generate the HTML which shows "Page 1 of ..." on the list view
                ViewData["PageNum"] = PageNum;
                ViewData["PageSummary"] = " " + (PageNum + 1) + " of " + (MaxPage + 1) + " ";

                // -- End of Pagination Algorithm --


                // Send back another request to get products, this time according to our paginated logic rules
                // GET api/productdata/getproductspage/{startindex}/{perpage}
                url = "productdata/getproductspage/" + StartIndex + "/" + PerPage;
                response = client.GetAsync(url).Result;

                // Retrieve the response of the HTTP Request
                IEnumerable<ProductDto> SelectedProductsPage = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;

                return View(SelectedProductsPage);

            }
            else
            {
                // If we reach here something went wrong with our list algorithm
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
        // only administrators get to this page
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
                //return View(SelectedProduct);
                ViewModel.product = SelectedProduct;
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Product ProductInfo, HttpPostedFileBase ProductPic)
        {
            Debug.WriteLine(ProductInfo.ProductName);
            string url = "productdata/updateproduct/" + id;
            Debug.WriteLine(jss.Serialize(ProductInfo));
            HttpContent content = new StringContent(jss.Serialize(ProductInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                //only attempt to send product picture data if we have it
                if (ProductPic != null)
                {
                    Debug.WriteLine("Calling Update Image method.");
                    //Send over image data for product
                    url = "productdata/updateproductpic/" + id;
                    //Debug.WriteLine("Received product picture "+ProductPic.FileName);

                    MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                    HttpContent imagecontent = new StreamContent(ProductPic.InputStream);
                    requestcontent.Add(imagecontent, "ProductPic", ProductPic.FileName);
                    response = client.PostAsync(url, requestcontent).Result;
                }

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Product/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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