﻿using Lake_of_the_Humber.Models;
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
    public class WellWishesController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static WellWishesController()
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

        // GET: WellWishes
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // GET: WellWishes/List
        [Authorize]
        public ActionResult List()
        {
            string url = "WellWishesData/GetAllWellWishes";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<WellWishDto> AllWellWishes = response.Content.ReadAsAsync<IEnumerable<WellWishDto>>().Result;
                return View(AllWellWishes);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            string url = "WellWishesData/GetWish/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                WellWishDto SelectedWish = response.Content.ReadAsAsync<WellWishDto>().Result;
                return View(SelectedWish);
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
        public ActionResult Create(WellWishDto Wish)
        {
            Wish.CreatorId = User.Identity.GetUserId();
            string url = "WellWishesData/AddWish";
            HttpContent content = new StringContent(jss.Serialize(Wish));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                // int WishId = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("List");
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
            string url = "WellWishesData/GetWish/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                WellWishDto SelectedWish = response.Content.ReadAsAsync<WellWishDto>().Result;
                return View(SelectedWish);
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
            string url = "WellWishesData/DeleteWish/" + id;
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

        public ActionResult Edit(int id)
        {

            // GET Wish Details
            string url = "WellWishesData/GetWish/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                WellWishDto SelectedWish = response.Content.ReadAsAsync<WellWishDto>().Result;
                string editUrl = "WellWishesData/UpdateWish/" + id;
                HttpContent content = new StringContent(jss.Serialize(SelectedWish));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage editResponse = client.PostAsync(editUrl, content).Result;

                if (editResponse.IsSuccessStatusCode)
                {
                    int WishId = editResponse.Content.ReadAsAsync<int>().Result;
                    return RedirectToAction("Details", new { id = WishId });
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

        public ActionResult Error()
        {
            return View();
        }
    }
}
