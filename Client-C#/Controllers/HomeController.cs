using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lab7.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Net;

namespace Lab7.Controllers
{
    public class HomeController : Controller
    {


        //private string serviceUrl = "http://localhost:56199/restaurantreview";
        //private string serviceUrl = "http://192.168.0.28/api/restaurantreview";

        private string serviceUrl = "http://localhost/RestaurantReviewServiceApi/RestaurantReview";

        public IActionResult Index()
        {
            HttpClient httpClient = new HttpClient();

            using var response = httpClient.GetAsync(serviceUrl).Result;

            if(response.StatusCode == HttpStatusCode.OK)
            {
                string apiResponse = response.Content.ReadAsStringAsync().Result;

                List<RestaurantInfo> restaurantInfos = JsonSerializer.Deserialize<List<RestaurantInfo>>(apiResponse);

                return View(restaurantInfos);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound( );
            }

            HttpClient httpClient = new HttpClient();

            using var response = httpClient.GetAsync(serviceUrl + '/' + id).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string apiResponse = response.Content.ReadAsStringAsync().Result;

                RestaurantInfo restaurantInfo = JsonSerializer.Deserialize<RestaurantInfo>(apiResponse);

                return View(restaurantInfo);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public IActionResult Edit(RestaurantInfo restInfo)
        {
            HttpClient httpClient = new HttpClient();

            string restInfoString = JsonSerializer.Serialize<RestaurantInfo>(restInfo);
            StringContent content = new StringContent(restInfoString, UnicodeEncoding.UTF8, "application/json");

            using var response = httpClient.PutAsync(serviceUrl, content).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(RestaurantInfo restInfo)
        {
            HttpClient httpClient = new HttpClient();

            string restInfoString = JsonSerializer.Serialize<RestaurantInfo>(restInfo);
            StringContent content = new StringContent(restInfoString, UnicodeEncoding.UTF8, "application/json");

            using var response = httpClient.PostAsync(serviceUrl, content).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpClient httpClient = new HttpClient();

            using var response = httpClient.DeleteAsync(serviceUrl + '/' + id.Value).Result;

            if(response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }

}
