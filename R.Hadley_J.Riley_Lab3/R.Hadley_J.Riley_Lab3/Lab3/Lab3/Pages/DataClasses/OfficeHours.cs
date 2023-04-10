namespace Lab3.Pages.DataClasses
{
    public class OfficeHours
    {
        public int OfficeHoursID { get; set; }

        public String? OfficeHoursDays { get; set; }

        public String? OHStartTime { get; set; } //Military Time

        public String? OHEndTime { get; set; } //Military Time

        public String? WaitingRoom { get; set; }

        public int FacultyID { get; set; }

        public bool IsRecurring { get; set; }
    }
}
