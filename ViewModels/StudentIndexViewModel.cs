using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using UniversityManagementApp.Models;

namespace UniversityManagementApp.ViewModels
{
    public class StudentIndexViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public SelectList Indexes { get; set; }
        public string SearchFullName { get; set; }
        public string SearchIndex { get; set; }
    }
}
