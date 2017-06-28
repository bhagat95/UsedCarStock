using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsedCarEntities;
using MySql.Data.MySqlClient;
using System.Configuration;
using Dapper;

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
            using (var con = connection)
            {
                IEnumerable<int> Result = con.Query<int>("DeleteStock_AS", Param,
                    commandType: CommandType.StoredProcedure);
                return Result.ElementAt(0);
            }
            
        }

        public IEnumerable<UsedCarModel> GetAllCars(int PageId, int Offset)
        {
            var Param = new DynamicParameters();
            Param.Add("PageId", PageId);
            Param.Add("Offset", Offset);
            using(var con = connection)
            {
                IEnumerable<UsedCarModel> Result = con.Query<UsedCarModel> ("GetAllStock_AS", Param,
                    commandType: CommandType.StoredProcedure);
                return Result;
            }
            
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
            param.Add("MakeId",usedCarModel.MakeId);
            param.Add("ModelId",usedCarModel.ModelId);
            param.Add("VersionId", usedCarModel.VersionId);
            param.Add("ImgUri", usedCarModel.ImgUri);
            //param.Add("IsAvailable", usedCarModel.IsAvailable);

            IEnumerable<int> id ;
            using (var con = connection)
            {
                //int rowsInserted = con.Execute("AddStock_AS", param, commandType:CommandType.StoredProcedure);

                id = con.Query<int>("AddStock_AS", param, commandType: CommandType.StoredProcedure);
                
            }
            return id.ElementAt(0);
        }


        public int UpdateCarDetails(int id, UsedCarModel usedCarModel)
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

            }
            return response.ElementAt(0);
        }


        public object id { get; set; }
    }
}
