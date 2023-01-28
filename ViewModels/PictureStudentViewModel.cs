using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using UniversityManagementApp.Models;

namespace UniversityManagementApp.ViewModels
{
    public class PictureStudentViewModel
    {
        public Student Student { get; set; }


        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }

    }
}
