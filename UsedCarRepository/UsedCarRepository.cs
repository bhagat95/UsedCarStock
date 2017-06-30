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


namespace UsedCarDAL
{
    public class UsedCarRepository
    {

        static IDbConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"]
    .ConnectionString);

        /// <summary>
        /// TODO: manage exceptions in each method
        /// </summary>
        /// <param name="usedCarModel"></param>
        /// <returns></returns>

        public int DeleteCar(int Id)
        {

            var Param = new DynamicParameters();
            Param.Add("Id", Id);
            try
            {
                using (var con = connection)
                {
                    int Result = con.Execute("DeleteStock_AS", Param,
                        commandType: CommandType.StoredProcedure);
                    MemCacheManager memCacheManager = new MemCacheManager();
                    bool isDeleted = memCacheManager.DeleteFromCache(Convert.ToString(Id));

                    //Delete from Elastic Search
                    new ElasticSearchRepository().Delete(Id);

                    return Result;
                }
            }
            catch (Exception)
            {

                //throw;
            }
            return -1;
        }

        public IEnumerable<UsedCarModel> GetAllCars(int PageId, int Offset)
        {
            var Param = new DynamicParameters();
            Param.Add("PageId", PageId);
            Param.Add("Offset", Offset);
            using (var con = connection)
            {
                IEnumerable<UsedCarModel> Result = con.Query<UsedCarModel>("GetAllStock_AS", Param,
                    commandType: CommandType.StoredProcedure);
                return Result;
            }

        }

        public UsedCarModel GetSingleCar(int id)
        {
            var Param = new DynamicParameters();
            Param.Add("Id", id);
            using (var con = connection)
            {
                IEnumerable<UsedCarModel> Result = con.Query<UsedCarModel>("GetSingleStock_AS", Param,
                       commandType: CommandType.StoredProcedure);
                if (((System.Collections.Generic.List<UsedCarEntities.UsedCarModel>)Result).Count == 0)
                {
                    return null;
                }
                return Result.ElementAt(0);
            }
        }

        public UsedCarModel GetSingleCarMemCache(int id)
        {
            try
            {
                MemCacheManager mc = new MemCacheManager();
                return mc.GetFromCache<UsedCarModel>(Convert.ToString(id), new TimeSpan(0, 30, 0), () => GetSingleCar(id));
            }
            catch (Exception err)
            {

                throw;
            }
            //return this;
        }


        public int AddCar(UsedCarModel usedCarModel)
        {

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
            usedCarModel.ImgUri = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagePath"].ToString()+"l_car_";
            param.Add("ImgUri", usedCarModel.ImgUri);
            //param.Add("IsAvailable", usedCarModel.IsAvailable);

            IEnumerable<int> id;
            using (var con = connection)
            {
                //int rowsInserted = con.Execute("AddStock_AS", param, commandType:CommandType.StoredProcedure);

                id = con.Query<int>("AddStock_AS", param, commandType: CommandType.StoredProcedure);

            }

            AddIdRabbitMQ(id.ElementAt(0));
            //new ElasticSearchRepository().GlobalRabbitMQSubscriber();

            return id.ElementAt(0);
        }

        public bool UpdateCarDetails(int id, UsedCarModel usedCarModel)
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
            //param.Add("IsAvailable", usedCarModel.IsAvailable);

            IEnumerable<int> response;
            using (var con = connection)
            {
                //int rowsInserted = con.Execute("AddStock_AS", param, commandType:CommandType.StoredProcedure);

                response = con.Query<int>("EditStock_AS", param, commandType: CommandType.StoredProcedure);
                MemCacheManager memCacheManager = new MemCacheManager();
                bool isDeleted = memCacheManager.DeleteFromCache(Convert.ToString(id));
                

            }

            AddIdRabbitMQ(id);

            return true;
        }

/// <summary>
/// Make SIngleton classes and single instances of Connection Factory
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
        public String AddIdRabbitMQ(int id)
        {
            try
            {
                var connectionFactory = new ConnectionFactory() { HostName = "172.16.0.11" };
                IConnection connection = connectionFactory.CreateConnection();
                IModel channel = connection.CreateModel();

                String Queue = "UsedCarElasticSearchQueue";

                channel.QueueDeclare(Queue, false, false, false, null);

                byte[] message = Encoding.UTF8.GetBytes(id + "");

                channel.BasicPublish("", Queue, null, message);
                //Console.WriteLine("Press any key to exit");
                //Console.ReadKey();
                channel.Close();
                connection.Close();
                return "success";
            }

            catch (Exception e)
            {
                Console.WriteLine(e + "");
            }
            return "exception";
        }

        public object id { get; set; }
    }
}
