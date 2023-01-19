using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace UniversityManagementApp.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string? Degree { get; set; }


        [Display(Name = "Academic Rank")]
        [StringLength(25)]
        public string? AcademicRank { get; set; }

        [Display(Name = "Office number")]
        [StringLength(25)]
        public string? OfficeNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        public string FullName 
        { 
            get { return String.Format("{0} {1}", FirstName, LastName); } 
        }

        ICollection<Course> Courses { get; set; } // one-to-many

    }
}
