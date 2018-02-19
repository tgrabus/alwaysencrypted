using Web.Models;
using X.PagedList;

namespace Web.ViewModels
{
    public class PatientListViewModel
    {
        public PatientListViewModel(IPagedList<Patient> patients, string search, PatientFilterColumns column)
        {
            Patients = patients;
            Search = search;
            Column = column;
        }

        public IPagedList<Patient> Patients { get; }

        public PatientFilterColumns Column { get; }

        public string Search { get; }
    }
}