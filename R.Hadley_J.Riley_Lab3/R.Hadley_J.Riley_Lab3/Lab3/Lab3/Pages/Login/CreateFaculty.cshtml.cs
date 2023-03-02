using Lab3.Pages.DataClasses;
using Lab3.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab3.Pages.Login
{
    public class CreateFacultyModel : PageModel
    {
        [BindProperty]
        public Faculty? NewFaculty { get; set; }


        public void OnGet()
        {
        }

        public IActionResult OnPost(string button)
        {
            int facultyId = DBClass.InsertFaculty(NewFaculty);

            DBClass.LabDBConnection.Close();

            return RedirectToPage("FacultyHash");
        }

    }
}
