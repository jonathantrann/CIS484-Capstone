namespace Lab3.Pages.DataClasses
{
    public class Meeting
    {
        public int MeetingID { get; set; }

        public String? MeetingLocation { get; set; }

        public String? MeetingDate { get; set; } //Month Day, Year

        public String? MeetStartTime { get; set; } //Military Time

        public String? MeetEndTime { get; set; } //Military Time

        public String? MeetingPurpose { get; set; }

        public int StudentID { get; set; }

        public int FacultyID { get; set; }
    }
}