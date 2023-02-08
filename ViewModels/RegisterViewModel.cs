using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniversityManagementApp.Models;

namespace UniversityManagementApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Role { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public int? SelectedStudent { get; set; }
        public int? SelectedTeacher { get; set; }
        public IEnumerable<SelectListItem> StudentsList { get; set; }
        public IEnumerable<SelectListItem> TeachersList { get; set; }



    }
}
