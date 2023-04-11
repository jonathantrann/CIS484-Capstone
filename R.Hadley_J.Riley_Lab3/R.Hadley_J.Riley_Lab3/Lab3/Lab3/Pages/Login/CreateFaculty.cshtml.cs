using Lab3.Pages.DataClasses;
using Lab3.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab3.Pages.Login
{
    public class CreateFacultyModel : PageModel
    {
        [BindProperty] public int selectedClassID { get; set; }

        [BindProperty] public int selectedFacultyID { get; set; }

        [BindProperty] public Faculty? NewFaculty { get; set; }


        public List<Class> ClassList;

        public CreateFacultyModel()
        {
            ClassList = new List<Class>();
        }
        
        public void OnGet()
        {
            
        }

        public IActionResult OnPost(string button)
        {
            int facultyId = DBClass.InsertFaculty(NewFaculty);

            facultyId = selectedFacultyID;

            DBClass.LabDBConnection.Close();

            return RedirectToPage("/Login/SelectClass");
        }

    }
}
