using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using UniversityManagementApp.Models;

namespace UniversityManagementApp.ViewModels
{
    public class StudentsByCourseViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public SelectList Courses { get; set; }
        public string SelectCourse { get; set; }
    }
}
