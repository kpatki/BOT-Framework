using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WeatherBOT.WeatherClient
{
    public class Helper
    {
        public static RootObject Fetch(string cityName, string CountryName)
        {
            RootObject model = null;
            try
            {
                var apiKey = System.Configuration.ConfigurationManager.AppSettings["API-KEY"];

                var weather = Client.GET(string.Format("http://api.openweathermap.org/data/2.5/weather?q={0},{1}&appid={2}",cityName,CountryName, apiKey));

                var serializer = new JavaScriptSerializer();
                model = serializer.Deserialize<RootObject>(weather);

            }
            catch(Exception ex)
            {

            }

            return model;
        }
    }
}