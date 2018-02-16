using System;
using System.Collections.Generic;

namespace Web.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime BirthDate { get; set; }

        public List<Visit> Visits { get; set; }
        public List<Staff> AssignedStaff { get; set; }
    }
}