using Lab3.Pages.DataClasses;
using Lab3.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab3.Pages.StudentPages
{
    public class StudentHomeModel : PageModel
    {
        [BindProperty] public String? SelectedStudent { get; set; }

        public List<SpecificQueue> SpecificQueueList;
        // Create a list for students
        public List<Student> StudentList { get; set; }

        public StudentHomeModel()
        {
            StudentList = new List<Student>();

            SpecificQueueList = new List<SpecificQueue>();
        }


        public IActionResult OnGet()
        {
            var username = HttpContext.Session.GetString("Username");


            if (username == null)
            {
                return new RedirectToPageResult("/Login/StudentLogin");
            }
            else
            {
                // Reads in data from the student table and converts it to strings
                SqlDataReader SpecificQueueReader = DBClass.SpecificQueue(username);

                while (SpecificQueueReader.Read())
                {


                    SpecificQueueList.Add(new SpecificQueue
                    {
                        OfficeHoursDays = (SpecificQueueReader["OfficeHoursDays"].ToString()),
                        OHStartTime = SpecificQueueReader["OHStartTime"].ToString(),
                        OHEndTime = SpecificQueueReader["OHEndTime"].ToString(),
                        FacultyFirst = SpecificQueueReader["FacultyFirst"].ToString(),
                        FacultyLast = SpecificQueueReader["FacultyLast"].ToString(),
                        WaitingRoom = SpecificQueueReader["WaitingRoom"].ToString(),
                        QueuePosition = Int32.Parse(SpecificQueueReader["QueuePosition"].ToString()),

                    });
                }
                DBClass.LabDBConnection.Close();

                return Page();

            }
        }
    }
}

