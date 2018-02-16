using System.Collections.Generic;

namespace Web.Models
{
    public class Staff
    {
        public int StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }

        public List<Patient> Patients { get; set; }
    }
}