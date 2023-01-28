using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace UniversityManagementApp.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set;}


        public int StudentId { get; set; }
        public Student Student { get; set; }


        //[StringLength(10)]
        public int? Semester { get; set; }

        public int? year { get; set; }

        [Display(Name = "Grade")]
        public int? grade { get; set; }

        [Display(Name = "Exam points")]
        public int? ExamPoints { get; set; }

        [Display(Name = "Seminal points")]
        public int? SeminalPoints { get; set; }


        [Display(Name = "Project points")]
        public int? ProjectPoints { get; set; }

        [Display(Name = "Additional points")]
        public int? AdditionalPoints { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Finish Date")]
        public DateTime? FinishDate { get; set; }

        [StringLength(255)]
        [Display(Name = "Seminal file")]
        public string? SeminalUrl { get; set; }

        [StringLength(255)]
        [Display(Name = "Project URL")]
        public string? ProjectUrl { get; set; }


    }
}
