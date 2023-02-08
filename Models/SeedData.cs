using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniversityManagementApp.Areas.Identity.Data;
using UniversityManagementApp.Data;

namespace UniversityManagementApp.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<UniversityManagementAppUser>>();

            IdentityResult roleResult;

            //add admin role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin"); //dali postoi takva uloga
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); } //kreiraj uloga

            var roleCheck1 = await RoleManager.RoleExistsAsync("Student"); //dali postoi takva uloga
            if (!roleCheck1) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Student")); } //kreiraj uloga

            var roleCheck2 = await RoleManager.RoleExistsAsync("Teacher"); //dali postoi takva uloga
            if (!roleCheck2) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Teacher")); } //kreiraj uloga

            UniversityManagementAppUser user = await UserManager.FindByEmailAsync("admin@proba.com"); // najdi takov user
            if (user == null)
            { 
                var User = new UniversityManagementAppUser();

                User.Email = "admin@universitymanagmentapp.com";
                User.UserName = "admin@universitymanagmentapp.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);

                //add default user to role admin
                if (chkUser.Succeeded)
                {
                    var result1 = await UserManager.AddToRoleAsync(User, "Admin");
                }

            }
        }




        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UniversityManagementAppContext(serviceProvider.GetRequiredService<DbContextOptions<UniversityManagementAppContext>>()))
            {
                // seed Admin
                CreateUserRoles(serviceProvider).Wait(); 

                if (context.Course.Any() || context.Student.Any() || context.Teacher.Any())
                {
                    return;   // DB has been seeded
                }

                context.Student.AddRange(
                         new Student
                         { /*Id = 1, */
                             StudentIndex = "192/2017",
                             FirstName = "Tamara",
                             LastName = "Markachevikj",
                             EnrollmentDate = DateTime.Now,
                             CurrentSemester = 8
                         },
                         new Student
                         { /*Id = 2, */
                             StudentIndex = "41/2017",
                             FirstName = "Angela",
                             LastName = "Najdoska",
                             EnrollmentDate = DateTime.Now,
                             CurrentSemester = 8
                         },
                         new Student
                         { /*Id = 3, */
                             StudentIndex = "101/2017",
                             FirstName = "Marija",
                             LastName = "Milojkovska",
                             EnrollmentDate = DateTime.Now,
                             CurrentSemester = 8
                         }
                      );
                context.SaveChanges();

                context.Teacher.AddRange(
                    new Teacher
                    { /*Id = 1, */
                        FirstName = "Valentin",
                        LastName = "Rakovikj",
                        Degree = "Doctor"
                    },
                    new Teacher
                    { /*Id = 2, */
                        FirstName = "Vladimir",
                        LastName = "Atanasovski",
                        Degree = "Doctor"
                    },
                    new Teacher
                    { /*Id = 3, */
                        FirstName = "Goran",
                        LastName = "Jakimovski",
                        Degree = "Doctor"
                    },
                    new Teacher
                    { /*Id = 4, */
                        FirstName = "Marko",
                        LastName = "Porjazoski",
                        Degree = "Doctor"
                    },
                    new Teacher
                    { /*Id = 5, */
                        FirstName = "Daniel",
                        LastName = "Denkovski",
                        Degree = "Doctor"
                    },
                    new Teacher
                    { /*Id = 6, */
                        FirstName = "Pero",
                        LastName = "Latkovski",
                        Degree = "Doctor"
                    },
                    new Teacher
                    { /*Id = 7, */
                        FirstName = "Tomislav",
                        LastName = "Shuminoski",
                        Degree = "Doctor"
                    }
                 );
                context.SaveChanges();

                context.Course.AddRange(
                    new Course
                    { /*Id = 1, */
                        Title = "Introduction to WEB programming",
                        Credits = 6,
                        Semester = 5,
                        Programme = "KTI",
                        EducationLevel = "HTML/CSS/JS/JQ/ANGULAR",
                        FirstTeacherId = 1,
                        SecondTeacherId = 2
                    },
                    new Course
                    { /*Id = 2, */
                        Title = "WEB applications",
                        Credits = 6,
                        Semester = 6,
                        Programme = "KTI",
                        EducationLevel = "PHP",
                        FirstTeacherId = 3,
                        SecondTeacherId = 4
                    },
                    new Course
                    { /*Id = 3, */
                        Title = "Development of server-based WEB applications",
                        Credits = 6,
                        Semester = 6,
                        Programme = "KTI",
                        EducationLevel = "ASP.NET CORE",
                        FirstTeacherId = 5,
                        SecondTeacherId = 6
                    },
                    new Course
                    { /*Id = 4, */
                        Title = "WEB Services",
                        Credits = 6,
                        Semester = 7,
                        Programme = "KTI",
                        EducationLevel = "java/XML/JSON/SOAP/WSDL/REST",
                        FirstTeacherId = 7,
                        SecondTeacherId = 3
                    }
                );
                context.SaveChanges();





                context.Enrollment.AddRange(
                    new Enrollment
                    { /*Id = 1, */
                        CourseId = 1,
                        StudentId = 1,
                        Semester = 5,
                        year = 3,
                        grade = 4
                    },
                    new Enrollment
                    { /*Id = 2, */
                        CourseId = 1,
                        StudentId = 2,
                        Semester = 5,
                        year = 3,
                        grade = 4
                    },
                   new Enrollment
                   { /*Id = 3, */
                       CourseId = 1,
                       StudentId = 3,
                       Semester = 5,
                       year = 3,
                       grade = 4
                   },
                   new Enrollment
                   { /*Id = 4, */
                       CourseId = 2,
                       StudentId = 1,
                       Semester = 6,
                       year = 3,
                       grade = 4
                   },
                    new Enrollment
                    { /*Id = 5, */
                        CourseId = 2,
                        StudentId = 2,
                        Semester = 6,
                        year = 3,
                        grade = 4
                    },
                   new Enrollment
                   { /*Id = 6, */
                       CourseId = 2,
                       StudentId = 3,
                       Semester = 6,
                       year = 3,
                       grade = 4
                   },

                   new Enrollment
                   { /*Id = 7, */
                       CourseId = 3,
                       StudentId = 1,
                       Semester = 6,
                       year = 3,
                       grade = 4
                   },
                   new Enrollment
                   { /*Id = 7, */
                       CourseId = 4,
                       StudentId = 2,
                       Semester = 7,
                       year = 4,
                       grade = 4
                   }
                   );
                context.SaveChanges();
            }

        }
    }
}
