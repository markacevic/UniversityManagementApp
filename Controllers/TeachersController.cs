using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityManagementApp.Data;
using UniversityManagementApp.Models;
using UniversityManagementApp.ViewModels;

namespace UniversityManagementApp.Controllers
{
    public class TeachersController : Controller
    {
        private readonly UniversityManagementAppContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public TeachersController(UniversityManagementAppContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }




        // GET: Teachers
        public async Task<IActionResult> Index(string searchFullName, string searchDegree, string searchAcademicRank)
        {
            //return View(await _context.Teacher.ToListAsync());
            IEnumerable<Teacher> teachers = _context.Teacher.AsEnumerable();
            IQueryable<string> degrees = _context.Teacher.OrderBy(t => t.Degree).Select(t => t.Degree).Distinct();
            IQueryable<string> ranks = _context.Teacher.OrderBy(t => t.AcademicRank).Select(t => t.AcademicRank).Distinct();

            if (!String.IsNullOrEmpty(searchFullName))
            {
                teachers = teachers.Where(t => t.FullName.Contains(searchFullName)); // filter by FullName
            }

            if (!string.IsNullOrEmpty(searchDegree))
            {
                teachers = teachers.Where(t => t.Degree == searchDegree); // filter by Semester
            }

            if (!string.IsNullOrEmpty(searchAcademicRank))
            {
                teachers = teachers.Where(t => t.AcademicRank == searchAcademicRank); // filter by Programme
            }

            var viewmodel = new TeacherDegreeRankViewModel
            {
                Teachers =  teachers,
                Degrees = new SelectList( degrees.AsEnumerable()),
                Ranks = new SelectList( ranks.AsEnumerable()),
            };

            return View(viewmodel);
        }





        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }





        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }




        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        public async Task<IActionResult> Create(int id, PictureTeacherViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                    //new code start
                    string uniqueFileName = UploadedFile(viewmodel);

                    Teacher teacher = new Teacher
                    {
                        FirstName = viewmodel.Teacher.FirstName,
                        LastName = viewmodel.Teacher.LastName,
                        AcademicRank = viewmodel.Teacher.AcademicRank,
                        OfficeNumber = viewmodel.Teacher.OfficeNumber,
                        Degree = viewmodel.Teacher.Degree,
                        HireDate = viewmodel.Teacher.HireDate,
                        ProfilePicture = uniqueFileName
                    };
                    //end


                    _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewmodel);
        }





        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            PictureTeacherViewModel viewmodel = new PictureTeacherViewModel()
            {
                Teacher = teacher,
                //ProfileImage = null
            };


            return View(viewmodel);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        public async Task<IActionResult> Edit(int id, PictureTeacherViewModel viewmodel)
        {
            if (id != viewmodel.Teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //new code start
                    string uniqueFileName = UploadedFile(viewmodel);

                    Teacher teacher = _context.Teacher.Where(s => s.Id == id).First();

                    teacher.FirstName = viewmodel.Teacher.FirstName;
                    teacher.LastName = viewmodel.Teacher.LastName;
                    teacher.Degree = viewmodel.Teacher.Degree;
                    teacher.AcademicRank = viewmodel.Teacher.AcademicRank;
                    teacher.OfficeNumber = viewmodel.Teacher.OfficeNumber;
                    teacher.HireDate = viewmodel.Teacher.HireDate;
                    teacher.ProfilePicture = uniqueFileName;
                    //end

                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(viewmodel.Teacher.Id))
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
            return View(viewmodel);
        }

        private string UploadedFile(PictureTeacherViewModel viewmodel)
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



        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.Id == id);
        }
    }
}
