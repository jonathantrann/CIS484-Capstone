using Lab3.Pages.DB;
using Lab3.Pages.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab3.Pages.FacultyPages
{
    public class QueueManagerModel : PageModel
    {
        // Create new list for student and queue
        // public List<Student> StudentList { get; set; }
        [BindProperty] public int selectedOfficeHoursID { get; set; }
        [BindProperty] public int currentFacultyID { get; set; }
        [BindProperty] public int selectedQueueID { get; set; }
        public List<SpecificQueue> SpecificQueueList { get; set; }


        public QueueManagerModel()
        {
            //StudentList = new List<Student>();
            SpecificQueueList = new List<SpecificQueue>();
        }


        public void OnGet()
        {
            // Clear the list before filling it up
            SpecificQueueList.Clear();

            // Call the AdminQueue method in DBClass to get the SqlDataReader
            SqlDataReader adminQueueReader = DBClass.AdminQueue();

            // Loop through each row in the SqlDataReader
            while (adminQueueReader.Read())
            {
                // Create a new SpecificQueue object and fill its properties with the data from the SqlDataReader
                SpecificQueue specificQueue = new SpecificQueue();
                specificQueue.StudentFirst = adminQueueReader["StudentFirst"].ToString();
                specificQueue.StudentLast = adminQueueReader["StudentLast"].ToString();
                specificQueue.OfficeHoursDays = adminQueueReader["OfficeHoursDays"].ToString();
                specificQueue.OHStartTime = adminQueueReader["OHStartTime"].ToString();
                specificQueue.OHEndTime = adminQueueReader["OHEndTime"].ToString();
                specificQueue.FacultyFirst = adminQueueReader["FacultyFirst"].ToString();
                specificQueue.FacultyLast = adminQueueReader["FacultyLast"].ToString();
                specificQueue.WaitingRoom = adminQueueReader["WaitingRoom"].ToString();

                // Add the SpecificQueue object to the list
                SpecificQueueList.Add(specificQueue);
            }

            // Close the SqlDataReader and SqlConnection
            adminQueueReader.Close();
            DBClass.LabDBConnection.Close();

            // Set the count of items in the list
            int count = SpecificQueueList.Count;
        }



    }
}
