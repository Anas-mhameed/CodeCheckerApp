using CollegeAPI.Models;
using CheckingFiles.MyClass;

using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json.Linq;
using Task = CollegeAPI.Models.Task;

namespace ClientSide.View
{
        /// <summary>
        /// Interaction logic for Window1.xaml
        /// </summary>
        public partial class afterLogin : Window
        {
        static HttpClient client;
        static List<Course> courses ;
        static User user;
        int coursNum ;
        int taskNum;

        public afterLogin(List<Course> cc, User u)
        {   
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
            courcecombo.Items.Add("courses...");
            foreach (Course c in courses)
                courcecombo.Items.Add(c.CourseName);
            courcecombo.SelectedIndex = 0;
                
        }

        // New function => getting course tasks from api (checking
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

        // 
        static async Task<Grade> GetGrade(int courseNum, int Id, int taskNum)
        {
            Grade grade;
            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync($"/api/College/Grade/{courseNum}/{Id}/{taskNum}");
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
                    grade = await response.Content.ReadAsAsync<Grade>();

                    return grade;

                }
                catch
                {
                    MessageBox.Show("failed to get the students");
                }

            }

            return null;
        }

        static async Task<Uri> InsertGradeAsync(Grade grade)
        {

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/College/Grade", grade);
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

            private void btnOpenFile_Click(object sender, RoutedEventArgs e)
            {

                OpenFileDialog openFileDialog = new OpenFileDialog();
                string fname, filecontent;

                if (openFileDialog.ShowDialog() == true)
                {
                    fname = openFileDialog.FileName;
                    fpath.Text = fname;
                    filecontent = File.ReadAllText(openFileDialog.FileName);
                    cancel.Visibility = Visibility.Visible;
                    submit.Visibility = Visibility.Visible;

                }


            }
            private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                string selectedText = (string)courcecombo.SelectedItem;
                List<CollegeAPI.Models.Task> tasks;

                
            assignmentcombo.Items.Clear();
            if (selectedText != null && selectedText == "courses..." && assignmentcombo != null)
            {
                assignmentcombo.Visibility = Visibility.Collapsed;
                btnfile.Visibility = Visibility.Collapsed;

            }
            else if (assignmentcombo != null)
            {
                assignmentcombo.Visibility = Visibility.Visible;
                
                foreach (Course c in courses)
                {
                    if (c.CourseName == courcecombo.SelectedItem)
                        coursNum = c.CourseNum;
                }
                tasks = await GetTasks(coursNum);
                assignmentcombo.Items.Add("assighnment number...");
                foreach (CollegeAPI.Models.Task t in tasks)
                {
                    assignmentcombo.Items.Add(t.TaskNum.ToString()); 
                }
                assignmentcombo.SelectedIndex = 0;

            }

            }
            private async void ComboBox_SelectionChanged2(object sender, SelectionChangedEventArgs e)
            {
                
                string selectedText = (string)assignmentcombo.SelectedItem;
            Grade grade;

                if (selectedText != null && selectedText == "assighnment number..." && btnfile != null)
                {
                    btnfile.Visibility = Visibility.Collapsed;
                    cancel.Visibility = Visibility.Collapsed;
                    submit.Visibility = Visibility.Collapsed;
                }
                else if (btnfile != null && assignmentcombo.SelectedItem!=null)
                {
                    btnfile.Visibility = Visibility.Visible;

                    grade = await GetGrade(coursNum,user.UserID,int.Parse((string)assignmentcombo.SelectedItem)); 
                    if(grade != null)
                    {
                        fpath.Text = "your grade is: " + grade.grade;
                        btnOpenFile.Visibility = Visibility.Collapsed;
                        sfh.Visibility = Visibility.Collapsed;    
                    }
                    else
                    {
                        taskNum = int.Parse((string)assignmentcombo.SelectedItem);
                        fpath.Text = "";
                        btnOpenFile.Visibility = Visibility.Visible;
                        sfh.Visibility = Visibility.Visible;
                    }
                }
            }
            private void logout_Click(object sender, RoutedEventArgs e)
            {
                var newForm = new LoginView(); //create your new form.
                newForm.Show(); //show the new form.
                this.Close(); //only if you want to close the current form.

            }

            private void cancel_Click(object sender, RoutedEventArgs e)
            {
                fpath.Text = "";
                cancel.Visibility = Visibility.Collapsed;
                submit.Visibility = Visibility.Collapsed;
            }
        private async void submit_Click(object sender, RoutedEventArgs e)
        {

            var result = Checker.Option2(fpath.Text);
            string  outp = result[0];
            string err = result[1];
            List<Task> task = await GetTasks(coursNum);
            var taskOut = task.Find(x => x.TaskNum == taskNum).RoleFile;
            var taskJson = task.Find(x => x.TaskNum == taskNum).JsonFile;
            var outarr = taskOut.Split('\n');
            JObject json = JObject.Parse(taskJson);
            
            int i = 0;
            int g = 100;
            foreach(var item in json)
            {
                if (outp.Contains(outarr[i]))
                {

                    continue;
                }
                else
                { 
                    g -= ((int)item.Value);
                }

                i++;
            }

            Grade grade = new Grade()
            {
                StudentID = user.UserID,
                CourseNum = coursNum,
                TaskNum = taskNum,
                grade = g,
            };
            try
            {
                var res = await InsertGradeAsync(grade);
            }
            catch
            {
                MessageBox.Show("Somthing Went Wrong, try again");
            }
            assignmentcombo.SelectedIndex = 0;
            assignmentcombo.SelectedIndex = taskNum;
        }
    }
}
