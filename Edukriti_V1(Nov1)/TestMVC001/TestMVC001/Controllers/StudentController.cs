using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestMVC001.Models;

namespace TestMVC001.Controllers
{
    public class StudentController : Controller
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
            var student = new Student
            {
                StudentId =143,
                StudentFirstName = "Shailesh",
                StudentMiddleName = "Chandra",
                StudentLastName="Yerram",
                Class="X",
                Section = "A",
                Gender = "M",
                DateOfBirth = DateTime.Today,
                Rfid = "0000012345",
                ParentFirstName = "Santhosh",
                ParentMiddleName = "Kumar",
                ParentLastName = "Ragam",
                ParentPrimaryPhoneNumber = "9030644017",
                ParentHomePhoneNumber = "9030644017",
                ParentEmailId = "shaileshyerram@gmail.com",
                Orgid = "12345"

            };
            return View(student);
        }


        //bigint StudentId
        //varchar StudentFirstName
        //varchar StudentMiddleName
        //varchar StudentLastName
        //varchar Class
        //varchar Section
        //varchar Gender
        //datetime DateOfBirth
        //varchar Rfid
        //varchar ParentFirstName
        //varchar ParentMiddleName
        //varchar ParentLastName
        //float ParentPrimaryPhoneNumber
        //float ParentHomePhoneNumber
        //varchar ParentEmailId
        //varchar Orgid


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
                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                {
                    return View(student);
                }

                //return RedirectToAction("Index");
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
