using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolAppLabb2.Models
{
    public class UpdateTeacherNameModel
    {
        public string CurrentTeacherName { get; set; }
        public string NewTeacherName { get; set; }
    }
}
