using Lab3.Pages.DataClasses;
using Lab3.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab3.Pages.StudentPages
{
    public class SignUpModel : PageModel
    {
        // Create new list for student and queue
        // public List<Student> StudentList { get; set; }
        [BindProperty] public int SelectedFaculty { get; set; }
        //public List<Queue> QueueList { get; set; }
        public List<Faculty> FacultyList { get; set; }
        [BindProperty] public int currentStudentID { get; set; }
        [BindProperty] public int selectedOfficeHoursID { get; set; }

        [BindProperty] public int selectedFacultyID { get; set; }

        public Queue NewQueue { get; set; }


        public List<SpecificOfficeHours> SpecificOfficeHoursList;

        public SignUpModel()
        {
            
            FacultyList = new List<Faculty>();
            SpecificOfficeHoursList = new List<SpecificOfficeHours>();
            NewQueue = new Queue();

        }

        public void OnGet()
        {


            SqlDataReader FacultyReader = DBClass.FacultyReader();

            while (FacultyReader.Read())
            {

                FacultyList.Add(new Faculty
                {
                    FacultyID = Int32.Parse(FacultyReader["FacultyID"].ToString()),
                    FacultyFirst = FacultyReader["FacultyFirst"].ToString(),
                    FacultyLast = FacultyReader["FacultyLast"].ToString()

                });

            }

        }
        public IActionResult OnPostSingleSelect()
        {
            SpecificOfficeHoursList.Clear();
            SqlDataReader SpecificOfficeHoursReader = DBClass.SpecificOfficeHours(SelectedFaculty);

            while (SpecificOfficeHoursReader.Read())
            {
                SpecificOfficeHoursList.Add(new SpecificOfficeHours
                {
                    OfficeHoursID = Int32.Parse(SpecificOfficeHoursReader["OfficeHoursID"].ToString()),
                    FacultyFirst = SpecificOfficeHoursReader["FacultyFirst"].ToString(),
                    FacultyLast = SpecificOfficeHoursReader["FacultyLast"].ToString(),
                    OfficeHoursDays = SpecificOfficeHoursReader["OfficeHoursDays"].ToString(),
                    OHStartTime = SpecificOfficeHoursReader["OHStartTime"].ToString(),
                    OHEndTime = SpecificOfficeHoursReader["OHEndTime"].ToString(),
                    OfficeLocation = SpecificOfficeHoursReader["OfficeLocation"].ToString()
                });
            }


            SqlDataReader FacultyReader = DBClass.FacultyReader();

            while (FacultyReader.Read())
            {

                FacultyList.Add(new Faculty
                {
                    FacultyID = Int32.Parse(FacultyReader["FacultyID"].ToString()),
                    FacultyFirst = FacultyReader["FacultyFirst"].ToString(),
                    FacultyLast = FacultyReader["FacultyLast"].ToString()

                });

            }

            return Page();

        }
        public IActionResult OnPostAddToQueue()
        {
            ModelState.Clear();  
           
                // Get student's email from session data
                string username = HttpContext.Session.GetString("Username");

                // Get studentID based on email
                SqlDataReader studentIDReader = DBClass.GetStudentID(username);
                while (studentIDReader.Read())
                {
                    currentStudentID = Int32.Parse(studentIDReader["StudentID"].ToString());
                }   
                studentIDReader.Close();

                if (DBClass.StudentQueueExists(currentStudentID, selectedOfficeHoursID) == true)
                {
                    ModelState.AddModelError(string.Empty, "You have already signed up for these office hours. Please click confirm again to continue.");
                    return Page();
                }
                else
                {

                // Add student to queue
                NewQueue.StudentID = currentStudentID;
                NewQueue.OfficeHoursID = selectedOfficeHoursID;
                DBClass.InsertQueue(NewQueue, selectedOfficeHoursID);


                // Redirect to Queue page
                return RedirectToPage("/StudentPages/StudentHome");
                }
            
        }
    }

}

