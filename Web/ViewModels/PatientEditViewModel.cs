using System;
using System.ComponentModel.DataAnnotations;
using Web.Models;

namespace Web.ViewModels
{
    public class PatientEditViewModel
    {
        public PatientEditViewModel()
        {
        }

        public PatientEditViewModel(Patient patient)
        {
            PatientId = patient.PatientId;
            SSN = patient.SSN;
            FirstName = patient.FirstName;
            LastName = patient.LastName;
            StreetAddress = patient.StreetAddress;
            City = patient.City;
            BirthDate = patient.BirthDate;
            State = patient.State;
            MiddleName = patient.MiddleName;
        }

        public int PatientId { get; set; }
        [Required] public string SSN { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string MiddleName { get; set; }
        [Required] public string StreetAddress { get; set; }
        [Required] public string City { get; set; }
        [Required] public string State { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required] public DateTime BirthDate { get; set; }

        public Patient MapToPatient()
        {
            return new Patient()
            {
                PatientId = PatientId,
                LastName = LastName,
                FirstName = FirstName,
                MiddleName = MiddleName,
                SSN = SSN,
                StreetAddress = StreetAddress,
                City = City,
                State = State,
                BirthDate = BirthDate
            };
        }
    }
}