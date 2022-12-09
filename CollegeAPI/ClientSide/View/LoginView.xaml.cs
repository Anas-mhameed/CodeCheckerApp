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

using System.Net;
using System.Net.Http;
using CollegeAPI.Models;

namespace ClientSide.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {


        static HttpClient client;

        public LoginView()
        {
            try
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7286/swagger/");
                
            }
            catch
            {
                MessageBox.Show("connection faild");
            }


            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // New function => getting user from api (checking)
        static async Task<User> GetUser(String uname, String pw)
        {
            User user ;
            HttpResponseMessage response;
            
            try
            {
                response = await client.GetAsync($"/api/College/user/{uname}/{pw}");

            }
            catch
            {
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                try {
                    user = await response.Content.ReadAsAsync<User>();
                }
                catch
                {
                    user = null;
                }
                
                return user;
            }

            return null;
            

        }

        

        //New function => getting course students from api
        static async Task<List<User>> GetCoursetudents(int courseNum)
        {
            List<User> users;
            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync($"/api/College/Users/{courseNum}");
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
                    users = await response.Content.ReadAsAsync<List<User>>();

                    return users;

                }
                catch
                {
                    MessageBox.Show("failed to get the students");
                }

            }

            return null;
        }

        // 
        

        // New function => getting user courses from api 
        static async Task<List<Course>> GetUserCourses(int userId, string role)
        {
            List<Course> courses;
            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync($"/api/College/{role}Courses/{userId}");
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
                    courses = await response.Content.ReadAsAsync<List<Course>>();

                    return courses;

                }
                catch
                {
                    MessageBox.Show("failed to get the courses");
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

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            string un = txtUser.Text;
            string pw = txtPass.Password;

            //********************************************
            User user = null;
            user = await GetUser(un, pw);
            if (user == null)
            {   
                txtPass.Password = "";
                txtUser.Text = "";
                err.Text = "wrong user name or password";
                return;
            }

            //*********************************************

            List<Course> courses = await GetUserCourses(user.UserID, user.role.ToString());
            

            // succes
            if (user.role == User.Role.Teacher)
            {
                
                var newForm = new Deliver(courses,user); //create your new form.
                newForm.Show(); //show the new form.
                this.Close(); //only if you want to close the current form.

            }
            else 
            {

                var newForm = new afterLogin(courses,user); //create your new form.
                newForm.Show(); //show the new form.
                this.Close(); //only if you want to close the current form.

            }

           


        }
    }
}
