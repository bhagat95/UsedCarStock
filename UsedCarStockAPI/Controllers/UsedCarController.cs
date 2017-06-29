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
using UsedCarEntities;
using UsedCarDAL;

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
        UsedCarModel usedCarModel;

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public UsedCarEntities.UsedCarModel Get(int id)
        {

            Console.WriteLine(id + " GET " + usedCarModel);
            UsedCarRepository usedCarRepositoty = new UsedCarRepository();
            return usedCarRepositoty.GetSingleCarMemCache(id);
            //return usedCarModel;
        }



        // POST api/values
        public String Post(UsedCarModel usedCarModel)
        {
            UsedCarRepository usedCarRepository = new UsedCarRepository();
            int id = usedCarRepository.AddCar(usedCarModel);
           // Console.WriteLine(usedCarModel + "");
            String uri = "http://localhost:59011/api/usedCar/" + id;
            return uri;
        }

        // PUT api/values/5
        public void Put(int id, UsedCarModel usedCarModel)
        {
            UsedCarRepository usedCarRepository = new UsedCarRepository();
            usedCarRepository.UpdateCarDetails(id, usedCarModel);
        }

        // DELETE api/values/5
        public int Delete(int id)
        {
            UsedCarRepository usedCarRepository = new UsedCarRepository();
            int isDeleted = usedCarRepository.DeleteCar(id);
            return isDeleted;
        }

    }
}