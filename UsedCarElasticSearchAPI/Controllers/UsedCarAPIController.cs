using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
    public class UsedCarAPIController : ApiController
    {
        private UsedCarRepository _repository;
        string uri = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl"].ToString();
        public UsedCarAPIController()
        {
            _repository = new UsedCarRepository();
        }
        public IHttpActionResult Get(int id)
        {
            try
            {
                return Ok(_repository.GetSingleCarMemCache(id));
            }
            catch (Exception)
            {
                return InternalServerError();
            } 
        }
        public IHttpActionResult Post(UsedCarModel usedCarModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int id = _repository.AddCar(usedCarModel);
                    return Ok(uri + id);
                }
                else
                {
                    return BadRequest(ModelState);
                }
                
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
        public IHttpActionResult Put(int id, UsedCarModel usedCarModel)
        {
            try
            {
                _repository.UpdateCarDetails(id, usedCarModel);
                return Ok("Successfully Updated");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            
        }
        public IHttpActionResult Delete(int id)
        {
            try
            {
                _repository.DeleteCar(id);
                return Ok("successfully deleted");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}