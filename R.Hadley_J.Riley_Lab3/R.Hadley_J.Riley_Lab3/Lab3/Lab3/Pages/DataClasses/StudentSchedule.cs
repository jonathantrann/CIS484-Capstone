namespace Lab3.Pages.DataClasses
{
    public class StudentSchedule
    {
        public int StudentScheduleID { get; set; }
        public int StudentID { get; set; }

        public int ClassID { get; set; }

        public int TotalCreditHours { get; set; }
    }
}