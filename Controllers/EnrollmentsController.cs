using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    public class EnrollmentsController : Controller
    {
        private SchoolManagementSystemEntities db = new SchoolManagementSystemEntities();

        // GET: Enrollments
        public ActionResult Index()
        {
            var enrollment = db.Enrollment.Include(e=>e.Course).Include(e => e.Participant).Include(e=>e.Lecturer);
            return View(enrollment.ToList());
        }

        // GET: Enrollments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollment.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create()
        {
            ViewBag.ParticipantID = new SelectList(db.Participant, "ParticipantID");
            ViewBag.CourseID = new SelectList(db.Course, "CourseID");
            ViewBag.LecturerID = new SelectList(db.Lecturer, "Id", "First_Name");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentId,Grade,CourseId,ParticipantId")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollment.Add(enrollment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParticipantId = new SelectList(db.Participant, "ParticipantId", null, enrollment.ParticipantId);
            ViewBag.CourseId = new SelectList(db.Course, "CourseID", null, enrollment.CourseId);
            //ViewBag.LecturerId = new SelectList(db.Lecturer, "Id", "First_Name", enrollment.LecturerId);
            return View(enrollment);
        }

        [HttpPost]
        public async Task<JsonResult> AddStudent ([Bind(Include = "CourseId,ParticipantId")] Enrollment enrollment)
        {
            try
            {
                var isEnrolled = db.Enrollment.Any(p => p.CourseId == enrollment.CourseId && p.ParticipantId == enrollment.ParticipantId);
                if (ModelState.IsValid && !isEnrolled)
                {
                    db.Enrollment.Add(enrollment);
                    await db.SaveChangesAsync();
                    return Json(new {IsSuccess=true, Message="Student Added Successfully"}, JsonRequestBehavior.AllowGet);
                }
                return Json(new { IsSuccess = false, Message = "Student couldn't be added, maybe it's already enrolled?" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Please Contact Your Administrator" }, JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Enrollments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollment.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParticipantId = new SelectList(db.Participant, "ParticipantId", null, enrollment.ParticipantId);
            ViewBag.CourseId = new SelectList(db.Course, "CourseID",null , enrollment.CourseId);
            ViewBag.LecturerId = new SelectList(db.Lecturer, "Id", "First_Name", enrollment.LecturerId);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentId,Grade,CourseId,ParticipantId")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParticipantId = new SelectList(db.Participant, "ParticipantId", "LastName", enrollment.ParticipantId);
            ViewBag.CourseId = new SelectList(db.Course, "CourseID", "LastName", enrollment.CourseId);
            ViewBag.LecturerId = new SelectList(db.Lecturer, "Id", "First_Name", enrollment.LecturerId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollment.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollment.Find(id);
            db.Enrollment.Remove(enrollment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetParticipants(string term)
        {
            var students = db.Participant.Select(p => new
            {
                Name = p.FirstName + " " + p.LastName,
                Id = p.ParticipantId
            }).Where(p=>p.Name.Contains(term));

            return Json(students, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetCourses(string term)
        //{
        //    var courses = db.Course.Select(c => new
        //    {
        //        Name = c.Title 
        //    }).Where(p => p.Name.Contains(term));

        //    return Json(courses, JsonRequestBehavior.AllowGet);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
