using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Lab26.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetWeatherData(double lat, double lon)
        {
            //create a request to the website for the data at that URL

            //string url = ("https://forecast.weather.gov/MapClick.php?lat=42.335722&lon=-83.049944&FcstType=json");

          

            string url = ("https://forecast.weather.gov/MapClick.php?lat=42.335722&lon=-83.049944&FcstType=json");

            string latitude = lat.ToString();
            string longitude = lon.ToString();

            //if (lat != 42.335722 && lon != -83.049944)
            //{
            //    //input does not work
            //    //url = ("https://forecast.weather.gov/MapClick.php?lat=" + latitude + "&lon=" + longitude + "&FcstType=json");
            //}
            HttpWebRequest request = WebRequest.CreateHttp("https://forecast.weather.gov/MapClick.php?lat=" + (lat.ToString()) + "&lon=" + (lon.ToString()) + "&FcstType=json");

           // HttpWebRequest request = WebRequest.CreateHttp(url);
            
            
            //Tell it the list of browsers we're using
            request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

            //If authorization or keys are needed, those steps go HERE.
            //Gets the response to the request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //set up stream reader to interpret the response as a useable string
            StreamReader reader = new StreamReader(response.GetResponseStream());

            //return reader info as string
            String data = reader.ReadToEnd();


            //Create JSON object or follow steps to use XML instead here.
            JObject jo = JObject.Parse(data);

            //go through JSON data. Each tag contains a string array or a list, its helpful to have the 
            //JSON viewer open to track it. 
            //Can use .ToList() here to make a list of JTokens

            //JSON is a series of nested tags 

            //create a list, using the jo["key"]["key"].toList()
            List<JToken> times = jo["time"]["startPeriodName"].ToList();

            //list of string temperatures
            List<string> temps = new List<string>();

            //create new lines here 
            JToken region = jo["location"]["region"];

            //add to a viewbag to pass to View
            ViewBag.region = region;

            JToken LocationInfo = jo["location"]["areaDescription"];
            ViewBag.location = LocationInfo;
            //
            for (int i = 0; i < jo["data"]["temperature"].Count(); i++)
            {
                string timeLabel = times[i].ToString();
                string input = jo["time"]["startPeriodName"][i] + " " + jo["time"]["tempLabel"][i] + " " + jo["data"]["temperature"][i].ToString();

                temps.Add(input);


            }

            //viewbag holds the list called 'temps'
            ViewBag.AllTemps = temps;

            return View("Data");
        }

        public ActionResult Data()
        {   
            
            return GetWeatherData(42.335722, -83.049944);
        }

        public ActionResult Input()
        {
            return View();
        }

        public ActionResult UserInput(double latView, double lonView)
        {
            //try storing in a session object
            double lat = latView;
            double lon = lonView;
            ViewBag.test= lat + " / " + lon;
            //return View("Contact");
            return GetWeatherData(lat, lon);
        }

    }
}