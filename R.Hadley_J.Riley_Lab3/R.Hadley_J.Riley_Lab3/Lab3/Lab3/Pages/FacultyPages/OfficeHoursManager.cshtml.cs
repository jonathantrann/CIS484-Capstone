using Lab3.Pages.DataClasses;
using Lab3.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab3.Pages.FacultyPages
{
    public class Office_Hours_ManagerModel : PageModel
    {
        [BindProperty] public OfficeHours NewOfficeHours { get; set; }
        [BindProperty] public int currentFacultyID { get; set; }
        public void OnGet()
        {
            
        }
        public IActionResult OnPostCreateOfficeHours()
        {
            ModelState.Clear();

            string username = HttpContext.Session.GetString("Username");

            SqlDataReader facultyIDReader = DBClass.GetFacultyID(username);
            while (facultyIDReader.Read())
            {
                currentFacultyID = Int32.Parse(facultyIDReader["FacultyID"].ToString());
            }
            facultyIDReader.Close();

            NewOfficeHours.FacultyID = currentFacultyID;
            DBClass.InsertOfficeHours(NewOfficeHours, currentFacultyID);



            return RedirectToPage("/FacultyPages/FacultyHome");
        }
    }
}
