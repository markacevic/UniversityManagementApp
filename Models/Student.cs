using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityManagementApp.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Display(Name = "Student Index")]
        [StringLength(10)]
        public string StudentIndex { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Enrollment Date")]
        [DataType(DataType.Date)] // podatocen tip DATUM
        public DateTime? EnrollmentDate { get; set; }


        public int? AcquiredCredits { get; set; }
        public int? CurrentSemester { get; set; }

        [Display(Name = "Education Level")]
        [StringLength(25)]
        public string? EducationLevel { get; set; }

        public ICollection<Enrollment> Courses { get; set; } // one student can be enrolled to many courses




    }
}
