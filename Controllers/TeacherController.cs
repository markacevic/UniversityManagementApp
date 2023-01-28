using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniversityManagementApp.Data;
using UniversityManagementApp.Models;
using UniversityManagementApp.ViewModels;

namespace UniversityManagementApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly UniversityManagementAppContext _context;

        public TeacherController(UniversityManagementAppContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Teacher teacher = _context.Teacher.Where(t => t.Id == id).FirstOrDefault();
            if (teacher == null)
            {
                return NotFound();
            }

            IQueryable<Course> courses = _context.Course.Where( c => c.FirstTeacherId == id || c.SecondTeacherId == id);
            var Courses = courses.AsEnumerable();
            ViewData["FullName"] = _context.Teacher.Where( t => t.Id== id).Select( t => t.FullName).FirstOrDefault();
            return View(Courses);
        }

        public async Task<IActionResult> Students(int id, int? searchYear) // id e CourseId
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Course.FindAsync(id);
            //var course = _context.Course.Where(c => c.Id == id).First();

            if (course == null)
            {
                return NotFound();
            }
            
            var lastYear = searchYear;

            if (searchYear == null)
            { lastYear = DateAndTime.Today.Year; }
            
           
            //vishok start
            IEnumerable<int> students_ids = _context.Enrollment.Where(e => e.CourseId == course.Id && e.year == lastYear).Select(e => e.StudentId);
            // site ids na studenti zapisani na toj kurs vo posl godina ili vo baranata godina
            IQueryable<Student> students = _context.Student.Include(s => s.Courses).Where(s => students_ids.Contains(s.Id)).AsQueryable();
            IEnumerable<int?> years = _context.Enrollment.Select( e => e.year).Distinct().AsEnumerable();
            //end
            IQueryable<Enrollment> enrollments = _context.Enrollment.Where(e => e.CourseId == course.Id && e.year == lastYear).Include(e => e.Student).AsQueryable();

            StudentsByYearViewModel viewmodel = new StudentsByYearViewModel
            {
                Years = new SelectList(years),
                Students = students.AsEnumerable(), //vishok
                Enrollments= enrollments.AsEnumerable(),

            };
            return View(viewmodel);

        }

        //GET: Teacher/Edit/1
        public async Task<IActionResult> Edit(int? id)  // id e Enrollment id
        {
            if (id == null)
            {
                return NotFound();
            }

            //var enrollment = await _context.Enrollment.FindAsync(id);
            Enrollment enrollment =  _context.Enrollment.Where(e => e.Id == id).Include(e => e.Course).Include(e => e.Student).First();

            if (enrollment.FinishDate.HasValue)
            {
                return NotFound();
            }

            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["Course"] = enrollment.CourseId; // kako da se prati Course.Title
            ViewData["Fullname"] = enrollment.StudentId; // kako da se prati Student.Fullname?

            return View(enrollment);
        }



        // POST: Teacher/Edit/1    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Enrollment enrollment)
        {
            if (id != enrollment.Id) 
            {
                return NotFound();
            }
            Enrollment old_enrollment = _context.Enrollment.Where(e => e.Id == id).First();
            if (ModelState.IsValid)
            { 
                old_enrollment.AdditionalPoints = enrollment.AdditionalPoints;
                old_enrollment.SeminalPoints = enrollment.SeminalPoints;
                old_enrollment.ExamPoints = enrollment.ExamPoints;
                old_enrollment.ProjectPoints = enrollment.ProjectPoints;
                old_enrollment.grade =  enrollment.grade;   

                if (!enrollment.FinishDate.HasValue) // not necessary
                {
                    old_enrollment.FinishDate = enrollment.FinishDate;
                }

                _context.Update(old_enrollment);  
                await _context.SaveChangesAsync();

                var myId = enrollment.CourseId;
                return RedirectToAction("Students", new { id = myId}); //not working
            }
            ViewData["Course"] = enrollment.CourseId; // kako da se prati Course.Title
            ViewData["Fullname"] = enrollment.StudentId; // kako da se prati Student.Fullname?


            return View(enrollment);
        }






    }
}
