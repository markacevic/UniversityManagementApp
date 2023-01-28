using System.Collections;
using System.Collections.Generic;
using UniversityManagementApp.Models;

namespace UniversityManagementApp.ViewModels
{
    public class EnrollmentsBySemesterYear
    {
        public int SearchYear { get;set; }
        public int SearchSemester { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
        public int? IDCourse { get; set; }
    }
}
