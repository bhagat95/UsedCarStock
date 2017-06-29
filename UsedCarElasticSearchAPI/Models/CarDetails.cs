using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsedCarElasticSearchAPI.Models
{
    public class CarDetails
    {
        public int Id { get; set; }
        public String Make { get; set; }
        public String Model { get; set; }
        public String Version { get; set; }
        public int Price { get; set; }
        public String City { get; set; }
        
        public int Kilometer { get; set; }

        public String FuelType { get; set; }
        public int Year { get; set; }
        //public String Color { get; set; }
        //public int FuelEconomy { get; set; }
        
        public String ImageUri { get; set; }
        //public bool IsAvailable { get; set; }
    }
}