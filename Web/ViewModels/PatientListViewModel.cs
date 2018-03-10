using System.Linq;
using Web.Models;
using X.PagedList;

namespace Web.ViewModels
{
    public class PatientListViewModel
    {
        public PatientListViewModel(IPagedList<Patient> patients, string search, PatientFilterColumns column)
        {
            Patients = MapPatients(patients);
            Search = search;
            Column = column;
        }

        public IPagedList<PatientListItemViewModel> Patients { get; }

        public PatientFilterColumns Column { get; }

        public string Search { get; }

        private IPagedList<PatientListItemViewModel> MapPatients(IPagedList<Patient> patients)
        {
            var source = patients.Select(patient => new PatientListItemViewModel(patient));
            return new StaticPagedList<PatientListItemViewModel>(source, patients.GetMetaData());
        }
    }

    public enum PatientFilterColumns
    {
        SSN = 0,
        LASTNAME
    }
}