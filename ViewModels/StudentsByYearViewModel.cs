using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using UniversityManagementApp.Models;

namespace UniversityManagementApp.ViewModels
{
    public class StudentsByYearViewModel
    {
        public SelectList Years { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public string SearchYear { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
