using Lab3.Pages.DataClasses;
using Lab3.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab3.Pages.Login
{
    public class CreateStudentModel : PageModel
    {
        [BindProperty]
        public Student NewStudent { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string button)
        {

            int studentId = DBClass.InsertStudent(NewStudent);

            DBClass.LabDBConnection.Close();

            return RedirectToPage("StudentHash");
        }
    }
}
