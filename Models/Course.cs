using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityManagementApp.Models
{
    public class Course
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public int Credits { get; set; }

        // treba validator
        public int Semester { get; set; }

        [StringLength(100)]
        public string? Programme { get; set; }

        [StringLength(100)]
        [Display(Name = "Education level")]
        public string? EducationLevel { get; set; }

        // ICollection<CourseTeacher> Teachers { get; set; } //one course can have two(many teachers)


        [Display(Name = "First Teacher Id")]
        public int? FirstTeacherId { get; set; } // one-to-one

        //[ForeignKey("FirstTeacherId")]
        [Display(Name = "First Teacher")]
        public Teacher FirstTeacher { get; set; } // reference object 


        [Display(Name = "Second Teacher Id")]
        public int? SecondTeacherId { get; set; } // one-to-one

        // [ForeignKey("SecondTeacherId")]
        [Display(Name = "Second Teacher")]
        public Teacher SecondTeacher { get; set; }

        public ICollection<Enrollment> Students { get; set; } // one course can be participated by many students
    }
}
