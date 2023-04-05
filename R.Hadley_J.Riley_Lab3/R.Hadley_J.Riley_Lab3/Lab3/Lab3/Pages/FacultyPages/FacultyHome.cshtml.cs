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

        public List<SpecificOfficeHours> SpecificOfficeHoursList;
        public List<OfficeHours> OfficeHoursList;

        [BindProperty] public int selectedOfficeHoursID { get; set; }
        [BindProperty] public int currentFacultyID { get; set; }
        public Queue NewQueue { get; set; }
        public FacultyHomeModel()
        {
            FacultyList = new List<Faculty>();
            OfficeHoursList = new List<OfficeHours>();
            NewQueue = new Queue();
        }


        public void OnGet()
        {
            ModelState.Clear();
            string username = HttpContext.Session.GetString("Username");

            SqlDataReader facultyIDReader = DBClass.GetFacultyID(username);
            while(facultyIDReader.Read())
            {
                currentFacultyID = Int32.Parse(facultyIDReader["FacultyID"].ToString());
            }
            facultyIDReader.Close();

            SqlDataReader OfficeHoursReader = DBClass.SpecificOfficeHours(currentFacultyID);

            while (OfficeHoursReader.Read())
            {
                OfficeHoursList.Add(new OfficeHours
                {
                    OfficeHoursID = Int32.Parse(OfficeHoursReader["OfficeHoursID"].ToString()),
                    OfficeHoursDays = OfficeHoursReader["OfficeHoursDays"].ToString(),
                    OHStartTime = OfficeHoursReader["OHStartTime"].ToString(),
                    OHEndTime = OfficeHoursReader["OHEndTime"].ToString(),
                    WaitingRoom = OfficeHoursReader["WaitingRoom"].ToString(),

                });
            }
        }

        public IActionResult OnPostSelectOfficeHours()
        {
            OfficeHoursList.Clear();

            string username = HttpContext.Session.GetString("Username");

            SqlDataReader facultyIDReader = DBClass.GetFacultyID(username);
            while (facultyIDReader.Read())
            {
                currentFacultyID = Int32.Parse(facultyIDReader["FacultyID"].ToString());
            }
            facultyIDReader.Close();

            SqlDataReader OfficeHoursReader = DBClass.OfficeHoursReader(currentFacultyID, selectedOfficeHoursID);
            while(OfficeHoursReader.Read())
            {
                OfficeHoursList.Add(new OfficeHours
                {
                    OfficeHoursID = Int32.Parse(OfficeHoursReader["OfficeHoursID"].ToString()),
                    OfficeHoursDays = OfficeHoursReader["OfficeHoursDays"].ToString(),
                    OHStartTime = OfficeHoursReader["OHStartTime"].ToString(),
                    OHEndTime = OfficeHoursReader["OHEndTime"].ToString(),
                    WaitingRoom = OfficeHoursReader["WaitingRoom"].ToString(),
                    FacultyID = Int32.Parse(OfficeHoursReader["FacultyID"].ToString())
                });
            }
            return Page();
        }

    }

}
