using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UsedCarStockAPI.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;

namespace UsedCarStockAPI.Controllers
{
    //[Authorize]
    public class UsedCarController : ApiController
    {

        //public string test()
        //{
        //    UsedCarModel usedCarModel = new UsedCarModel();

        //    return usedCarModel.Year;
        //}


        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public UsedCarModel Get(int id)
        {
            Console.WriteLine(id + " GET " + usedCarModel);
            return usedCarModel;
        }

        UsedCarModel usedCarModel;

        // POST api/values
        public UsedCarModel Post(UsedCarModel usedCarModel)
        {
            usedCarModel.Id = 99;

            IDbConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"]
                .ConnectionString);

            
            var param = new DynamicParameters();

            param.Add("price",usedCarModel.Price);
            param.Add("year", usedCarModel.Year);
            param.Add("kilometer", usedCarModel.Kilometer);
            param.Add("fuel_type_id", usedCarModel.FuelType);
            param.Add("city_id", usedCarModel.City);
            param.Add("color_id", usedCarModel.Color);
            param.Add("fuel_economy", usedCarModel.FuelEconomy);
            

            //using (var con = connection)
            //{
            //    int rowsInserted = con.Execute("[AddStock_AS]", param, commandType:CommandType.StoredProcedure);
            //}


            Console.WriteLine(usedCarModel + "");
            return usedCarModel;
        }

        // PUT api/values/5
        public UsedCarModel Put(int id, UsedCarModel usedCarModel)
        {
            Console.WriteLine(id+" "+usedCarModel);
            this.usedCarModel = usedCarModel;
            return usedCarModel;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

    }
}