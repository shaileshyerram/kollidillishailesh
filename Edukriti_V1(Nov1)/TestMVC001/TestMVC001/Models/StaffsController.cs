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
    [Authorize(Roles = "SysAdmin,Admin")]
    public class StaffsController : Controller
    {
        private DB_95608_edukritiEntities db = new DB_95608_edukritiEntities();

        //// GET: Staffs
        //public ActionResult Index()
        //{
        //    return View(db.Staffs.ToList());
        //}

        // GET: Staffs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: Staffs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffId,FirstName,MiddleName,LastName,Gender,DateOfBirth,PhoneNumber,EmailId,RFID,ORGID,Dept")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Staffs.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staff);
        }

        // GET: Staffs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffId,FirstName,MiddleName,LastName,Gender,DateOfBirth,PhoneNumber,EmailId,RFID,ORGID,Dept")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        // GET: Staffs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Staff staff = db.Staffs.Find(id);
            db.Staffs.Remove(staff);
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
            //return View(db.staffs.ToList());
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var staffs = from s in db.Staffs
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                staffs = staffs.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "FirstName":
                    if (sortOrder.Equals(currentSort))
                        staffs = staffs.OrderByDescending(s => s.FirstName);
                    else
                        staffs = staffs.OrderBy(s => s.FirstName);
                    break;
                case "LastName":
                    if (sortOrder.Equals(currentSort))
                        staffs = staffs.OrderByDescending(s => s.LastName);
                    else
                        staffs = staffs.OrderBy(s => s.LastName);
                    break;
                case "RFID":
                    if (sortOrder.Equals(currentSort))
                        staffs = staffs.OrderByDescending(s => s.RFID);
                    else
                        staffs = staffs.OrderBy(s => s.RFID);
                    break;
                default:  // Name ascending 
                    staffs = staffs.OrderBy(s => s.FirstName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(staffs.ToPagedList(pageNumber, pageSize));
        }
    }
}
