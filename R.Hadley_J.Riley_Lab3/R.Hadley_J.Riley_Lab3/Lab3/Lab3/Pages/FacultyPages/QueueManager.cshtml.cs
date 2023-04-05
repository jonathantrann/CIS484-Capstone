using Lab3.Pages.DB;
using Lab3.Pages.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Lab3.Pages.FacultyPages
{
    public class QueueManagerModel : PageModel
    {
        
        [BindProperty] public int currentFacultyID { get; set; }
        [BindProperty] public int SelectedOfficeHours { get; set; }
        public List<SpecificQueue> SpecificQueueList { get; set; }
        public List<SpecificOfficeHours> SelectOfficeHoursList { get; set; }

        public QueueManagerModel()
        {
            SpecificQueueList = new List<SpecificQueue>();
            SelectOfficeHoursList = new List<SpecificOfficeHours>();
        }


        public void OnGet()
        {

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
                SelectOfficeHoursList.Add(new SpecificOfficeHours
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
                SelectOfficeHoursList.Add(new SpecificOfficeHours
                {
                    OfficeHoursID = Int32.Parse(OfficeHoursReader["OfficeHoursID"].ToString()),
                    OfficeHoursDays = OfficeHoursReader["OfficeHoursDays"].ToString(),
                    OHStartTime = OfficeHoursReader["OHStartTime"].ToString(),
                    OHEndTime = OfficeHoursReader["OHEndTime"].ToString()
                });
            }

            if (SelectedOfficeHours != null)
            {
                SqlDataReader SpecificOfficeHoursReader = DBClass.StudentsInSpecificQueue(currentFacultyID, SelectedOfficeHours);

                while (SpecificOfficeHoursReader.Read())
                {
                    SpecificQueueList.Add(new SpecificQueue
                    {
                        QueueID = Int32.Parse(SpecificOfficeHoursReader["QueueID"].ToString()),
                        OfficeHoursID = Int32.Parse(SpecificOfficeHoursReader["OfficeHoursID"].ToString()),
                        OfficeHoursDays = SpecificOfficeHoursReader["OfficeHoursDays"].ToString(),
                        OHStartTime = SpecificOfficeHoursReader["OHStartTime"].ToString(),
                        OHEndTime = SpecificOfficeHoursReader["OHEndTime"].ToString(),
                        WaitingRoom = SpecificOfficeHoursReader["WaitingRoom"].ToString(),
                        FacultyFirst = SpecificOfficeHoursReader["FacultyFirst"].ToString(),
                        FacultyLast= SpecificOfficeHoursReader["FacultyLast"].ToString(),
                        StudentFirst = SpecificOfficeHoursReader["StudentFirst"].ToString(),
                        StudentLast = SpecificOfficeHoursReader["StudentLast"].ToString()
                    });

                }
                SpecificOfficeHoursReader.Close();
            }
            return Page();
        }
        public IActionResult OnPostNotifyStudent(int queueId)
        {
            Debug.WriteLine(queueId);
            DBClass.NotifyStudent(queueId);
            return Page();
        }
    }
}
