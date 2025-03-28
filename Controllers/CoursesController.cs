﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using UniversityManagementApp.Data;
using UniversityManagementApp.Models;
using UniversityManagementApp.ViewModels;

namespace UniversityManagementApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly UniversityManagementAppContext _context;

        public CoursesController(UniversityManagementAppContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string searchTitle, string searchSemester, string searchProgramme)
        {
            //var universityManagementAppContext = _context.Course.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher);
            //return View(await universityManagementAppContext.ToListAsync());

            IQueryable<Course> courses = _context.Course.AsQueryable();
            IQueryable<int> semesters = _context.Course.OrderBy(c => c.Semester).Select(c => c.Semester).Distinct();
            IQueryable<string> programmes = _context.Course.OrderBy(c => c.Programme).Select(c => c.Programme).Distinct();

            if(!String.IsNullOrEmpty(searchTitle))
            {
                courses = courses.Where(c => c.Title.Contains(searchTitle)); // filter by Title
            }

            if(!string.IsNullOrEmpty(searchSemester))
            {
                int sem = Convert.ToInt32(searchSemester);
                courses = courses.Where(c => c.Semester == sem); // filter by Semester
            }

            if(!string.IsNullOrEmpty(searchProgramme))
            {
                courses = courses.Where(c => c.Programme== searchProgramme); // filter by Programme
            }

            courses = courses.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher);
            var CSPVM = new CourseSemesterProgrammeViewModel
            {
                Courses = await courses.ToListAsync(),
                Semesters = new SelectList(await semesters.ToListAsync()),
                Programmes = new SelectList(await programmes.ToListAsync()),
            };

            return View(CSPVM);
        }


        // GET: Courses By teacher
        public async Task<IActionResult> Index1(string selectTeacher)
        {
            IQueryable<string> teachers = _context.Teacher.OrderBy(t => t.LastName).Select(c => c.FullName).Distinct(); // fullnames
            IQueryable<Course> courses = _context.Course.AsQueryable();
            courses = courses.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher);
            var Courses_en = courses.AsEnumerable();

            if (!String.IsNullOrEmpty(selectTeacher))
            {
                Courses_en = Courses_en.Where(c => c.FirstTeacher.FullName == selectTeacher || c.SecondTeacher.FullName == selectTeacher);
            }

            var CoursesByTeacher = new CoursesByTeacherViewModel
            {
                Courses = Courses_en,
                Teachers = new SelectList(teachers.AsEnumerable()),
            };

            return View(CoursesByTeacher);
        }



        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }







        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "Id");
            //ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "Id");
            ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName");
            ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName");
            return View();
        }






        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "Id", course.FirstTeacherId);
            //ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "Id", course.SecondTeacherId);
            ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.SecondTeacherId);

 
            return View(course);
        }






        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var course = await _context.Course.FindAsync(id);
            var course = _context.Course.Where(c => c.Id == id).Include(c => c.Students).First();

            if (course == null)
            {
                return NotFound();
            }

            IEnumerable<Student> students = _context.Student.AsEnumerable();
            students = students.OrderBy(s => s.FullName);

            StudentsToCourseViewModel viewmodel = new StudentsToCourseViewModel
            {
                Course = course,
                StudentsList = new MultiSelectList(students, "Id", "FullName"),
                SelectedStudents = course.Students.Select(sa => sa.StudentId)
            };

            //ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "Id", course.FirstTeacherId);
            //ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "Id", course.SecondTeacherId);
            ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.SecondTeacherId);

            //return View(course);
            return View(viewmodel);

            
        }





        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        public async Task<IActionResult> Edit(int id, StudentsToCourseViewModel viewmodel)
        {
            if (id != viewmodel.Course.Id) //if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Course);  //_context.Update(course);
                    await _context.SaveChangesAsync();

                    // added code start
                    IEnumerable<int> listStudents = viewmodel.SelectedStudents; //novoselektiranite
                    IQueryable<Enrollment> toBeRemoved = _context.Enrollment.Where(s => !listStudents.Contains(s.StudentId) && s.CourseId == id);
                    _context.Enrollment.RemoveRange(toBeRemoved);

                    IEnumerable<int> existStudents = _context.Enrollment.Where(s => listStudents.Contains(s.StudentId) && s.CourseId == id).Select(s => s.StudentId); 
                    IEnumerable<int> newStudents = listStudents.Where(s => !existStudents.Contains(s)); 
                    foreach (int studentId in newStudents)
                    {
                        _context.Enrollment.Add(new Enrollment { StudentId = studentId, CourseId = id });
                    }
                    await _context.SaveChangesAsync();
                    // end
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(viewmodel.Course.Id)) //if (!CourseExists(course.Id))
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
            //ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "Id", course.FirstTeacherId);
            //ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "Id", course.SecondTeacherId);
            //ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.FirstTeacherId);
           // ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.SecondTeacherId);
            ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", viewmodel.Course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", viewmodel.Course.SecondTeacherId);

            return View(viewmodel); //return View(course);
        }




        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }





        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
