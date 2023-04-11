using Lab3.Pages.DataClasses;
using Lab3.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab3.Pages.Login
{
    public class SelectClassModel : PageModel
    {
        [BindProperty] public int selectedFacultyID { get; set; }

        [BindProperty] public int selectedClassID { get; set; }

        public List<Class> ClassList;

        public FacultySchedule NewFS { get; set; }

        public SelectClassModel()
        {
            ClassList = new List<Class>();

            NewFS = new FacultySchedule();
        }
        public void OnGet()
        {
            // Read all classes from the database
            using (SqlDataReader reader = DBClass.AllClassReader())
            {
                // Read each class and add it to the ClassList
                while (reader.Read())
                {
                    int classID = reader.GetInt32(0);
                    string className = reader.GetString(1);

                    ClassList.Add(new Class { ClassID = classID, ClassName = className });
                }
            }
        }

        public IActionResult OnPost(string button)
        {
            NewFS.FacultyID = selectedFacultyID;
            NewFS.ClassID = selectedClassID;
            DBClass.InsertFacultySchedule(NewFS, selectedClassID, selectedFacultyID);

            DBClass.LabDBConnection.Close();

            return RedirectToPage("FacultyHash");
        }
    }
}
