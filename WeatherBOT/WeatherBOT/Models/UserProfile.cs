using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherBOT.Models
{
    [Serializable]
    public class UserProfile
    {
        public string Name { get; set; }
        public string EmailId { get; set; }
        public WeatherQuery WeatherQuery { get; set; }
    }
}