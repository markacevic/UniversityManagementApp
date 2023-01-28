using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using UniversityManagementApp.Models;

namespace UniversityManagementApp.ViewModels
{
    public class SeminalViewModel
    {
        public Enrollment Enrollment { get; set; }

        [Display(Name = "Seminal file")]
        public IFormFile SeminalUrl { get; set; }
    }

}
