using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using UsedCarEntities;
using MySql.Data.MySqlClient;
using System.Configuration;
using Dapper;
using RabbitMQ.Client;
using UsedCarBL;
using ElasticSearchDAL;
using Newtonsoft.Json;
namespace UsedCarDAL
{
    public class UsedCarRepository
    {
        static IDbConnection Connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DbConnenction"]
    .ConnectionString);
        static string ImagePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagePath"].ToString();
        static string RabbitMQHostName = System.Web.Configuration.WebConfigurationManager.AppSettings["RabbitMQIP"].ToString();
        static string UsedCarElasticSearchQueue = System.Web.Configuration.WebConfigurationManager
            .AppSettings["UsedCarElasticSearchQueue"].ToString();
        static string UsedCarImageUploadQueue = System.Web.Configuration.WebConfigurationManager
            .AppSettings["UsedCarImageUploadQueue"].ToString();
        MemCacheManager memCacheManager = new MemCacheManager();
        
        public bool DeleteCar(int Id)
        {
            try
            {
                using (var con = Connection)
                {
                    var Param = new DynamicParameters();
                    Param.Add("Id", Id);
                    con.Execute("DeleteStock_AS", Param, commandType: CommandType.StoredProcedure);
                    bool isDeleted = memCacheManager.DeleteFromCache(Convert.ToString(Id));
                    new ElasticSearchRepository().Delete(Id);
                    return isDeleted;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<UsedCarModel> GetAllCars()
        {
            using (var con = Connection)
            {
                IEnumerable<UsedCarModel> Result = con.Query<UsedCarModel>("GetAllStock_AS", null,
                    commandType: CommandType.StoredProcedure);
                return Result;
            }
        }
        public IEnumerable<UsedCarCitiesModel> GetAllCities()
        {
            IEnumerable<UsedCarCitiesModel> Result = null;
            try
            {
                using (var con = Connection)
                {
                    Result = con.Query<UsedCarCitiesModel>("GetCities_AS", null,
                        commandType: CommandType.StoredProcedure);
                }
                return Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<UsedCarCitiesModel> GetCitiesMemCache()
        {
            try
            {
                return memCacheManager.GetFromCache<IEnumerable<UsedCarCitiesModel>>("cities",
                    new TimeSpan(0, 30, 0), () => GetAllCities());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public UsedCarModel GetSingleCar(int id)
        {
            try
            {
                using (var con = Connection)
                {
                    var Param = new DynamicParameters();
                    Param.Add("Id", id);
                    var Result = con.Query<UsedCarModel>("GetSingleStock_AS", Param,
                           commandType: CommandType.StoredProcedure).First();
                    return (UsedCarModel)Result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public UsedCarModel GetSingleCarMemCache(int id)
        {
            try
            {
                return memCacheManager.GetFromCache<UsedCarModel>(Convert.ToString(id),
                    new TimeSpan(0, 30, 0), () => GetSingleCar(id));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int AddCar(UsedCarModel usedCarModel)
        {
            try
            {
                using (var con = Connection)
                {
                    ImageData imageData = new ImageData();
                    imageData.Url = usedCarModel.ImgUri;
                    
                    var param = new DynamicParameters();
                    param.Add("Price", usedCarModel.Price);
                    param.Add("Year", usedCarModel.Year);
                    param.Add("Kilometer", usedCarModel.Kilometer);
                    param.Add("FuelTypeId", usedCarModel.FuelTypeId);
                    param.Add("CityId", usedCarModel.CityId);
                    param.Add("ColorId", usedCarModel.ColorId);
                    param.Add("FuelEconomy", usedCarModel.FuelEconomy);
                    param.Add("MakeId", usedCarModel.MakeId);
                    param.Add("ModelId", usedCarModel.ModelId);
                    param.Add("VersionId", usedCarModel.VersionId);
                    usedCarModel.ImgUri = ImagePath;
                    param.Add("ImgUri", usedCarModel.ImgUri);
                    int id = (int)con.Query<int>("AddStock_AS", param, commandType: CommandType.StoredProcedure).First();
                    imageData.Id = id;
                    AddImageDataRabbitMQ(imageData);
                    AddIdRabbitMQ(id);
                    memCacheManager.DeleteFromCache("cities_AS");
                    return id;
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool UpdateCarDetails(int id, UsedCarModel usedCarModel)
        {
            try
            {
                bool isUpdated; 
                using (var con = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("Id", id);
                    param.Add("Price", usedCarModel.Price);
                    param.Add("Year", usedCarModel.Year);
                    param.Add("Kilometer", usedCarModel.Kilometer);
                    param.Add("FuelTypeId", usedCarModel.FuelTypeId);
                    param.Add("CityId", usedCarModel.CityId);
                    param.Add("ColorId", usedCarModel.ColorId);
                    param.Add("FuelEconomy", usedCarModel.FuelEconomy);
                    param.Add("MakeId", usedCarModel.MakeId);
                    param.Add("ModelId", usedCarModel.ModelId);
                    param.Add("VersionId", usedCarModel.VersionId);
                    param.Add("ImgUri", usedCarModel.ImgUri);
                    isUpdated = con.Query<bool>("EditStock_AS", param, commandType: CommandType.StoredProcedure).First();
                    memCacheManager.DeleteFromCache(Convert.ToString(id));
                }
                AddIdRabbitMQ(id);
                return isUpdated;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool AddIdRabbitMQ(int id)
        {
            try
            {
                byte[] message = Encoding.UTF8.GetBytes(id + "");
                var connectionFactory = new ConnectionFactory() { HostName = RabbitMQHostName };
                IConnection connection = connectionFactory.CreateConnection();
                IModel channel = connection.CreateModel();
                channel.QueueDeclare(UsedCarElasticSearchQueue, false, false, false, null);
                channel.BasicPublish("", UsedCarElasticSearchQueue, null, message);
                channel.Close();
                connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool AddImageDataRabbitMQ(ImageData imageData)
        {
            try
            {
                string SerializedObject = JsonConvert.SerializeObject(imageData);
                byte[] message = Encoding.UTF8.GetBytes(SerializedObject);
                var connectionFactory = new ConnectionFactory() { HostName = RabbitMQHostName };
                IConnection connection = connectionFactory.CreateConnection();
                IModel channel = connection.CreateModel();
                channel.QueueDeclare(UsedCarImageUploadQueue, false, false, false, null);
                channel.BasicPublish("", UsedCarImageUploadQueue, null, message);
                channel.Close();
                connection.Close();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}