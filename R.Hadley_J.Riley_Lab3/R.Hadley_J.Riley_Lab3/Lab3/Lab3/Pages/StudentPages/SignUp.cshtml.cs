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
        [BindProperty] public string SearchedFaculty { get; set; }

        [BindProperty] public int QueuePosition { get; set; }
        [BindProperty] public string Purpose { get; set; }


        public List<Faculty> FacultySearchList;
        public List<SpecificOfficeHours> SpecificOfficeHoursList;

        
        public SignUpModel()
        {
            FacultySearchList =  new List<Faculty>();
            FacultyList = new List<Faculty>();
            SpecificOfficeHoursList = new List<SpecificOfficeHours>();
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



        public IActionResult OnPostFacultySelect()
        {
            FacultySearchList.Clear();
            SqlDataReader SpecificFacultyReader = DBClass.SearchedFacultyReader(SearchedFaculty);

            int selectedFacultyID = 0;
            if (SpecificFacultyReader.Read())
            {
                // If the reader has data, get the ID of the first faculty record
                selectedFacultyID = Convert.ToInt32(SpecificFacultyReader["FacultyID"]);
            }

            // Set the "selectedFacultyID" session variable to the selected faculty ID
            HttpContext.Session.SetInt32("selectedFacultyID", selectedFacultyID);

            // Populate the FacultySearchList with data from the reader
            SpecificFacultyReader.Close(); // close the reader before using it again
            SpecificFacultyReader = DBClass.SearchedFacultyReader(SearchedFaculty);
            while (SpecificFacultyReader.Read())
            {
                FacultySearchList.Add(new Faculty
                {
                    FacultyFirst = SpecificFacultyReader["FacultyFirst"].ToString(),
                    FacultyLast = SpecificFacultyReader["FacultyLast"].ToString(),
                });
            }

            // Populate the FacultyList with data from the faculty table
            SpecificFacultyReader.Close(); // close the reader before using it again
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

        //public IActionResult OnPostFacultySelect()
        //{
        //    FacultySearchList.Clear();
        //    SqlDataReader SpecificFacultyReader = DBClass.SearchedFacultyReader(SearchedFaculty);

        //    HttpContext.Session.SetInt32("selectedFacultyID", selectedFacultyID);

        //    while (SpecificFacultyReader.Read())
        //    {

        //        FacultySearchList.Add(new Faculty
        //        {
        //            FacultyFirst = SpecificFacultyReader["FacultyFirst"].ToString(),
        //            FacultyLast = SpecificFacultyReader["FacultyLast"].ToString(),

        //        });
        //    }


        //    SqlDataReader FacultyReader = DBClass.FacultyReader();

        //    while (FacultyReader.Read())
        //    {

        //        FacultyList.Add(new Faculty
        //        {
        //            FacultyID = Int32.Parse(FacultyReader["FacultyID"].ToString()),
        //            FacultyFirst = FacultyReader["FacultyFirst"].ToString(),
        //            FacultyLast = FacultyReader["FacultyLast"].ToString()

        //        });

        //    }

        //    return Page();

        //}

        public IActionResult OnPostPage()
        {

            return RedirectToPage("/StudentPages/OfficeHourSignUp");
        }
        
    }

}

