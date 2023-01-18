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

        public int Semester { get; set; }

        [StringLength(100)]
        public string? Programme { get; set; }

        [StringLength(100)]
        [Display(Name = "Education level")]
        public string? EducationLevel { get; set; }

       // ICollection<CourseTeacher> Teachers { get; set; } //one course can have two(many teachers)


        public int? FirstTeacherId { get; set; } // one-to-one

        //[ForeignKey("FirstTeacherId")]
        public Teacher FirstTeacher { get; set; } // reference object 



        public int? SecondTeacherId { get; set; } // one-to-one

       // [ForeignKey("SecondTeacherId")]
        public Teacher SecondTeacher { get; set; }

        ICollection<Enrollment> Students { get; set; } // one course can be participated by many students
    }
}
