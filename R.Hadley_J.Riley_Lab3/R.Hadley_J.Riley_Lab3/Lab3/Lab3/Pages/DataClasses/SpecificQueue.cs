namespace Lab3.Pages.DataClasses
{
    public class SpecificQueue
    {
        public int OfficeHoursID { get; set; }

        public String? OfficeHoursDays { get; set; }

        public String? OHStartTime { get; set; } //Military Time

        public String? OHEndTime { get; set; } //Military Time

        public String? WaitingRoom { get; set; }

        public bool? Ready { get; set; }

        public int FacultyID { get; set; }

        public String? FacultyFirst { get; set; }

        public String? FacultyLast { get; set; }

        public String? FacultyEmailAddress { get; set; }

        public String? FacultyPhoneNumber { get; set; }

        public String? OfficeLocation { get; set; }

        public int QueuePosition { get; set; }

        public int QueueID { get; set; }

        public String? StudentFirst { get; set; }

        public String? StudentLast { get; set; }

        public int StudentID { get; set; }

        public String? StudentEmailAddress { get; set; }

        public String? StudentPhoneNumber { get; set; }

        public int StudentPartnerID { get; set; }

        public int CredentialsID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

    }
}
