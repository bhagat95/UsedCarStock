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
using System.Drawing;
using System.Web;

namespace UsedCarStockAPI.Controllers
{
    //[Authorize]
    public class UsedCarController : ApiController
    {
        UsedCarRepository _repo;
  
        UsedCarModel usedCarModel = null;

        // GET api/values
        public UsedCarController()
        {
            _repo = new UsedCarRepository();
        }
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public UsedCarModel Get(int id)
        {

            //UsedCarRepository usedCarRepositoty = new UsedCarRepository();
            return _repo.GetSingleCarMemCache(id);
        }

        // POST api/values
        public String Post(UsedCarModel usedCarModel)
        {
            UsedCarRepository usedCarRepository = new UsedCarRepository();
            int id = usedCarRepository.AddCar(usedCarModel);
            String uri = "http://localhost:59011/api/usedCar/" + id; //web config
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