using System.ComponentModel.DataAnnotations;
using Web.Models;

namespace Web.ViewModels
{
    public class PatientDeleteViewModel
    {
        public PatientDeleteViewModel(Patient patient)
        {
            PatientId = patient.PatientId;
            SSN = patient.SSN;
            FirstName = patient.FirstName;
            LastName = patient.LastName;
        }

        public int PatientId { get; set; }
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}