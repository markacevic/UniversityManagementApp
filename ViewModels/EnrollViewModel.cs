using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using UniversityManagementApp.Models;

namespace UniversityManagementApp.ViewModels
{
    public class EnrollViewModel
    {
       // public IEnumerable<Enrollment> Enrollments { get; set; }
        public IEnumerable<int> SelectedStudents { get; set; }
        public IEnumerable<SelectListItem> StudentsList { get; set; }
        public int SearchYear { get; set; }
        public int SearchSemester { get; set; }


    }
}
