namespace SchoolAppLabb2.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
