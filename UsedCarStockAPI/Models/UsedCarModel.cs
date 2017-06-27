using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace UsedCarStockAPI.Models
{
    public class UsedCarModel
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int Year { get; set; }
        public int Kilometer { get; set; }
        public String FuelType { get; set; }
        public String City { get; set; }
        public String Color { get; set; }
        public int FuelEconomy { get; set; }
        public String Make { get; set; }
        public String Model { get; set; }
        public String Version { get; set; }
        public String ImageURI { get; set; }
        public bool IsAvailable { get; set; }
        

    }
}