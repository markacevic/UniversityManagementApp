using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using UniversityManagementApp.Data;
using UniversityManagementApp.Models;
using UniversityManagementApp.ViewModels;

namespace UniversityManagementApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly UniversityManagementAppContext _context;



        public StudentsController(UniversityManagementAppContext context)
        {
            _context = context;
        }




        // GET: Students
        public async Task<IActionResult> Index(string searchIndex, string searchFullName)
        {
            //return View(await _context.Student.ToListAsync());

            IEnumerable<Student> students = _context.Student.AsEnumerable(); // Enumerable because of FullName
            IQueryable<string> indexes = _context.Student.OrderBy(s => s.StudentIndex).Select(s => s.StudentIndex).Distinct();

            if (!String.IsNullOrEmpty(searchFullName))
            {
                students = students.Where(s => s.FullName.Contains(searchFullName)); ; // filter by FullName
            }

            if (!string.IsNullOrEmpty(searchIndex))
            {
                students = students.Where(s => s.StudentIndex == searchIndex); // filter by Index
            }

            var viewmodel = new StudentIndexViewModel
            {
                Students = students,
                Indexes = new SelectList( indexes.AsEnumerable()),
            };

            return View(viewmodel);
        }



        // GET: Students By Course
        public async Task<IActionResult> Index1(string selectCourse)
        {
            IQueryable<string> courses = _context.Course.OrderBy(c => c.Title).Select(c => c.Title).Distinct(); 
            IQueryable<Student> students = _context.Student.AsQueryable();

            if (!String.IsNullOrEmpty(selectCourse))
            { 
                int CourseID = _context.Course.Where(c => c.Title.Contains(selectCourse)).FirstOrDefault().Id;
                students = students.Where(s => s.Id == CourseID);
            }

            var viewmodel = new StudentsByCourseViewModel
            {
                Students = students.AsEnumerable(),
                Courses = new SelectList(courses.AsEnumerable()),
            };

            return View(viewmodel);
        }






        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }





        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }






        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentIndex,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemester,EducationLevel")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }





        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }






        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentIndex,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemester,EducationLevel")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }





        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }




        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
