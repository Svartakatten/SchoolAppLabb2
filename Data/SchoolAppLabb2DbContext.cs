using Microsoft.EntityFrameworkCore;
using SchoolAppLabb2.Models;

namespace SchoolAppLabb2.Data
{
    public class SchoolAppLabb2DbContext : DbContext
    {
        public SchoolAppLabb2DbContext(DbContextOptions<SchoolAppLabb2DbContext> options) : base(options) { }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Teacher>().HasKey(t => t.Id);

            modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<Class>().HasData(
                new Class { Id = 1, Name = "3A" },
                new Class { Id = 2, Name = "3B" }
            );

            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { Id = 1, Name = "Reidar" },
                new Teacher { Id = 2, Name = "Johanna" },
                new Teacher { Id = 3, Name = "Herald" }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, Name = "Alice" },
                new Student { Id = 2, Name = "Bob" }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Programming 1", TeacherId = 1 },
                new Course { Id = 2, Name = "Programming 2", TeacherId = 2 },
                new Course { Id = 3, Name = "Programming 3", TeacherId = 3 }
            );

            modelBuilder.Entity<StudentCourse>().HasData(
                new StudentCourse { StudentId = 1, CourseId = 1 },
                new StudentCourse { StudentId = 2, CourseId = 2 }
            );
        }
    }
}
