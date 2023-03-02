namespace Lab3.Pages.DataClasses
{
    public class SpecificOfficeHours
    {
        public int FacultyID { get; set; }

        public String? FacultyFirst { get; set; }

        public String? FacultyLast { get; set; }

        public String? EmailAddress { get; set; }

        public String? PhoneNumber { get; set; }

        public String? OfficeLocation { get; set; }

        public int OfficeHoursID { get; set; }

        public String? OfficeHoursDays { get; set; }

        public String? OHStartTime { get; set; } //Military Time

        public String? OHEndTime { get; set; } //Military Time

        public String? WaitingRoom { get; set; }

        public int QueueID { get; set; }

        public String? StudentFirst { get; set; }

        public String? StudentLast { get; set; }

        public int StudentID { get; set; }

    }
}