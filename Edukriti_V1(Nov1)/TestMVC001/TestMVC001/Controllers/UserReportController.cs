using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestMVC001.Controllers
{
    public class UserReportController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUser()
        {
            return View();
        }

        public JsonResult GetAllUser()
        {
            //List<UserMaster> allUser = new List<UserMaster>();


            //// Here "MyDatabaseEntities " is dbContext, which is created at time of model creation.

            //using (MyDatabaseEntities dc = new MyDatabaseEntities())
            //{
            //    allUser = dc.UserMasters.ToList();
            //}

            return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //public JsonResult GetUserWithParameter(string prefix)
        //{
        //    List<UserMaster> allUser = new List<UserMaster>();


        //    // Here "MyDatabaseEntities " is dbContext, which is created at time of model creation.

        //    using (MyDatabaseEntities dc = new MyDatabaseEntities())
        //    {
        //        allUser = dc.UserMasters.Where(a => a.Username.Contains(prefix)).ToList();
        //    }

        //    return new JsonResult { Data = allUser, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

    }
}
