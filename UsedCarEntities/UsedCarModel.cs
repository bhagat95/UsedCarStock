using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsedCarEntities
{
    [Serializable]
    public class UsedCarModel
    {
        private int _Id;

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private int _Price;

        public int Price
        {
            get { return _Price; }
            set { _Price = value; }
        }

        private int _Year;

        public int Year
        {
            get { return _Year; }
            set { _Year = value; }
        }
        private int _Kilometer;

        public int Kilometer
        {
            get { return _Kilometer; }
            set { _Kilometer = value; }
        }

        private string _FuelTypeId;

        public string FuelTypeId
        {
            get { return _FuelTypeId; }
            set { _FuelTypeId = value; }
        }

        private string _CityId;

        public string CityId
        {
            get { return _CityId; }
            set { _CityId = value; }
        }

        private string _ColorId;

        public string ColorId
        {
            get { return _ColorId; }
            set { _ColorId = value; }
        }
        private int _FuelEconomy;

        public int FuelEconomy
        {
            get { return _FuelEconomy; }
            set { _FuelEconomy = value; }
        }

        private int _MakeId;

        public int MakeId
        {
            get { return _MakeId; }
            set { _MakeId = value; }
        }
        private int _ModelId;

        public int ModelId
        {
            get { return _ModelId; }
            set { _ModelId = value; }
        }
        private int _VersionId;

        public int VersionId
        {
            get { return _VersionId; }
            set { _VersionId = value; }
        }
        private string _ImgUri;

        public string ImgUri
        {
            get { return _ImgUri; }
            set { _ImgUri = value; }
        }

        private bool _IsAvailable;

        public bool IsAvailable
        {
            get { return _IsAvailable; }
            set { _IsAvailable = value; }
        }

        private DateTime _DateCreated;

        public DateTime DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }
        private string _FuelType;

        public string FuelType
        {
            get { return _FuelType; }
            set { _FuelType = value; }
        }

        private string _City;

        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        private string _Color;

        public string Color
        {
            get { return _Color; }
            set { _Color = value; }
        }

        private string _Make;

        public string Make
        {
            get { return _Make; }
            set { _Make = value; }
        }

        private string _Model;

        public string Model
        {
            get { return _Model; }
            set { _Model = value; }
        }
        private string _Version;

        public string Version
        {
            get { return _Version; }
            set { _Version = value; }
        }
        
        
        
        
        
        
        
        
        //public int Id { get; set; }
        //public int Price { get; set; }
        //public int Year { get; set; }
        //public int Kilometer { get; set; }
        //public string FuelTypeId { get; set; }
        //public string CityId { get; set; }
        //public string ColorId { get; set; }
        //public int FuelEconomy { get; set; }
        //public string MakeId { get; set; }
        //public string ModelId { get; set; }
        //public string VersionId { get; set; }
        //public string ImgUri { get; set; }
        //public bool IsAvailable { get; set; }
    }
}
