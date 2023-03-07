using Lab3.Pages.DB;
using Lab3.Pages.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab3.Pages.FacultyPages
{
    public class QueueManagerModel : PageModel
    {
        
        [BindProperty] public int currentFacultyID { get; set; }
        [BindProperty] public int selectedQueueID { get; set; }
        [BindProperty] public int SelectedOfficeHours { get; set; }
        public List<SpecificOfficeHours> SpecificQueueList { get; set; }

        

        public List<SpecificQueue> SelectOfficeHoursList { get; set; }

        public QueueManagerModel()
        {
            //StudentList = new List<Student>();
            SpecificQueueList = new List<SpecificOfficeHours>();
            SelectOfficeHoursList = new List<SpecificQueue>();
        }


        public void OnGet()
        {
            ModelState.Clear();

            string username = HttpContext.Session.GetString("Username");

            SqlDataReader facultyIDReader = DBClass.GetFacultyID(username);
            while (facultyIDReader.Read())
            {
                currentFacultyID = Int32.Parse(facultyIDReader["FacultyID"].ToString());
            }
            facultyIDReader.Close();
            SqlDataReader OfficeHoursReader = DBClass.SpecificOfficeHours(currentFacultyID);

            while (OfficeHoursReader.Read())
            {
                SelectOfficeHoursList.Add(new SpecificQueue
                {
                    OfficeHoursID = Int32.Parse(OfficeHoursReader["OfficeHoursID"].ToString()),
                    OfficeHoursDays = OfficeHoursReader["OfficeHoursDays"].ToString(),
                    OHStartTime = OfficeHoursReader["OHStartTime"].ToString(),
                    OHEndTime = OfficeHoursReader["OHEndTime"].ToString()
                });
                    
                    
            }
        }

        public IActionResult OnPostSingleSelect()
        {
            SpecificQueueList.Clear();
            SelectOfficeHoursList.Clear();

            if (SelectedOfficeHours != null)
            {
                SqlDataReader SpecificOfficeHoursReader = DBClass.OfficeHoursReader(currentFacultyID, SelectedOfficeHours);

                while (SpecificOfficeHoursReader.Read())
                {
                    SpecificQueueList.Add(new SpecificOfficeHours
                    {
                        OfficeHoursID = Int32.Parse(SpecificOfficeHoursReader["OfficeHoursID"].ToString()),
                        OfficeHoursDays = SpecificOfficeHoursReader["OfficeHoursDays"].ToString(),
                        OHStartTime = SpecificOfficeHoursReader["OHStartTime"].ToString(),
                        OHEndTime = SpecificOfficeHoursReader["OHEndTime"].ToString(),
                        WaitingRoom = SpecificOfficeHoursReader["WaitingRoom"].ToString(),
                        FacultyID = Int32.Parse(SpecificOfficeHoursReader["FacultyID"].ToString())
                    });

                }
                SpecificOfficeHoursReader.Close();
            }

            SqlDataReader OfficeHoursReader = DBClass.SpecificOfficeHours(currentFacultyID);

            while (OfficeHoursReader.Read())
            {
                SelectOfficeHoursList.Add(new SpecificQueue
                {
                    OfficeHoursID = Int32.Parse(OfficeHoursReader["OfficeHoursID"].ToString()),
                    OfficeHoursDays = OfficeHoursReader["OfficeHoursDays"].ToString(),
                    OHStartTime = OfficeHoursReader["OHStartTime"].ToString(),
                    OHEndTime = OfficeHoursReader["OHEndTime"].ToString()
                });
            }
            OfficeHoursReader.Close();
            return Page();
        }




    }
}
