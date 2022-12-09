using Microsoft.EntityFrameworkCore;
using ServerSide.Models;

namespace ServerSide.Data
{
    public class CollegeContext : DbContext
    {
        public CollegeContext(DbContextOptions<CollegeContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
        public DbSet<Average> Averages { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Course>().HasKey(c => new { c.CourseID });
            modelBuilder.Entity<Models.Task>().HasKey(c => new { c.TaskNum, c.CourseNum });
            modelBuilder.Entity<Grade>().HasKey(c => new { c.StudentID, c.CourseNum, c.TaskNum });
            modelBuilder.Entity<Enrolment>().HasKey(c => new { c.EnrolmentID });
            modelBuilder.Entity<Average>().HasKey(c => new { c.StudentID, c.courseNum });
        }


    }
}

