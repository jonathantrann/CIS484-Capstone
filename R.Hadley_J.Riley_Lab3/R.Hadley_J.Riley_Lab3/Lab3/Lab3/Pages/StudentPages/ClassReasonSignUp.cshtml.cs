using Lab3.Pages.DataClasses;
using Lab3.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab3.Pages.StudentPages
{
    public class ClassReasonSignUpModel : PageModel
    {
        [BindProperty] public int SelectedFaculty { get; set; }
        //public List<Queue> QueueList { get; set; }
        public List<Faculty> FacultyList { get; set; }
        [BindProperty] public int currentStudentID { get; set; }
        [BindProperty] public int selectedOfficeHoursID { get; set; }

        [BindProperty] public int selectedFacultyID { get; set; }
        [BindProperty] public string SearchedFaculty { get; set; }

        [BindProperty] public int QueuePosition { get; set; }
        [BindProperty] public string Purpose { get; set; }

        public Queue NewQueue { get; set; }

        public List<Faculty> FacultySearchList;
        public List<SpecificOfficeHours> SpecificOfficeHoursList;
        public List<SpecificClass> ClassList;

        public ClassReasonSignUpModel()
        {
            FacultySearchList =  new List<Faculty>();
            FacultyList = new List<Faculty>();
            SpecificOfficeHoursList = new List<SpecificOfficeHours>();
            ClassList = new List<SpecificClass>();
            NewQueue = new Queue();

        }
        public void OnGet()
        {
            int selectedFacultyID = HttpContext.Session.GetInt32("selectedFacultyID") ?? 0;

            int selectedOfficeHoursID = HttpContext.Session.GetInt32("selectedOfficeHoursID") ?? 0;

            SqlDataReader specificFacultyClassesReader = DBClass.SpecificFacultyClassesReader(selectedFacultyID);
            while(specificFacultyClassesReader.Read())
            {
                ClassList.Add(new SpecificClass
                {
                    FacultyFirst = specificFacultyClassesReader["FacultyFirst"].ToString(),
                    FacultyLast = specificFacultyClassesReader["FacultyLast"].ToString(),
                    ClassName = specificFacultyClassesReader["ClassName"].ToString(),
                    ClassID = Int32.Parse(specificFacultyClassesReader["ClassID"].ToString())
                });
            }
            specificFacultyClassesReader.Close();

        }

        public IActionResult OnPostAddToQueue()
        {
            ModelState.Clear();

            //SqlDataReader SpecificOfficeHoursReader = DBClass.SpecificOfficeHours(selectedFacultyID); int selectedOfficeHoursID = 0;
            //if (SpecificOfficeHoursReader.Read())
            //{
            //    selectedOfficeHoursID = Convert.ToInt32(SpecificOfficeHoursReader["OfficeHoursID"]);

            //}

            //HttpContext.Session.SetInt32("selectedOfficeHoursID", selectedOfficeHoursID);
            //SpecificOfficeHoursReader.Close();


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
                NewQueue.MeetingPurpose = Purpose;
                NewQueue.QueuePosition = QueuePosition;
                DBClass.InsertQueue(NewQueue, selectedOfficeHoursID);


                // Redirect to Queue page
                return RedirectToPage("/StudentPages/StudentHome");
            }

        }
    }
}
