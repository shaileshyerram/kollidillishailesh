using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using PagedList;
using System.Web.Mvc;

namespace TestMVC001.Models
{
    public class StudentsController : Controller
    {
        private DB_95608_edukritiEntities db = new DB_95608_edukritiEntities();

        ////GET: Students
        //public ActionResult Index()
        //{
        //    return View(db.Students.ToList());
        //}

        // GET: Students/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,StudentFirstName,StudentMiddleName,StudentLastName,Class,Section,Gender,DateOfBirth,RFID,ParentFirstName,ParentMiddleName,ParentLastName,ParentPrimaryPhoneNumber,ParentHomePhoneNumber,ParentEmailId,ORGID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,StudentFirstName,StudentMiddleName,StudentLastName,Class,Section,Gender,DateOfBirth,RFID,ParentFirstName,ParentMiddleName,ParentLastName,ParentPrimaryPhoneNumber,ParentHomePhoneNumber,ParentEmailId,ORGID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ViewResult Index(string sortOrder, string currentSort, string currentFilter, string searchString, int? page)
        {
           // string currentSortOrder = currentSort;
            ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            //return View(db.Students.ToList());
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var students = from s in db.Students
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.StudentLastName.Contains(searchString) || s.StudentFirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "FirstName":
                    if (sortOrder.Equals(currentSort))
                        students = students.OrderByDescending(s => s.StudentFirstName);
                    else
                        students = students.OrderBy(s => s.StudentFirstName);
                    break;
                case "LastName":
                    if (sortOrder.Equals(currentSort))
                        students = students.OrderByDescending(s => s.StudentLastName);
                    else
                        students = students.OrderBy(s => s.StudentLastName);
                    break;
                case "RFID":
                    if (sortOrder.Equals(currentSort))
                        students = students.OrderByDescending(s => s.RFID);
                    else
                        students = students.OrderBy(s => s.RFID);
                    break;
                default:  // Name ascending 
                    students = students.OrderBy(s => s.StudentFirstName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }


    }
}
