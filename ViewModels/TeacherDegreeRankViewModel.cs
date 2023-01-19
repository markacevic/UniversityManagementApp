using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Policy;
using UniversityManagementApp.Models;

namespace UniversityManagementApp.ViewModels
{
    public class TeacherDegreeRankViewModel
    {
        public IEnumerable<Teacher> Teachers { get; set; }
        public SelectList Degrees { get; set; }
        public SelectList Ranks { get; set; }
        public string SearchFullName { get; set; }
        public string SearchAcademicRank { get; set; }
        public string SearchDegree { get; set; }    

    }
}
