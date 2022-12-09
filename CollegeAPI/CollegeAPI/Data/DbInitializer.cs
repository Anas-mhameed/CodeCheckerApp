using CollegeAPI.Models;

namespace CollegeAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CollegeContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var Users = new User[]
            {
            new User{ UserID = 1, Name = "Gad Shor", userName = "GadShor", password = "GadShor", role = User.Role.Teacher },
            new User{ UserID = 2, Name = "Ofer Shier", userName = "Ofer", password = "Ofer", role = User.Role.Teacher },

            new User{ UserID = 3, Name = "Anas", userName = "Anas", password = "Anas", role = User.Role.Student },
            new User{ UserID = 4, Name = "Riad", userName = "Riad", password = "Riad", role = User.Role.Student },
            new User{ UserID = 5, Name = "Gonen", userName = "Gonen", password = "Gonen", role = User.Role.Student },
            new User{ UserID = 6, Name = "Ron", userName = "Ron", password = "Ron", role = User.Role.Student },
            new User{ UserID = 7, Name = "Itai", userName = "Itai", password = "Itai", role = User.Role.Student },
            new User{ UserID = 8, Name = "Roman", userName = "Roman", password = "Roman", role = User.Role.Student },
            };
            foreach (User s in Users)
            {
                context.Users.Add(s);
            }
            context.SaveChanges();

            //***************************************

            var courses = new Course[]
            {
            new Course{ CourseID = 1024, CourseName = "Java", TeacherID = 1 },
            new Course{ CourseID = 3009, CourseName = "Java", TeacherID = 2 },
            new Course{ CourseID = 8803, CourseName = "Python", TeacherID = 1 },
            new Course{ CourseID = 5601, CourseName = "C++", TeacherID = 2 },

            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();
        }
    }
}
