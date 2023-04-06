using Lab3.Pages.DataClasses;
using Lab3.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab3.Pages.StudentPages
{
    public class OfficeHourSignUpModel : PageModel
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

        public List<Faculty> FacultySearchList;
        public List<SpecificOfficeHours> SpecificOfficeHoursList;

        public OfficeHourSignUpModel()
        {
            FacultySearchList =  new List<Faculty>();
            FacultyList = new List<Faculty>();
            SpecificOfficeHoursList = new List<SpecificOfficeHours>();
        }
        public void OnGet()
        {
            int selectedFacultyID = HttpContext.Session.GetInt32("selectedFacultyID") ?? 0;

            SqlDataReader SpecificOfficeHoursReader = DBClass.SpecificOfficeHours(selectedFacultyID);

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
        }

        public IActionResult OnPostSingleSelect()
        {
            SqlDataReader SpecificOfficeHoursReader = DBClass.SpecificOfficeHours(selectedFacultyID); int selectedOfficeHoursID = 0;
            if(SpecificOfficeHoursReader.Read())
            {
                selectedOfficeHoursID = Convert.ToInt32(SpecificOfficeHoursReader["OfficeHoursID"]);

            }

            HttpContext.Session.SetInt32("selectedOfficeHoursID", selectedOfficeHoursID);
            SpecificOfficeHoursReader.Close();

            return RedirectToPage("/StudentPages/ClassReasonSignUp");

        }

    }
}
