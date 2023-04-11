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
        [BindProperty] public int SelectedOfficeHours { get; set; }
        public List<SpecificQueue> SpecificQueueList { get; set; }
        public List<SpecificOfficeHours> SelectOfficeHoursList { get; set; }
        public int currentPosition { get; set; }
        public int newPosition { get; set; }

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
        public IActionResult OnPostNotifyStudent(int queueId, int currentPosition, int newPostion)
        {
            DBClass.NotifyStudent(queueId, currentPosition, newPostion);
            return RedirectToPage("/FacultyPages/QueueManager");
        }

        public void OnPost(string username, DateTime? ohStartTime, DateTime? ohEndTime, string facultyLast)
        {
            var connectionString = "Server=LocalHost;Database=Lab3;Trusted_Connection=True";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                if (!string.IsNullOrEmpty(username))
                {
                    var updateCommand = new SqlCommand("UPDATE Student SET noShow = noShow + 1 WHERE Username = @Username", connection);
                    updateCommand.Parameters.AddWithValue("@Username", username);
                    updateCommand.ExecuteNonQuery();
                }

                var deleteCommand = new SqlCommand("DELETE FROM Queue WHERE QueueID IN(SELECT Queue.QueueID FROM Queue JOIN OfficeHours ON Queue.OfficeHoursID = OfficeHours.OfficeHoursID JOIN Faculty ON OfficeHours.FacultyID = Faculty.FacultyID JOIN Student ON Queue.StudentID = Student.StudentID WHERE(@Username IS NULL OR Student.Username = @Username) AND(@OHStartTime IS NULL OR OfficeHours.OHStartTime = @OHStartTime) AND(@OHEndTime IS NULL OR OfficeHours.OHEndTime = @OHEndTime) AND(@FacultyLast IS NULL OR Faculty.FacultyLast = @FacultyLast));", connection);

                deleteCommand.Parameters.AddWithValue("@Username", (object)username ?? DBNull.Value);
                deleteCommand.Parameters.AddWithValue("@OHStartTime", (object)ohStartTime ?? DBNull.Value);
                deleteCommand.Parameters.AddWithValue("@OHEndTime", (object)ohEndTime ?? DBNull.Value);
                deleteCommand.Parameters.AddWithValue("@FacultyLast", (object)facultyLast ?? DBNull.Value);

                deleteCommand.ExecuteNonQuery();
            }

            TempData["Message"] = "No Show count updated.";
        }
    }
}
