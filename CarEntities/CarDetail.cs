using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarEntities
{
    public class CarDetail
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
        public int Price { get; set; }
        public string City { get; set; }

        public int Kilometer { get; set; }

        public string FuelType { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int FuelEconomy { get; set; }

        public string ImageUri { get; set; }
        public bool IsAvailable { get; set; }
    }
}
