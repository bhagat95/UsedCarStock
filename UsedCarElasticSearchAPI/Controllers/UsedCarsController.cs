﻿using System;
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
        
         //GET: UsedCars
        public ActionResult All()
        {


            
            ElasticSearchRepository elasticRepo = new ElasticSearchRepository();

            IEnumerable<UsedCarModel> cars = elasticRepo.Search(0, "all", 0, int.MaxValue);
           
            return View("~/Views/UsedCars/SearchResult.cshtml", cars);
        }
       
        public ActionResult Filter()
        {
            Int32 Page = new Int32();
            Page = Convert.ToInt32(Request.QueryString["page"]);
            string City = Request.QueryString["city"];
            int MinBudget = Convert.ToInt32(Request.QueryString["minbudget"]);
            int MaxBudget = Convert.ToInt32(Request.QueryString["maxbudget"]);
            ElasticSearchRepository elasticRepo = new ElasticSearchRepository();
            IEnumerable<UsedCarModel> cars = elasticRepo.Search(Page, City, MinBudget, MaxBudget);
            
            return View("~/Views/Shared/_bindCarResult.cshtml", cars);
        }

    

        public ActionResult CarProfile(int id)
        {
            try
            {
                UsedCarModel usedCarModel = new UsedCarModel();
                UsedCarRepository usedCarRepositoty = new UsedCarRepository();
                usedCarModel = usedCarRepositoty.GetSingleCarMemCache(id);

                //ImageProcessorRmq imageProcessorES = new ImageProcessorRmq();
                //imageProcessorES.SaveImage("https://imgd.aeplcdn.com/891x501/cw/ucp/stockApiImg/2697XMS_1085006_1_8031299.jpg?q=85",
                //    ImageFormat.Jpeg, id);
                return View("~/Views/UsedCars/ProfileView.cshtml", usedCarModel);
            }
            catch (Exception err)
            {

                throw;
            }


        }
        
        
    }
}