using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TestMVC001.Core.Services;
using TestMVC001.Models;

namespace TestMVC001.Controllers
{
    public class OldStudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        // GET: Student/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //GET: Student/Details/
        public ActionResult Details()
        {
            //TODO get given student details from database.
            //var student = new Student
            //{
            //    StudentId =143,
            //    StudentFirstName = "Shailesh",
            //    StudentMiddleName = "Chandra",
            //    StudentLastName="Yerram",
            //    Class="X",
            //    Section = "A",
            //    Gender = "M",
            //    DateOfBirth = DateTime.Today,
            //    Rfid = "0000012345",
            //    ParentFirstName = "Santhosh",
            //    ParentMiddleName = "Kumar",
            //    ParentLastName = "Ragam",
            //    ParentPrimaryPhoneNumber = "9030644017",
            //    ParentHomePhoneNumber = "9030644017",
            //    ParentEmailId = "shaileshyerram@gmail.com",
            //    Orgid = "12345"

            //};
            //return View(student);
            return null;
        }




        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult  Create(Student student ) //Create(FormCollection collection)
        {
            try
            {
                
                if (!ModelState.IsValid)
                {
                    return View(student);
                }

                // TODO: Add insert logic here


                string connectionString = WebConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
              
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();

                    //string studentInsertQuery= 
                    //          "INSERT INTO [dbo].[tblStudent]  ([StudentFirstName],[StudentMiddleName],[StudentLastName],[Class]," +
                    //          "         [Section],[Gender],[DateOfBirth],[RFID],[ParentFirstName],[ParentMiddleName],[ParentLastName],[ParentPrimaryPhoneNumber]," +
                    //          "             [ParentHomePhoneNumber],[ParentEmailId],[ORGID]) VALUES('"
                    //          + student.StudentFirstName+ "', '"
                    //          + student.StudentMiddleName + "', '" 
                    //          + student.StudentLastName + "', '"
                    //          + student.Class + "', '"
                    //          + student.Section + "', '"
                    //          + student.Gender + "', '"
                    //          + student.DateOfBirth + "', '"
                    //          + student.Rfid + "', '"
                    //          + student.ParentFirstName + "', '"
                    //          + student.ParentMiddleName + "', '"
                    //          + student.ParentLastName + "', '"
                    //          + student.ParentPrimaryPhoneNumber + "', '"
                    //          + student.ParentHomePhoneNumber + "', '"
                    //          + student.ParentEmailId + "', '"
                    //          + student.Orgid + 
                    //          "')"
                    //          ;
                    
                    //SqlCommand cmd = new SqlCommand(studentInsertQuery, con);
                    //cmd.ExecuteNonQuery();
                    con.Close();
                }


                return View();

            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Student/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
