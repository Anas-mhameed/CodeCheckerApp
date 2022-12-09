using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerSide.Models;

namespace ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegeController : ControllerBase
    {
        private readonly CollegeContext _context;

        public CollegeController(CollegeContext context)
        {
            _context = context;
        }

        [HttpGet("user/{userName}/{pass}")]
        public async Task<ActionResult<User>> GetUser(string? userName, string? pass)
        {
            if (userName == null || pass == null)
            {
                return BadRequest("invalid input!");
            }

            var user = await _context.Users.Where(x => x.userName == userName && x.password == pass).ToListAsync();
            if (user.Count == 0)
            {
                return BadRequest("User Not Found");
            }
            return Ok(user.Find(x => x.userName == userName && x.password == pass));
        }

        [HttpPost("user")]

        public async Task<ActionResult<List<User>>> InsertUser(User user)
        {
            if (user == null)
            {
                return BadRequest("invalid input!");
            }
            var dbuser = await _context.Users.Where(x => x.userName == user.userName).ToListAsync();
            if (dbuser.Count != 0)
            {
                return BadRequest("user already exist!!");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpGet("StudentCourses/{userId}")]
        public async Task<ActionResult<List<Course>>> GetStudentCourses(int userId)
        {
            var dbUser = await _context.Users.FindAsync(userId);

            if (dbUser == null)
            {
                return BadRequest("Student Not Found");
            }

            var userEnrolments = await _context.Enrolments.Where(x => x.UserID == userId).ToListAsync();
            if (userEnrolments.Count == 0)
            {
                return Ok(new Course[] { });
            }


            var courses = new List<Course> { };

            foreach (Enrolment e in userEnrolments)
            {

                var temp = await _context.Courses.Where(x => x.CourseNum == e.CourseNum).ToListAsync();

                courses.Add(temp.Find(x => x.CourseNum == e.CourseNum));

            }


            return Ok(courses);
        }

        [HttpGet("TeacherCourses/{userId}")]
        public async Task<ActionResult<List<Course>>> GetTeacherCourses(int userId)
        {
            var dbUser = await _context.Users.FindAsync(userId);

            if (dbUser == null)
            {
                return BadRequest("Teacher Not Found");
            }

            var courses = await _context.Courses.Where(x => x.TeacherID == userId).ToListAsync();

            return Ok(courses);
        }

        [HttpPost("Course")]

        public async Task<ActionResult<Course>> InsertCourse(Course course)
        {

            if (course == null)
            {
                return BadRequest("invalid input!");
            }
            var dbcourse = await _context.Courses.Where(x => x.CourseNum == course.CourseNum).ToListAsync();
            if (dbcourse.Count != 0)
            {
                return BadRequest("Course already exist!");
            }
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        [HttpGet("Course/{courseId}")]

        public async Task<ActionResult<Course>> GetCourse(int courseId)
        {

            var dbcourse = await _context.Courses.FindAsync(courseId);
            if (dbcourse == null)
            {
                return BadRequest("Not Found!");
            }

            return Ok(dbcourse);
        }

        [HttpPut("Course/{courseID}")]

        public async Task<ActionResult<Course>> UpdateCourse(int courseID, Course newCourse)
        {

            var course = await _context.Courses.FindAsync(courseID);
            if (course == null)
            {
                return BadRequest("Not Found!");
            }

            course.CourseNum = (newCourse.CourseNum == -1 ? course.CourseNum : newCourse.CourseNum); 
            course.CourseName = (newCourse.CourseName == "" ? course.CourseName : newCourse.CourseName);
            course.Description = (newCourse.Description == "" ? course.Description : newCourse.Description);
            if (newCourse.TeacherID != -1)
            {
                var dbteacher = await _context.Users.FindAsync(newCourse.TeacherID);
                if(dbteacher == null)
                    return BadRequest("teacher Not Found!");
                
                if(dbteacher.role == Models.User.Role.Student)
                    return BadRequest("This id not a teacher");

                course.TeacherID = dbteacher.UserID;

            }

            await _context.SaveChangesAsync();

            return Ok(course);
        }

        [HttpDelete("Course/{courseID}")]
        public async Task<ActionResult<Course>> DeleteCourse(int courseID)
        {
            var dbCourse = await _context.Courses.FindAsync(courseID);
            if (dbCourse == null)
                return BadRequest("Not Found!");
            _context.Courses.Remove(dbCourse);
            await _context.SaveChangesAsync();
            return dbCourse;
        }

        [HttpPost("Enrolment")]

        public async Task<ActionResult<Enrolment>> AddEnrolment(Enrolment e)
        {
            if (e == null)
            {
                return BadRequest("invalid input!");
            }

            var dbTask = await _context.Enrolments.Where(x => x.CourseNum == e.CourseNum && x.UserID == e.UserID).ToListAsync();
            if (dbTask.Count != 0)
            {
                return BadRequest("Enrolment is Already exist!");
            }
            _context.Enrolments.Add(e);
            await _context.SaveChangesAsync();

            return Ok(e);

        }


        [HttpPost("Task")]

        public async Task<ActionResult<Models.Task>> InsertTask(Models.Task task)
        {
            if (task == null)
            {
                return BadRequest("invalid input!");
            }

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }

        [HttpDelete("Task")]
        public async Task<ActionResult<Models.Task>> DeleteTask(Models.Task t)
        {
            var dbtask = await _context.Tasks.Where(x => x.TaskNum == t.TaskNum && x.CourseNum == t.CourseNum).ToListAsync();
            if (dbtask.Count == 0)
            {
                return BadRequest("Task Not Found");
            }
            _context.Tasks.Remove(dbtask.FirstOrDefault());
            await _context.SaveChangesAsync();
            return Ok(dbtask);
        }

        [HttpGet("Task/{CourseNum}")]
        public async Task<ActionResult<List<Models.Task>>> GetCourseTasks(int CourseNum)
        {

            var tasks = await _context.Tasks.Where(x => x.CourseNum == CourseNum).ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("Task/{CourseNum}/{TaskNum}")]
        public async Task<ActionResult<Models.Task>> GetTask(int CourseNum, int TaskNum)
        {

            var tasks = await _context.Tasks.Where(x => x.CourseNum == CourseNum && x.TaskNum == TaskNum).ToListAsync();

            return Ok(tasks.Find(x => x.CourseNum == CourseNum && x.TaskNum == TaskNum));
        }



        [HttpGet("Users/{CourseNum}")]
        public async Task<ActionResult<List<User>>> GetCourseStudents(int CourseNum)
        {
            var dbEnrolments = await _context.Enrolments.Where(x => x.CourseNum == CourseNum).ToListAsync();

            var users = new List<User> { };

            foreach (var e in dbEnrolments)
            {
                var temp = await _context.Users.FindAsync(e.UserID);
                users.Add(temp);
            }

            return Ok(users);
        }

        [HttpGet("Grade/{courseNum}/{studentID}/{taskNum}")]
        public async Task<ActionResult<Grade>> GetGrade(int courseNum, int studentID, int taskNum)
        {
            var grade = await _context.Grades.Where(x => x.StudentID == studentID && x.CourseNum == courseNum && x.TaskNum == taskNum).ToListAsync();

            return Ok(grade.Find(x => x.StudentID == studentID && x.CourseNum == courseNum && x.TaskNum == taskNum));
        }

        [HttpPost("Grade")]
        public async Task<ActionResult<Grade>> InsertGrade(Grade grade)
        {
            if (grade == null)
            {
                return BadRequest("invalid input!");
            }

            var dbgrade = await _context.Grades.Where(x => x.TaskNum == grade.TaskNum && x.CourseNum == grade.CourseNum && x.StudentID == grade.StudentID).ToListAsync();
            if (dbgrade.Count != 0)
            {
                return BadRequest("grade already exist!");
            }

            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();

            return Ok(grade);
        }

        [HttpPut("Grade/{degree}")]
        public async Task<ActionResult<Grade>> UpdateGrade(Grade grade, int degree)
        {
            var dbGrade = await _context.Grades.Where(x => x.TaskNum == grade.TaskNum && x.CourseNum == grade.CourseNum && x.StudentID == grade.StudentID).ToListAsync();
            if (dbGrade.Count == 0)
            {
                return BadRequest("Grade Not Found");
            }

            dbGrade.Find(x => x.TaskNum == grade.TaskNum && x.CourseNum == grade.CourseNum && x.StudentID == grade.StudentID).grade = degree;
            await _context.SaveChangesAsync();

            return Ok(dbGrade);
        }

        [HttpDelete("Grade")]
        public async Task<ActionResult<Grade>> DeleteGrade(Grade g)
        {
            var dbGrade = await _context.Grades.Where(x => x.StudentID == g.StudentID && x.CourseNum == g.CourseNum && x.TaskNum == g.TaskNum).ToListAsync();
            if (dbGrade.Count == 0)
            {
                return BadRequest("Grade Not Found!");
            }

            _context.Grades.Remove(dbGrade.FirstOrDefault());
            await _context.SaveChangesAsync();

            return Ok(dbGrade);
        }

        // Helper !
        private async Task<double> StudentAverage(int courseNum, int StudentID)
        {
            var dbGrades = await _context.Grades.Where(x => x.CourseNum == courseNum && x.StudentID == StudentID).ToListAsync();
            var dbtasks = await _context.Tasks.Where(x => x.CourseNum == courseNum).ToListAsync();
            int total = 0;
            foreach (var g in dbGrades)
            {
                total += g.grade;

            }
            return (double)total / (dbtasks.Count);
        }

        //task average for teacher
        [HttpGet("TaskAverage/{courseNum}/{taskNum}")]
        public async Task<ActionResult<double>> TaskAverage(int courseNum, int taskNum)
        {
            var dbGrades = await _context.Grades.Where(x => x.CourseNum == courseNum && x.TaskNum == taskNum).ToListAsync();
            var dbEnrolments = await _context.Enrolments.Where(x => x.CourseNum == courseNum).ToListAsync();

            int total = 0;
            foreach (var g in dbGrades)
            {
                total += g.grade;
            }

            return Ok(Math.Round((double)total / dbEnrolments.Count, 2));
        }

        //task grades for teacher
        [HttpGet("TaskGrades/{courseNum}/{taskNum}")]
        public async Task<ActionResult<List<Grade>>> TaskGrades(int courseNum, int taskNum)
        {
            var dbGrades = await _context.Grades.Where(x => x.CourseNum == courseNum && x.TaskNum == taskNum).ToListAsync();

            return Ok(dbGrades);
        }

        //course average for teacher 
        [HttpGet("CourseAverage/{courseNum}")]
        public async Task<ActionResult<double>> CourseAverage(int courseNum)
        {
            //getting all students
            var dbEnrolments = await _context.Enrolments.Where(x => x.CourseNum == courseNum).ToListAsync();

            var allStudents = new List<User> { };

            foreach (var e in dbEnrolments)
            {
                var temp = await _context.Users.FindAsync(e.UserID);
                allStudents.Add(temp);
            }

            //start callculating averages for each one 
            double total = 0;
            foreach (var student in allStudents)
            {
                var average = await StudentAverage(courseNum, student.UserID);
                total += average;
            }

            var avg = Math.Round(total / allStudents.Count, 2);
            return Ok(avg);

        }

        [HttpGet("CourseGrades/{courseNum}")]
        public async Task<ActionResult<List<Average>>> CourseGrades(int courseNum)
        {
            var dbCourseAverage = await _context.Averages.Where(x => x.courseNum == courseNum).ToListAsync();

            return Ok(dbCourseAverage);
        }

        [HttpPost("CourseGrades")]
        public async Task<ActionResult<List<Average>>> InsertAverage(Average avg)
        {
            var dbAvg = await _context.Averages.Where(x => x.courseNum == avg.courseNum && x.StudentID == avg.StudentID).ToListAsync();
            var avgObj = dbAvg.Find(x => x.courseNum == avg.courseNum && x.StudentID == avg.StudentID);
            if (avgObj != null)
            {
                return BadRequest("Average Already Exist");
            }

            _context.Averages.Add(avg);
            await _context.SaveChangesAsync();
            return Ok(avg);

        }

        [HttpPut("CourseGrades")]
        public async Task<ActionResult<List<Average>>> UpdaterAverage(Average avg, double average)
        {
            var dbAvg = await _context.Averages.Where(x => x.courseNum == avg.courseNum && x.StudentID == avg.StudentID).ToListAsync();
            var avgObj = dbAvg.Find(x => x.courseNum == avg.courseNum && x.StudentID == avg.StudentID);
            if (avgObj == null)
            {
                return BadRequest("Average Already Exist");
            }

            avgObj.average = average;
            await _context.SaveChangesAsync();
            return Ok(avgObj);

        }

    }
}
