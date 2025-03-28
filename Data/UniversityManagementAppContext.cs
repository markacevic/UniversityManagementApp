﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversityManagementApp.Areas.Identity.Data;
using UniversityManagementApp.Models;

namespace UniversityManagementApp.Data
{
    public class UniversityManagementAppContext : IdentityDbContext<UniversityManagementAppUser> //DbContext
    {
        public UniversityManagementAppContext(DbContextOptions<UniversityManagementAppContext> options)
            : base(options)
        {
        }

        public DbSet<UniversityManagementApp.Models.Student> Student { get; set; }

        public DbSet<UniversityManagementApp.Models.Course> Course { get; set; }

        public DbSet<UniversityManagementApp.Models.Teacher> Teacher { get; set; }

        public DbSet<UniversityManagementApp.Models.Enrollment> Enrollment { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Enrollment>().HasOne<Student>(p => p.Student).WithMany(p => p.Courses).HasForeignKey(p => p.StudentId); //student.cs
            //builder.Entity<Enrollment>().HasOne<Course>(p => p.Course).WithMany(p => p.Students).HasForeignKey(p => p.CourseId ); //course.cs

            //builder.Entity<Course>().HasOne(c => c.FirstTeacher).WithMany(t => t.Courses).HasForeignKey(c => c.FirstTeacherId);
            //builder.Entity<Course>().HasOne(c => c.SecondTeacher).WithMany(t => t.Courses).HasForeignKey(c => c.SecondTeacherId);

            base.OnModelCreating(builder);

            builder.Entity<Teacher>()
            .HasOne(t => t.User)
            .WithOne(u => u.Teacher)
            .HasForeignKey<Teacher>(t => t.UserId);

            builder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId);

        }
    }
}

