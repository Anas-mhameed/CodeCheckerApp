using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using CollegeAPI.Models;
using System.Net.Http;
using System.Globalization;
using System.Text;

using CsvHelper;
using System.Net;
using Task = CollegeAPI.Models.Task;

namespace ClientSide.View
{
    /// <summary>
    /// Interaction logic for Deliver.xaml
    /// </summary>
    public partial class Deliver : Window
    {
        string selected_cource = "courses...";
        string selected_task = "task number...";
        static HttpClient client;
        List<Course> courses;
        User user;
        public int coursNum;
        public int courseID;
        string output = "";
        string json = "";
        string oldinfo = "";

        public Deliver(List<Course> cc, User u)
        {
            client = new HttpClient();
            try
            {
                client = new HttpClient();
                //courses = new List<Course>();
                client.BaseAddress = new Uri("https://localhost:7286/swagger/");

            }
            catch
            {
                MessageBox.Show("connection faild");
            }
            
            courses = cc;
            user = u;
            InitializeComponent();
            tcourses.Items.Add("courses...");
            foreach (var i in courses)
            {
                tcourses.Items.Add(i.CourseName);
            }
            tcourses.SelectedIndex = 0;
        }
        static async Task<List<CollegeAPI.Models.Task>> GetTasks(int courseNum)
        {
            List<CollegeAPI.Models.Task> tasks;
            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync($"/api/College/Task/{courseNum}");
            }
            catch
            {
                MessageBox.Show("response faild");
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    tasks = await response.Content.ReadAsAsync<List<CollegeAPI.Models.Task>>();

                    return tasks;

                }
                catch
                {
                    MessageBox.Show("failed to get the tasks");
                }

            }

            return null;
        }

        //GETTING task average
        static async Task<double> GetTaskAverage(int courseNum, int taskNum)
        {
            double average;
            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync($"/api/College/TaskAverage/{courseNum}/{taskNum}");
            }
            catch
            {
                MessageBox.Show("response faild");
                return 0;
            }

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    average = await response.Content.ReadAsAsync<double>();

                    return average;

                }
                catch
                {
                    MessageBox.Show("failed to get the task average");
                }

            }

            return 0;
        }

        // getting task grades
        static async Task<List<Grade>> GetTaskGrades(int courseNum, int taskNum)
        {
            List<Grade> grades;
            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync($"/api/College/TaskGrades/{courseNum}/{taskNum}");
            }
            catch
            {
                MessageBox.Show("response faild");
                return new List<Grade>();
            }

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    grades = await response.Content.ReadAsAsync<List<Grade>>();

                    return grades;

                }
                catch
                {
                    MessageBox.Show("failed to get the task average");
                }

            }

            return new List<Grade>();
        }

        // getting course average
        static async Task<double> GetCourseAverage(int courseNum)
        {
            double average;
            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync($"/api/College/CourseAverage/{courseNum}");
            }
            catch
            {
                MessageBox.Show("response faild");
                return 0;
            }

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    average = await response.Content.ReadAsAsync<double>();

                    return average;

                }
                catch
                {
                    MessageBox.Show("failed to get the task average");
                }

            }

            return 0;
        }

        static async Task<Uri> CreateProductAsync(CollegeAPI.Models.Task task)
        {

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/College/Task", task);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            // return URI of the created resource.
            return response.Headers.Location;
        }
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        static async Task<Course> GetCourse(int courseID)
        {
            Course course;
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync($"/api/College/Course/{courseID}");
            }
            catch
            {
                MessageBox.Show("response faild");
                return null;
            }

            if (response.IsSuccessStatusCode)
            {

                try
                {
                    course = await response.Content.ReadAsAsync<Course>();

                    return course;

                }
                catch
                {
                    MessageBox.Show("failed to get the Course ");
                }

            }

            return null;

        }

        static async Task<Course> CreateCourseAsync(Course course)
        {

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/College/Course", course);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            var res = await response.Content.ReadAsAsync<Course>();
            // return URI of the created resource.
            return res;
        }

        static async Task<Course> UpdateCourseAsync(Course course, int courseId)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"/api/College/Course/{courseId}", course);
            var UpdatedCourse = await response.Content.ReadAsAsync<Course>();
            return UpdatedCourse;
        }

        static async Task<Course> DeleteCourseAsync(int courseId)
        {
            HttpResponseMessage response = await client.DeleteAsync(
            $"/api/College/Course/{courseId}");
            return await response.Content.ReadAsAsync<Course>(); 
        }
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        // getting course average
        static async Task<List<Average>> GetCourseGrades(int courseNum)
        {
            List<Average> grades;
            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync($"/api/College/CourseGrades/{courseNum}");
            }
            catch
            {
                MessageBox.Show("response faild");
                return null;
            }

            if (response.IsSuccessStatusCode)
            {

                try
                {
                    grades = await response.Content.ReadAsAsync<List<Average>>();

                    return grades;

                }
                catch
                {
                    MessageBox.Show("failed to get the task average");
                }

            }

            return null;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected_cource = (string)tcourses.SelectedItem;
            List<CollegeAPI.Models.Task> tasks;
            if (selected_cource == "courses..." && addass != null && getcg != null)
            {
                addcourse.IsEnabled = true;
                cnentry.Text = "";
                addcbblock.Visibility = Visibility.Collapsed;
                courseStuff.Visibility = Visibility.Collapsed;
                addcourse.Visibility = Visibility.Visible;
                remcourse.Visibility = Visibility.Collapsed;
                cinf.Visibility = Visibility.Collapsed;


                addass.Visibility = Visibility.Collapsed;
                getcg.Visibility = Visibility.Collapsed;
                tcombo.Visibility = Visibility.Collapsed;
                wrap.Visibility = Visibility.Collapsed;
                getavg.Visibility = Visibility.Collapsed;
                gettg.Visibility = Visibility.Collapsed;
                gcavg.Visibility = Visibility.Collapsed;
                opath.Text = "output.txt file:";
                jpath.Text = "ruls.json file:";
                tcombo.SelectedIndex = 0;
                selected_task = "task number...";



            }
            else if (addass != null && getcg != null)
            {
                cnentry.Text = "";
                addcbblock.Visibility = Visibility.Collapsed;
                courseStuff.Visibility = Visibility.Visible;
                remcourse.Visibility = Visibility.Visible;
                addcourse.Visibility = Visibility.Collapsed;
                addass.Visibility = Visibility.Visible;
                getcg.Visibility = Visibility.Visible;
                tcombo.Visibility = Visibility.Visible;
                gcavg.Visibility = Visibility.Visible;
                wrap.Visibility = Visibility.Collapsed;
                getavg.Visibility = Visibility.Collapsed;
                gettg.Visibility = Visibility.Collapsed;
                cinf.Visibility = Visibility.Visible;

                foreach (Course c in courses)
                {
                    if (c.CourseName == tcourses.SelectedItem) { 
                        coursNum = c.CourseNum;
                        courseID = c.CourseID;
                        courseinfo.Text = c.Description;
                        oldinfo = c.Description;
                    }

                }
                tasks = await GetTasks(coursNum);
                tcombo.Items.Clear();
                tcombo.Items.Add("assighnment number...");
                foreach (CollegeAPI.Models.Task t in tasks)
                {
                    tcombo.Items.Add(t.TaskNum.ToString());
                }
                tcombo.SelectedIndex = 0;
            }
        }

        private void addass_Click(object sender, RoutedEventArgs e)
        {
            json = "";
            output = "";
            wrap.Visibility = Visibility.Visible;
            addass.Visibility = Visibility.Hidden;
            courseStuff.Visibility = Visibility.Collapsed;

        }
        private async void getcg_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = selected_cource + "_" + "Grades"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "csv file (.csv)|*.csv"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                int cid = -1;
                foreach (var c in courses)
                {
                    if (c.CourseName == selected_cource)
                    {
                        cid = c.CourseNum;
                    }
                }
                string filename = dlg.FileName;
                var averages = await GetCourseGrades(cid);
                string s = $"cuorse:{selected_cource}\n" + "student_ID,average\n";

                foreach (var g in averages)
                {
                    s += g.StudentID + "," + g.average + "\n";
                }
                File.WriteAllText(filename, s);
                MessageBox.Show($"the file saved in :{filename}");
            }
        }

        private async void submit_Click(object sender, RoutedEventArgs e)
        {
            if (json != "" && output != "")
            {
                wrap.Visibility = Visibility.Collapsed;
                addass.Visibility = Visibility.Visible;
                opath.Text = "output.txt file:";
                jpath.Text = "ruls.json file:";
                int tn = tcombo.Items.Count;

                Task t = new Task()
                {
                    CourseNum = coursNum,
                    TaskNum = tn,
                    JsonFile = json,
                    RoleFile = output

                };
                await CreateProductAsync(t);
                json = "";
                output = "";
                tcombo.Items.Add("" + tn);
            }
            else
            {
                MessageBox.Show("json filr or rules file are missing");
            }

            courseStuff.Visibility = Visibility.Visible;

        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            jpath.Text = "ruls.json file:";
            opath.Text = "output.txt file:";
            wrap.Visibility = Visibility.Collapsed;
            addass.Visibility = Visibility.Visible;
            json = "";
            output = "";

            courseStuff.Visibility = Visibility.Visible;
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            var newForm = new LoginView(); //create your new form.
            newForm.Show(); //show the new form.
            this.Close(); //only if you want to close the current form.

        }

        private void addrules_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            string fname, filecontent;

            if (openFileDialog.ShowDialog() == true)
            {
                fname = openFileDialog.FileName;
                opath.Text += " " + fname;
                opath.Visibility = Visibility.Visible;
                filecontent = File.ReadAllText(openFileDialog.FileName);
                cancel.Visibility = Visibility.Visible;
                submit.Visibility = Visibility.Visible;
                output = File.ReadAllText(fname);

            }
        }

        private void addjson_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            string fname, filecontent;

            if (openFileDialog.ShowDialog() == true)
            {
                fname = openFileDialog.FileName;
                jpath.Text += " " + fname;
                jpath.Visibility = Visibility.Visible;
                filecontent = File.ReadAllText(openFileDialog.FileName);
                cancel.Visibility = Visibility.Visible;
                submit.Visibility = Visibility.Visible;
                json = File.ReadAllText(fname);

            }
        }

        private void tcombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected_task = (string)tcombo.SelectedItem;

            if (selected_task == "assighnment number..." && addass != null && getcg != null)
            {
                addcourse.IsEnabled = true;
                addcbblock.Visibility = Visibility.Collapsed;
                courseStuff.Visibility = Visibility.Visible;
                remcourse.Visibility = Visibility.Visible;
                cinf.Visibility = Visibility.Visible;
                addass.Visibility = Visibility.Visible;
                getcg.Visibility = Visibility.Visible;
                gcavg.Visibility = Visibility.Visible;
                wrap.Visibility = Visibility.Collapsed;
                getavg.Visibility = Visibility.Collapsed;
                gettg.Visibility = Visibility.Collapsed;
                jpath.Text = "ruls.json file:";
                opath.Text = "output.txt file:";
            }
            else if (addass != null && getcg != null)
            {
                addcbblock.Visibility = Visibility.Collapsed;
                courseStuff.Visibility = Visibility.Collapsed;
                remcourse.Visibility = Visibility.Collapsed;
                addass.Visibility = Visibility.Collapsed;
                cinf.Visibility = Visibility.Collapsed;
                getcg.Visibility = Visibility.Collapsed;
                gcavg.Visibility = Visibility.Collapsed;
                wrap.Visibility = Visibility.Collapsed;
                getavg.Visibility = Visibility.Visible;
                gettg.Visibility = Visibility.Visible;
                jpath.Text = "ruls.json file:";
                opath.Text = "output.txt file:";

            }
        }

        private async void getavg_Click(object sender, RoutedEventArgs e)
        {
            int cid = -1;
            foreach (var c in courses)
            {
                if (c.CourseName == selected_cource)
                {
                    cid = c.CourseNum;
                }
            }

            var avg = await GetTaskAverage(cid, int.Parse(selected_task));
            MessageBox.Show("the task average is: " + avg);
        }

        private async void gcavg_Click(object sender, RoutedEventArgs e)
        {
            int cid = -1;
            foreach (var c in courses)
            {
                if (c.CourseName == selected_cource)
                {
                    cid = c.CourseNum;
                }
            }

            var avg = await GetCourseAverage(cid);
            MessageBox.Show("the cource average is: " + avg);
        }

        private async void gettg_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = selected_cource + "_" + selected_task + "_" + "Grades"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "csv file (.csv)|*.csv"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                int cid = -1;
                foreach (var c in courses)
                {
                    if (c.CourseName == selected_cource)
                    {
                        cid = c.CourseNum;
                    }
                }
                string filename = dlg.FileName;
                var grades = await GetTaskGrades(cid, int.Parse(selected_task));
                string s = $"cuorse:{selected_cource},task:{selected_task}\n" + "student_ID,grade\n";

                foreach (var g in grades)
                {
                    s += g.StudentID + "," + g.grade + "\n";
                }
                File.WriteAllText(filename, s);
                MessageBox.Show($"the file saved in :{filename}");

            }
        }

        private void addcourse_Click(object sender, RoutedEventArgs e)
        {

            addcourse.IsEnabled = false;
            addcbblock.Visibility = Visibility.Visible;

        }

        private async void addc_Click(object sender, RoutedEventArgs e)
        {
            string cont = cnentry.Text;
            cnentry.Text = "";
            addcbblock.Visibility = Visibility.Collapsed;
            addcourse.IsEnabled = true;
            Course course = new Course();
            //kahder add course
            try
            {
                course = await CreateCourseAsync(new Course() { CourseName = cont, TeacherID = user.UserID, CourseNum = courses.Count() + 2000, Description = "" });
                courses.Add(course);
                tcourses.Items.Add(course.CourseName);
            }
            catch
            {
                MessageBox.Show("something went wrong, try again");
            }
 
        }

        private void canceladdc_Click(object sender, RoutedEventArgs e)
        {
            cnentry.Text = "";
            addcbblock.Visibility = Visibility.Collapsed;
            addcourse.IsEnabled = true;
        }

        private async void remcourse_Click(object sender, RoutedEventArgs e)
        {
            string cname = selected_cource;
            int x = courseID;
            tcombo.SelectedIndex = 0;
            tcourses.SelectedIndex = 0;
            //khader remove course
            try
            {
                var course = await DeleteCourseAsync(x);
                courses.Remove(course);
                tcourses.Items.Remove(cname);
                //ComboBox_SelectionChanged(null, null);
            }
            catch 
            {
                MessageBox.Show("something went wrong, try again");
            }
        }

        private void editcoursebtn_Click(object sender, RoutedEventArgs e)
        {
            editcoursebtn.IsEnabled = false;
            courseinfo.IsReadOnly = false;
            saveedit.Visibility = Visibility.Visible;
            canceledit.Visibility = Visibility.Visible;
            Visible.IsEnabled = false;
            remcourse.IsEnabled = false;
            gcavg.IsEnabled = false;
            getcg.IsEnabled = false;
            addass.IsEnabled = false;
            tcourses.IsEnabled = false; ;
            tcombo.IsEnabled = false;
            oldinfo = courseinfo.Text;
        }

        private void canceledit_Click(object sender, RoutedEventArgs e)
        {
            editcoursebtn.IsEnabled = true;
            courseinfo.IsReadOnly = true;
            saveedit.Visibility = Visibility.Collapsed;
            canceledit.Visibility = Visibility.Collapsed;
            Visible.IsEnabled = true;
            remcourse.IsEnabled = true;
            gcavg.IsEnabled = true;
            getcg.IsEnabled = true;
            addass.IsEnabled = true;
            tcourses.IsEnabled = true;
            tcombo.IsEnabled = true;
            courseinfo.Text = oldinfo;
        }

        private async void saveedit_Click(object sender, RoutedEventArgs e)
        {
            oldinfo = courseinfo.Text;
            editcoursebtn.IsEnabled = true;
            courseinfo.IsReadOnly = true;
            saveedit.Visibility = Visibility.Collapsed;
            canceledit.Visibility = Visibility.Collapsed;
            Visible.IsEnabled = true;
            remcourse.IsEnabled = true;
            gcavg.IsEnabled = true;
            getcg.IsEnabled = true;
            addass.IsEnabled = true;
            tcourses.IsEnabled = true;
            tcombo.IsEnabled = true;
            courseinfo.Text = oldinfo;
            foreach (Course c in courses)
            {
                if (c.CourseName == tcourses.SelectedItem)
                {
                    c.Description = oldinfo;
                }
            }
                    //khader save the oldinfo to the database

            try
            {
                await UpdateCourseAsync(new Course() { CourseName = "", CourseNum = -1, TeacherID = -1, Description = oldinfo }, courseID);
            }
            catch
            {
                MessageBox.Show("could not update, try again");
            }

        }

        private void cinf_Click(object sender, RoutedEventArgs e)
        {
            showinf.Visibility = Visibility.Visible;
            infohere.Text = "";
            foreach (Course c in courses)
            {
                if (c.CourseID == courseID)
                {
                    infohere.Text = "Course info:\n";
                    infohere.Text += "Course name: " + c.CourseName + "\n";
                    infohere.Text += "Course number: " + c.CourseNum + "\n";
                    infohere.Text += "Course description: \n\n\t" + c.Description + "\n";

                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            showinf.Visibility = Visibility.Collapsed;
        }
    }
}
