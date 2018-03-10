using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Web.EF;
using Web.Models;
using Web.ViewModels;
using X.PagedList;

namespace Web.Controllers
{
    public class PatientsController : Controller
    {
        private AlwaysEncryptedContext db = new AlwaysEncryptedContext();

        public async Task<ActionResult> Index(string search, PatientFilterColumns column = PatientFilterColumns.SSN, int? page = null)
        {
            int pageSize = 25;
            int pageNumber = (page ?? 1);

            IQueryable<Patient> query = db.Patients;
            
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = ApplyFilter(search, column, query);
            }

            var patients = await query.OrderBy(patient => patient.LastName)
                .ThenBy(patient => patient.FirstName)
                .ToPagedListAsync(pageNumber, pageSize);

            var viewModel = new PatientListViewModel(patients, search, column);
            return View(viewModel);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = await db.Patients.FindAsync(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PatientCreateViewModel patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient.MapToPatient());
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = await db.Patients.FindAsync(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(new PatientEditViewModel(patient));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PatientEditViewModel patient)
        {
            if (ModelState.IsValid)
            {
                var dbPatient = patient.MapToPatient();
                db.Entry(dbPatient).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = await db.Patients.FindAsync(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(new PatientDeleteViewModel(patient));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Patient patient = await db.Patients.FindAsync(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            db.Patients.Remove(patient);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private IQueryable<Patient> ApplyFilter(string search, PatientFilterColumns filterColumn, IQueryable<Patient> query)
        {
            search = search.Trim();
            switch (filterColumn)
            {
                case PatientFilterColumns.LASTNAME:
                    query = query.Where(x => x.LastName.StartsWith(search));
                    break;
                default:
                    query = query.Where(x => x.SSN.Equals(search));
                    break;
            }

            return query;
        }
    }
}
