using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using UniversityManagementApp.Data;
using UniversityManagementApp.Models;
using Microsoft.CodeAnalysis.Emit;
using UniversityManagementApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using UniversityManagementApp.Migrations;
using UniversityManagementApp.Areas.Identity.Data;
using System.Security.Claims;

namespace UniversityManagementApp.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly UniversityManagementAppContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<UniversityManagementAppUser> _userManager;

        public StudentController(UniversityManagementAppContext context, IWebHostEnvironment webHostEnvironment, UserManager<UniversityManagementAppUser> userManager)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }


        public IActionResult Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student student = _context.Student.Where(s => s.Id == id).FirstOrDefault();
            if (student == null)
            {
                return NotFound();
            }
            // restrict access
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;  
            if (userId != student.UserId)
            {
                string host = Request.Host.ToString();
                string url = $"{Request.Scheme}://{host}/Identity/Account/AccessDenied";
                return Redirect(url);
            }

            IQueryable<Enrollment> enrollments = _context.Enrollment.Where(e => e.StudentId == id).AsQueryable();
            enrollments = enrollments.Include(e => e.Course); // error if ".ThenInclude(c => c.Title)"
            var Enrollments = enrollments.AsEnumerable();
            ViewData["FullName"] = _context.Student.Where(s => s.Id == id).Select(s => s.FullName).FirstOrDefault();
            ViewData["ProfilePicture"] = _context.Student.Where(s => s.Id == id).Select(s => s.ProfilePicture).FirstOrDefault();
            return View(enrollments);
        }




        //GET: Student/Edit/1
        public async Task<IActionResult> Edit(int? id)  // id e Enrollment id
        {
            if (id == null)
            {
                return NotFound();
            }

            //var enrollment = await _context.Enrollment.FindAsync(id);
            Enrollment enrollment = _context.Enrollment.Where(e => e.Id == id).Include(e => e.Course).Include(e => e.Student).First();
           
            if (enrollment.FinishDate.HasValue)
            {
                return NotFound();
            }

            if (enrollment == null)
            {
                return NotFound();
            }

            // restrict access
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string student_userId = _context.Student.Where(s => s.Id == enrollment.StudentId).Select(s => s.UserId).FirstOrDefault();
            if (student_userId != userId)
            {
                string host = Request.Host.ToString();
                string url = $"{Request.Scheme}://{host}/Identity/Account/AccessDenied";
                return Redirect(url);
            }

            ViewData["Course"] = enrollment.CourseId; // kako da se prati Course.Title ?

            SeminalViewModel viewmodel = new SeminalViewModel()
            {
                Enrollment = enrollment,
                //SeminalUrl = null
            };


            return View(viewmodel);
        }


        

        // POST: Student/Edit/1    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SeminalViewModel viewmodel)
        {
            if (id != viewmodel.Enrollment.Id)
            {
                return NotFound();
            }

            // restrict access
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string student_userId = _context.Student.Where(s => s.Id == viewmodel.Enrollment.StudentId).Select(s => s.UserId).FirstOrDefault();
            if (student_userId != userId)
            {
                string host = Request.Host.ToString();
                string url = $"{Request.Scheme}://{host}/Identity/Account/AccessDenied";
                return Redirect(url);
            }

            Enrollment old_enrollment = _context.Enrollment.Where(e => e.Id == id).First();

            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(viewmodel);

                old_enrollment.SeminalUrl = uniqueFileName; 
                old_enrollment.ProjectUrl = viewmodel.Enrollment.ProjectUrl;
                
                _context.Update(old_enrollment);
                await _context.SaveChangesAsync();

                var myId = viewmodel.Enrollment.StudentId;
                return RedirectToAction("Index", new { id = myId }); //not working
            }

            ViewData["Course"] = viewmodel.Enrollment.CourseId; // kako da se prati Course.Title
            ViewData["Student"] = viewmodel.Enrollment.StudentId;
            return View(viewmodel);
        }


        private string UploadedFile(SeminalViewModel viewmodel)
        {
            string uniqueFileName = null;

            if (viewmodel.SeminalUrl != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "seminals");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewmodel.SeminalUrl.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    viewmodel.SeminalUrl.CopyTo(fileStream);
                }
                return uniqueFileName;
            }
            return string.Empty;

        }



    }
}
