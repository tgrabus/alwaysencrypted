using System;
using System.ComponentModel.DataAnnotations;
using Web.Models;

namespace Web.ViewModels
{
    public class PatientCreateViewModel
    {
        [Required] public string SSN { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        public string MiddleName { get; set; }
        [Required] public string StreetAddress { get; set; }
        [Required] public string City { get; set; }
        [Required] public string State { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required] public DateTime BirthDate { get; set; }

        public Patient MapToPatient()
        {
            return new Patient()
            {
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