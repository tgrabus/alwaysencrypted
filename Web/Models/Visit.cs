using System;

namespace Web.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string Treatment { get; set; }
        public DateTime? FollowUpDate { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}