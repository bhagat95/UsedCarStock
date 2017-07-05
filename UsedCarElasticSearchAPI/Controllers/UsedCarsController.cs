using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using UsedCarElasticSearchAPI.Models;
using ElasticSearchDAL;
using UsedCarEntities;
using PagedList;
using PagedList.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Dapper;
using UsedCarDAL;
using UsedCarBL;
using System.Drawing.Imaging;
namespace UsedCarElasticSearchAPI.Controllers
{
    public class UsedCarsController : Controller
    {
        private ElasticSearchRepository _elasticRepository;
        private UsedCarRepository _usedCarRepositoty;
        public UsedCarsController()
        {
            _elasticRepository = new  ElasticSearchRepository();
            _usedCarRepositoty = new UsedCarRepository();
        }
        
         //GET: UsedCars
        public ActionResult All()
        {
            try
            {
                IEnumerable<UsedCarModel> cars = _elasticRepository.Search(0, "all", 0, 0);
                return View("~/Views/UsedCars/SearchResult.cshtml", cars);
            }
            catch (Exception)
            {
                return View("~/Views/Shared/Error.cshtml");
                throw;
            }
        }
       
        public ActionResult Filter()
        {
            Int32 Page = new Int32();
            Page = Convert.ToInt32(Request.QueryString["page"]);
            string City = Request.QueryString["city"];
            int MinBudget = Convert.ToInt32(Request.QueryString["minbudget"]);
            int MaxBudget = Convert.ToInt32(Request.QueryString["maxbudget"]);
            try
            {
                IEnumerable<UsedCarModel> cars = _elasticRepository.Search(Page, City, MinBudget, MaxBudget);
                return View("~/Views/Shared/_bindCarResult.cshtml", cars);
            }
            catch (Exception)
            {
                return View("~/Views/Shared/_NotFoundError.cshtml");
            }
            
        }
        
        public ActionResult CarProfile(int id)
        {
            try
            {
                UsedCarModel usedCarModel = new UsedCarModel();
                usedCarModel = _usedCarRepositoty.GetSingleCarMemCache(id);
                return View("~/Views/UsedCars/ProfileView.cshtml", usedCarModel);
            }
            catch (Exception)
            {
                return View("~/Views/Shared/_NotFoundError.cshtml");
            }
        }
        public ActionResult CarCities()
        {
            try
            {
                IEnumerable<UsedCarCitiesModel> cities = _usedCarRepositoty.GetCitiesMemCache();
                return View("~/Views/Shared/_bindCities.cshtml", cities);
            }
            catch (Exception)
            {
                return View("~/Views/Shared/_NotFoundError.cshtml");
            }
        }
    }
}