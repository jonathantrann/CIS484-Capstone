using Lab3.Pages.DataClasses;
using Lab3.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab3.Pages.StudentPages
{
    public class ProfileModel : PageModel
    {
        [BindProperty] public string Username { get; set; }
        public List<User> UserList { get; set; }

        public ProfileModel()
        {
            UserList = new List<User>();
        }

        public void OnGet(string username)
        {
            SqlDataReader reader = DBClass.GetStudentInfo(username);

            while (reader.Read())
            {
                UserList.Add(new User
                {
                    StudentFirst = reader["StudentFirst"].ToString(),
                    StudentLast = reader["StudentLast"].ToString(),
                    StudentEmailAddress = reader["StudentEmailAddress"].ToString(),
                    StudentPhoneNumber = reader["StudentPhoneNumber"].ToString(),

                });
            }

            DBClass.LabDBConnection.Close();
        }
    }
}
