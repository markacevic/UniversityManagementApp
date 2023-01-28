using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UniversityManagementApp.Models;

namespace UniversityManagementApp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the UniversityManagementAppUser class
    public class UniversityManagementAppUser : IdentityUser
    {
        public string Role { get; set; }
        public int? TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public int? StudentId { get; set; }
        public Student Student { get; set; }
    }
}
