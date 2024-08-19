using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolAppLabb2.Data;
using SchoolAppLabb2.Models;
using System.Diagnostics;

namespace SchoolAppLabb2.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolAppLabb2DbContext _context;

        public HomeController(SchoolAppLabb2DbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        // Get all teachers who teach "Programming 1"
        public IActionResult GetTeachersForProgramming1()
        {
            var teachers = _context.Courses
                .Include(c => c.Teacher)
                .Where(c => c.Name == "Programming 1")
                .Select(c => c.Teacher)
                .Distinct()
                .ToList();
            return View(teachers);
        }

        // Get all students with their teachers
        public IActionResult GetStudentsWithTeachers()
        {
            var students = _context.Students
                .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                        .ThenInclude(c => c.Teacher)
                .ToList();

            return View(students);
        }

        public IActionResult GetStudentsForProgramming1()
        {
            // Query students and their corresponding teachers for the specified course
            var studentTeacherData = _context.Courses
                .Where(c => c.Name == "Programming 1")
                .SelectMany(c => c.StudentCourses)
                .Select(sc => new StudentTeacherViewModel
                {
                    StudentName = sc.Student.Name,
                    TeacherName = sc.Course.Teacher.Name
                })
                .Distinct()
                .ToList();

            return View(studentTeacherData);
        }

        // GET: EditCourseTopic/5
        [HttpGet]
        public async Task<IActionResult> EditCourseTopic(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound(); // Handle course not found
            }

            var viewModel = new EditCourseModel
            {
                CourseId = course.Id,
                OldCourseName = course.Name
            };

            return View(viewModel);
        }

        // POST: EditCourseTopic
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourseTopic(EditCourseModel model)
        {
            if (ModelState.IsValid)
            {
                var course = await _context.Courses.FindAsync(model.CourseId);
                if (course == null)
                {
                    return NotFound();
                }

                course.Name = model.NewCourseName;
                _context.Update(course);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        // GET: UpdateTeacherName
        public IActionResult UpdateTeacherName()
        {
            var model = new UpdateTeacherNameModel();
            return View(model);
        }

        // POST: UpdateTeacherName
        [HttpPost]
        public async Task<IActionResult> UpdateTeacherName(UpdateTeacherNameModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var teacher = await _context.Teachers
                .Where(t => t.Name == model.CurrentTeacherName)
                .FirstOrDefaultAsync();

            if (teacher == null)
            {
                ModelState.AddModelError(string.Empty, "Teacher not found.");
                return View(model);
            }

            teacher.Name = model.NewTeacherName;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
