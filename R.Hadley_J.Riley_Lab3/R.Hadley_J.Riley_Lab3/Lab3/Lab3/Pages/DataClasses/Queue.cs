namespace Lab3.Pages.DataClasses
{
    public class Queue
    {

        public int QueueID { get; set; }

        public String? MeetingPurpose { get; set; }

        public int? QueuePosition { get; set; 
        }
        public int StudentID { get; set; }

        public bool ready { get; set; }

        public int OfficeHoursID { get; set; }
    }
}