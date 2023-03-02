using Lab3.Pages.DataClasses;
using Lab3.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab3.Pages.FacultyPages
{
    public class FacultyHomeModel : PageModel
    {
        // Create new list for faculty
        public List<Faculty> FacultyList { get; set; }

        public FacultyHomeModel()
        {
            FacultyList = new List<Faculty>();
        }


        public void OnGet()
        {
            // Reads in the data from the faculty table
            SqlDataReader facultyReader = DBClass.FacultyReader();

            while (facultyReader.Read())
            {
                FacultyList.Add(new Faculty
                {
                    FacultyID = Int32.Parse(facultyReader["FacultyID"].ToString()),
                    FacultyFirst = facultyReader["FacultyFirst"].ToString(),
                    FacultyLast = facultyReader["FacultyLast"].ToString(),
                    FacultyEmailAddress = facultyReader["FacultyEmailAddress"].ToString(),
                    FacultyPhoneNumber = facultyReader["FacultyPhoneNumber"].ToString(),
                    OfficeLocation = facultyReader["OfficeLocation"].ToString(),

                });
            }
            DBClass.LabDBConnection.Close();
        }
    }

}
