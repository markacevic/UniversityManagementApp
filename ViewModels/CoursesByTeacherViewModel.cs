using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using UniversityManagementApp.Models;

namespace UniversityManagementApp.ViewModels
{
    public class CoursesByTeacherViewModel
    {
        public IEnumerable<Course> Courses { get; set; }
        public SelectList Teachers { get; set; }
        public string SelectTeacher { get; set; }
        
    }
}
