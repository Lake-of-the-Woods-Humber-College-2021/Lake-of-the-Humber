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
    public class OrderController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static OrderController()
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



        // GET: Order/List
        public ActionResult List()
        {
            string url = "orderdata/getorders";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<OrderDto> SelectedOrders = response.Content.ReadAsAsync<IEnumerable<OrderDto>>().Result;
                return View(SelectedOrders);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            UpdateOrder ViewModel = new UpdateOrder();

            string url = "orderdata/findorder/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Order data transfer object
                OrderDto SelectedOrder = response.Content.ReadAsAsync<OrderDto>().Result;
                ViewModel.order = SelectedOrder;

                //find products that are Ordered by this Order
                url = "orderdata/getproductsfororder/" + id;
                response = client.GetAsync(url).Result;

                //Put data into Order data transfer object
                IEnumerable<ProductDto> SelectedProducts = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;
                ViewModel.orderedproducts = SelectedProducts;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");

            }

        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Order OrderInfo)
        {
            Debug.WriteLine(OrderInfo.OrderName);
            string url = "orderdata/addorder";
            Debug.WriteLine(jss.Serialize(OrderInfo));
            HttpContent content = new StringContent(jss.Serialize(OrderInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int Orderid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Orderid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateOrder ViewModel = new UpdateOrder();

            string url = "orderdata/findorder/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Order data transfer object
                OrderDto SelectedOrder = response.Content.ReadAsAsync<OrderDto>().Result;
                ViewModel.order = SelectedOrder;

                //find products that are ordered by this order
                url = "orderdata/getproductsfororder/" + id;
                response = client.GetAsync(url).Result;

                //Put data into Product data transfer object
                IEnumerable<ProductDto> SelectedProducts = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;
                ViewModel.orderedproducts = SelectedProducts;

                //find products that are not ordered by this order
                url = "orderdata/getproductsnotordered/" + id;
                response = client.GetAsync(url).Result;

                //put data into data transfer object
                IEnumerable<ProductDto> UnorderedProducts = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;
                ViewModel.allproducts = UnorderedProducts;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");

            }
        }

        // GET:  Ordered/Unorder/productid/orderid
        [HttpGet]
        [Route("Order/Unorder/{productid}/{orderid}")]

        public ActionResult Unorder(int productid, int orderid)
        {
            string url = "orderdata/unorder/" + productid + "/" + orderid;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Edit", new { id = orderid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: order/order
        // First Order is the noun (the Order themselves)
        // second orderis the verb (the act of ordering)
        // The order(1) orders(2) a product
        [HttpPost]
        [Route("Order/order/{productid}/{orderid}")]
        public ActionResult Order(int productid, int orderid)
        {
            string url = "orderdata/order/" + productid + "/" + orderid;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Edit", new { id = orderid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Order OrderInfo)
        {
            Debug.WriteLine(OrderInfo.OrderName);
            string url = "orderdata/updateorder/" + id;
            Debug.WriteLine(jss.Serialize(OrderInfo));
            HttpContent content = new StringContent(jss.Serialize(OrderInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("update Order request succeeded");
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                Debug.WriteLine("update Order request failed");
                Debug.WriteLine(response.StatusCode.ToString());
                return RedirectToAction("Error");
            }
        }

        // GET: Order/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "orderdata/findorder/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Order data transfer object
                OrderDto SelectedOrder = response.Content.ReadAsAsync<OrderDto>().Result;
                return View(SelectedOrder);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Order/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "orderdata/deleteorder/" + id;
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