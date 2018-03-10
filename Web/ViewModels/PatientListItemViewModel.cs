using System;
using System.ComponentModel.DataAnnotations;
using Web.Models;

namespace Web.ViewModels
{
    public class PatientListItemViewModel
    {
        public PatientListItemViewModel(Patient patient)
        {
            PatientId = patient.PatientId;
            SSN = patient.SSN;
            FirstName = patient.FirstName;
            LastName = patient.LastName;
            StreetAddress = patient.StreetAddress;
            City = patient.City;
            BirthDate = patient.BirthDate;
        }

        public int PatientId { get; set; }
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
    }
}