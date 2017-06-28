using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsedCarEntities
{

    public class UsedCarModel
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int Year { get; set; }
        public int Kilometer { get; set; }
        public String FuelTypeId { get; set; }
        public String CityId { get; set; }
        public String ColorId { get; set; }
        public int FuelEconomy { get; set; }
        public String MakeId { get; set; }
        public String ModelId { get; set; }
        public String VersionId { get; set; }
        public String ImgUri { get; set; }
        public bool IsAvailable { get; set; }
    }
}
