using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniversityManagementApp.Models;

namespace UniversityManagementApp.ViewModels
{
    public class CourseSemesterProgrammeViewModel
    {
        public IList<Course> Courses { get; set; }
        public SelectList Semesters { get; set; }   
        public SelectList Programmes { get; set; }  
        public string SearchProgramme { get; set; }
        public string SearchSemester { get; set; }
        public string SearchTitle { get; set; }
    }
}
