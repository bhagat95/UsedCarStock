using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsedCarProfilePage.Models;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Dapper;

namespace UsedCarProfilePage.Controllers
{
   
    public class UsedCarController : Controller
    {
        
        public string strConnString = ConfigurationManager.ConnectionStrings["mysqlconnstr"].ConnectionString;
        
        public ActionResult Details(int id)
        {
            IEnumerable<UsedCarModel> data;
             
            IDbConnection conn = new MySqlConnection(strConnString);
            var param = new DynamicParameters();
            param.Add("id", id);
            UsedCarModel res = new UsedCarModel();
            try
            {
                using (var con = conn)
                {
                    data = con.Query<UsedCarModel>("GetSingleStock_AS", param, commandType: CommandType.StoredProcedure);
                }

                 res = data.ElementAt(0);

            }
            catch (Exception)
            {
                res = null;
                
            }
            



            return View("~/Views/UsedCar/ProfileView.cshtml", res);
        }
        //public string GetString()
        //{
        //    return "its working";
        //}
        //public ActionResult GetView()
        //{
        //    return View("ProfileView");
        //}

        //protected DataSet func(string str)
        //{
        //    MySqlConnection con2 = new MySqlConnection(strConnString);
        //    MySqlCommand cmd2 = new MySqlCommand(str, con2);

        //    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
        //    DataSet dset2 = new DataSet();
        //    using (MySqlDataAdapter adpt2 = new MySqlDataAdapter(cmd2))
        //    {

        //        adpt2.Fill(dset2);
        //        con2.Close();

        //    }
        //    return dset2;
        //}

    }
}