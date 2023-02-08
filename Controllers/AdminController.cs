using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UniversityManagementApp.Data;
using UniversityManagementApp.Models;
using UniversityManagementApp.ViewModels;

namespace UniversityManagementApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly UniversityManagementAppContext _context;

        public AdminController(UniversityManagementAppContext context)
        {
            _context = context;
        }

        //GET: Admin/Index1/1
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index1(int? id, int? searchSemester, int? searchYear)
        {
            if (id == null)
            {
                return NotFound();
            }
            IEnumerable<Enrollment> enrollments = _context.Enrollment
                .Where(e => e.CourseId == id).Include(e => e.Course).Include(e => e.Student).AsEnumerable();

            if (searchSemester != null)
            {
                enrollments = enrollments.Where(e => e.Semester == searchSemester);
            }
            if (searchYear != null)
            {
                enrollments = enrollments.Where(e => e.year == searchYear);
            }

            EnrollmentsBySemesterYear viewmodel = new EnrollmentsBySemesterYear
            {
                Enrollments = enrollments,
                IDCourse = id
            };

            return View(viewmodel);
        }







        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Enroll(int? id)  // id e CourseId
        { 
            if (id == null)
            {
                return NotFound();
            }


            // lista od site studenti
            IEnumerable<Student> students = _context.Student.AsEnumerable();
            students = students.OrderBy(s => s.FullName);

            // lista od ids na selektiranite studenti
            var selectedStudents = students.Select(s => s.Id);

            EnrollViewModel viewmodel = new EnrollViewModel
            {

                StudentsList = new MultiSelectList(students, "Id", "FullName"), //lista od site studenti
                SelectedStudents = selectedStudents
             
            };

            return View(viewmodel);
        }




        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Enroll(int id, EnrollViewModel viewmodel) // id == CourseId
        {
            if (ModelState.IsValid)
            {

                // added code start
                IEnumerable<int> listStudents = viewmodel.SelectedStudents; //novoselektiranite
                IQueryable<Enrollment> toBeRemoved = _context.Enrollment
                    .Where(s => !listStudents.Contains(s.StudentId) && s.CourseId == id && s.Semester == viewmodel.SearchSemester && s.year == viewmodel.SearchYear);
                _context.Enrollment.RemoveRange(toBeRemoved);

                IEnumerable<int> existStudents = _context.Enrollment
                    .Where(s => listStudents.Contains(s.StudentId) && s.CourseId == id && s.Semester == viewmodel.SearchSemester && s.year == viewmodel.SearchYear).Select(s => s.StudentId);
                IEnumerable<int> newStudents = listStudents.Where(s => !existStudents.Contains(s));

                foreach (int studentId in newStudents)
                {
                    _context.Enrollment.Add(new Enrollment
                    {
                        StudentId = studentId,
                        CourseId = id,
                        year = viewmodel.SearchYear,
                        Semester = viewmodel.SearchSemester
                    });
                }
                await _context.SaveChangesAsync();
                // end

                return RedirectToAction("Index1", "Admin", new { id = id });
            }


            return View(viewmodel);

        }

        //GET: Admin/Deactivate/4
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = _context.Enrollment.Where(e => e.Id == id).Include(e => e.Course).Include(e => e.Student).FirstOrDefault();
            if (enrollment == null) { return NotFound(); }
            return View(enrollment);

        }

        //Post: Admin/Deactivate/4
        [HttpPost, ActionName("Deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate(int? id, DateTime finishDate)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = _context.Enrollment.Where(e => e.Id == id).FirstOrDefault();
            enrollment.FinishDate = finishDate;
            await _context.SaveChangesAsync();
            if (enrollment == null) { return NotFound(); }
            
            return RedirectToAction("Index1", new { id = enrollment.CourseId });

        }




        /*
        // GET: Admin/Enroll
        public async Task<IActionResult> Enroll(int? id, int? searchSemester, int? searchYear)  // id e CourseId
        {
            // treba da se vrati lista od site studenti
            // lista od selektirani studenti
            // lista od enrollments

            if (id == null)
            {
                return NotFound();
            }
            IQueryable<Enrollment> enrollments = _context.Enrollment.AsQueryable();

            if (searchYear.HasValue)
            {
                enrollments = enrollments.Where(e => e.year == searchYear); // enrollments so soodvetniot semestar i godina
            }
            if (searchSemester.HasValue)
            {
                enrollments = enrollments.Where(e => e.year == searchSemester);
            }

            enrollments = enrollments.Where(e => e.CourseId == id) // enrollments so soodv kurs
                .Include(e => e.Student).Include(e => e.Course);

            // ids na studentite od tie enrollments
            IEnumerable<int> students_ids = enrollments.Select(e => e.StudentId);

            // lista od site studenti
            IEnumerable<Student> students = _context.Student.AsEnumerable();
            students = students.OrderBy(s => s.FullName);

            // lista od ids na selektiranite studenti
            var selectedStudents = students.Where(s => students_ids.Contains(s.Id)).Select(s => s.Id);

            EnrollViewModel viewmodel = new EnrollViewModel
            {

                StudentsList = new MultiSelectList(students, "Id", "FullName"), //lista od site studenti
                SelectedStudents = selectedStudents,
                Enrollments = enrollments.AsEnumerable()
            };

            return View(viewmodel);
        }
       



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int id, int? searchSemester, int? searchYear, EnrollViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {

                // added code start
                IEnumerable<int> listStudents = viewmodel.SelectedStudents; //novoselektiranite
                IQueryable<Enrollment> toBeRemoved = _context.Enrollment
                    .Where(s => !listStudents.Contains(s.StudentId) && s.CourseId == id && s.Semester == searchSemester && s.year == searchYear);
                _context.Enrollment.RemoveRange(toBeRemoved);

                IEnumerable<int> existStudents = _context.Enrollment
                    .Where(s => listStudents.Contains(s.StudentId) && s.CourseId == id && s.Semester == searchSemester && s.year == searchYear).Select(s => s.StudentId);
                IEnumerable<int> newStudents = listStudents.Where(s => !existStudents.Contains(s));

                foreach (int studentId in newStudents)
                {
                    _context.Enrollment.Add(new Enrollment {
                        StudentId = studentId, 
                        CourseId = id, 
                        year = searchYear,
                        Semester = searchSemester });
                }
                await _context.SaveChangesAsync();
                // end


                //return RedirectToAction(nameof(Index1));
            }


            return View(viewmodel);

        }
        
        */
    }
}