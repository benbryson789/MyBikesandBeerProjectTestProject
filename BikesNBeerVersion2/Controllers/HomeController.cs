using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BikesNBeerVersion2.Models;
using Newtonsoft.Json;
using BikesNBeerVersion2.Services;
using System.Net.Http;

namespace BikesNBeerVersion2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {

            //var client = new HttpClient();
            //var httpResponse = await client.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?address=67337&key=AIzaSyDDQ1uMLrSYDQtlX-VIFyyiXMB5_dRJNqU");
            //// https://maps.googleapis.com/maps/api/geocode/json?address=zipcode&key=YOUR_API_KEY
            //// replace zipcode with original code
            //// replace YOUR_API_KEY with the api key from google
            //httpResponse.EnsureSuccessStatusCode();
            //var content = await httpResponse.Content.ReadAsStringAsync();
            //var result = JsonConvert.DeserializeObject<Rootobject>(content);


            //InvalidOperationException: The model item passed into the ViewDataDictionary is of type 'BikesNBeerVersion2.Services.Rootobject', but this ViewDataDictionary instance requires a model item of type 'BikesNBeerVersion2.Services.Result'.

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string zipcode)
        {

            var client = new HttpClient();
            var httpResponse = await client.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={zipcode}&key=AIzaSyDDQ1uMLrSYDQtlX-VIFyyiXMB5_dRJNqU");
            // https://maps.googleapis.com/maps/api/geocode/json?address=zipcode&key=YOUR_API_KEY
            // replace zipcode with original code
            // replace YOUR_API_KEY with the api key from google
            httpResponse.EnsureSuccessStatusCode();
            var content = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Rootobject>(content);

            //ViewBag["result"] = result;
            
            return View(result);
        }



        public IActionResult Hotel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Hotel(string zipcode)
        {

            var client = new HttpClient();
            var httpResponseLatLong = await client.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={zipcode}&key=AIzaSyDDQ1uMLrSYDQtlX-VIFyyiXMB5_dRJNqU");
            httpResponseLatLong.EnsureSuccessStatusCode();
            var contentLatLong = await httpResponseLatLong.Content.ReadAsStringAsync();
            var resultLatLong = JsonConvert.DeserializeObject<Rootobject>(contentLatLong);


            HotelResponse result = new HotelResponse();
            if (resultLatLong!=null && resultLatLong.results.Length>0 && resultLatLong.results[0].geometry!=null)
            {
                //string url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={resultLatLong.results[0].geometry.location.lat}.1298305&radius=1500&type=lodging&keyword=hotels&key=%20AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA";
                var httpResponse = await client.GetAsync($"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={resultLatLong.results[0].geometry.location.lat},{resultLatLong.results[0].geometry.location.lng}&radius=5000&keyword=hotel&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA");
                httpResponse.EnsureSuccessStatusCode();
                var content = await httpResponse.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<HotelResponse>(content);
            }

            

            //ViewBag["result"] = result;

            return View(result);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
