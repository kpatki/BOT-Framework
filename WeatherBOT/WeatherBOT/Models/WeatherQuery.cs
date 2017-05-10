﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherBOT.Models
{
    [Serializable]
    public class WeatherQuery
    {
        public string City { get; set; }
        public string Country { get; set; }
    }
}