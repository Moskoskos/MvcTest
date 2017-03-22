﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WatermentWebSCADA.Models;
using WatermentWebSCADA.ViewModels;

namespace WatermentWebSCADA.ViewModels
{
    public class MainViewModel
    {
       //public FacilityViewModel facilitiess { get; set; }


        //facilities
        public List<string> Name { get; set; }
        public string IP { get; set; }
        public string Domain { get; set; }

        //locations model
        public string Address { get; set; }
        public int Postcode { get; set; }
        public string County { get; set; }

        //Countrie model

        public List<string> CountryName { get; set; }

   
        //locations model
 
        public List<string> FirstName { get; set; }
        public List<string> LastName { get; set; }
        public List<int?> Phone { get; set; }
        public List<string> Email { get; set; }
        public List<float?> ProcessValue { get; set; }
        public List<string> Tag { get; set; }
        public List<System.DateTime> Timestamp { get; set; }
        public List<String> Description { get; set; }
        public List<System.DateTime> Alarmsoccured { get; set; }
    }
}