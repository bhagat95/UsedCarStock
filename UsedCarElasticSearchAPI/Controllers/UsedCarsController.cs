using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using UsedCarElasticSearchAPI.Models;
using ElasticSearchDAL;
using CarEntities;

namespace UsedCarElasticSearchAPI.Controllers
{
    public class UsedCarsController : Controller
    {
        
         //GET: UsedCars
        public ActionResult All()
        {
            
             
            ElasticSearchRepository elasticRepo = new ElasticSearchRepository();

            IEnumerable<CarDetail> cars = elasticRepo.Search();
           
            return View("~/Views/UsedCars/SearchResult.cshtml", cars);
        }
       
        public ActionResult Filter()
        {
            string City = Request.QueryString["city"];
            int MinBudget = Convert.ToInt32(Request.QueryString["minbudget"]);
            int MaxBudget = Convert.ToInt32(Request.QueryString["maxbudget"]);
            ElasticSearchRepository elasticRepo = new ElasticSearchRepository();
            IEnumerable<CarDetail> cars = elasticRepo.Search(City, MinBudget, MaxBudget);
            return View("~/Views/Shared/_bindCarResult.cshtml",cars);
        }
        
        
    }
}