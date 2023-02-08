using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityManagementApp.Areas.Identity.Data;
using UniversityManagementApp.Data;
using UniversityManagementApp.Models;
using UniversityManagementApp.ViewModels;

namespace UniversityManagementApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UniversityManagementAppContext _context;
        private readonly UserManager<UniversityManagementAppUser> _userManager;
        private readonly SignInManager<UniversityManagementAppUser> _signInManager;

        public UserController(UserManager<UniversityManagementAppUser> userManager,
            SignInManager<UniversityManagementAppUser> signInManager,
            UniversityManagementAppContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            IEnumerable<Student> students = _context.Student.Where( s=> s .UserId == null).AsEnumerable();
            students = students.OrderBy(s => s.FullName);

            IEnumerable<Teacher> teachers = _context.Teacher.Where(s => s.UserId == null).AsEnumerable();
            teachers = teachers.OrderBy(s => s.FullName);

            RegisterViewModel viewmodel = new RegisterViewModel
            {
                StudentsList = new MultiSelectList(students, "Id", "FullName"),
                TeachersList = new MultiSelectList(teachers, "Id", "FullName"),
                //SelectedStudent = null,
                //SelectedTeacher = null,
                //Role = null,
                //Email = null, 
                //Password = null
            };

        
            return View(viewmodel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UniversityManagementAppUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                if (model.Role == "Teacher")
                {
                    // Link the teacher profile to the newly created user instance
                    //user.TeacherId = model.TeacherId;

                    Teacher teacher = _context.Teacher.Where(t => t.Id == model.SelectedTeacher).FirstOrDefault();
                    teacher.UserId = user.Id;
                    _context.Update(teacher);
                }
                else if (model.Role == "Student")
                {
                    // Link the student profile to the newly created user instance
                    //user.StudentId = model.StudentId;

                    Student student = _context.Student.Where(s => s.Id == model.SelectedStudent).FirstOrDefault();
                    student.UserId = user.Id;
                    _context.Update(student);
                }

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign the role to the new user
                    await _userManager.AddToRoleAsync(user, model.Role);

                    return RedirectToAction("Register", "User");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);

      
        }
    }
}
