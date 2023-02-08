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
        public Teacher Teacher { get; set; }
        public Student Student { get; set; }
       // public List<IdentityUserRole<string>> Roles { get; set; }
    }
}
