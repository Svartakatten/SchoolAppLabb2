namespace SchoolAppLabb2.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
