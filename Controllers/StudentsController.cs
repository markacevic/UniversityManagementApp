using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Hosting;
using UniversityManagementApp.Data;
using UniversityManagementApp.Models;
using UniversityManagementApp.ViewModels;

namespace UniversityManagementApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly UniversityManagementAppContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;



        public StudentsController(UniversityManagementAppContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index1(string selectCourse)
        {
            IQueryable<string> courses = _context.Course.OrderBy(c => c.Title).Select(c => c.Title).Distinct(); 
            IQueryable<Student> students = _context.Student.AsQueryable();
            
            if (!String.IsNullOrEmpty(selectCourse))
            { 
                int CourseID = _context.Course.Where(c => c.Title.Contains(selectCourse)).FirstOrDefault().Id;
                IQueryable<int> students_ids = _context.Enrollment.Where(e => e.CourseId == CourseID).Select(e => e.StudentId).AsQueryable();
                students = students.Where(s => students_ids.Contains(s.Id));
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }






        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Create([Bind("Id,StudentIndex,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemester,EducationLevel")] Student student)
        public async Task<IActionResult> Create(int id, PictureStudentViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                //new code start
                string uniqueFileName = UploadedFile(viewmodel);

                Student student = new Student
                {
                    StudentIndex = viewmodel.Student.StudentIndex,
                    FirstName = viewmodel.Student.FirstName,
                    LastName = viewmodel.Student.LastName,
                    EnrollmentDate = viewmodel.Student.EnrollmentDate,
                    AcquiredCredits = viewmodel.Student.AcquiredCredits,
                    CurrentSemester = viewmodel.Student.CurrentSemester,
                    EducationLevel = viewmodel.Student.EducationLevel,
                    ProfilePicture = uniqueFileName
                };
                //end

                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewmodel);
        }





        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
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
            PictureStudentViewModel viewmodel = new PictureStudentViewModel() // ne znam dali e potrebno
            {
                Student = student,
                ProfileImage = null
            };


            return View(viewmodel);
        }






        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,StudentIndex,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemester,EducationLevel")] Student student)
        public async Task<IActionResult> Edit(int id,  PictureStudentViewModel viewmodel)
        {
            if (id != viewmodel.Student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //new code start
                    string uniqueFileName = UploadedFile(viewmodel);

                    Student student = _context.Student.Where(s => s.Id == id).First();

                    student.StudentIndex = viewmodel.Student.StudentIndex;
                    student.FirstName = viewmodel.Student.FirstName;
                    student.LastName = viewmodel.Student.LastName;
                    student.EnrollmentDate = viewmodel.Student.EnrollmentDate;
                    student.AcquiredCredits = viewmodel.Student.AcquiredCredits;
                    student.CurrentSemester = viewmodel.Student.CurrentSemester;
                    student.EducationLevel = viewmodel.Student.EducationLevel;
                    student.ProfilePicture = uniqueFileName;
                    //end

                    _context.Update(student);
                    await _context.SaveChangesAsync();
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(viewmodel.Student.Id))
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
            return NotFound(); //View(viewmodel);
        }


        private string UploadedFile(PictureStudentViewModel viewmodel)
        {
            string uniqueFileName = null;

            if (viewmodel.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewmodel.ProfileImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    viewmodel.ProfileImage.CopyTo(fileStream);
                }
                return uniqueFileName;
            }
            return string.Empty;
            
        }



















        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
