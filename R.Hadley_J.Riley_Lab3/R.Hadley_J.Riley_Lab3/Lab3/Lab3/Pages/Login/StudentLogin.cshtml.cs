using Lab3.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab3.Pages.Login
{
    public class StudentLoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (DBClass.HashedParameterLogin(Username, Password))
            {
                HttpContext.Session.SetString("Username", Username);
                ViewData["LoginMessage"] = "Login Successful!";
                DBClass.AuthDBConnection.Close();

                return RedirectToPage("/StudentPages/StudentHome");

            }
            else
            {
                ViewData["LoginMessage"] = "Username and/or Password Incorrect";
                DBClass.AuthDBConnection.Close();

                return Page();

            }



        }
    }
}
