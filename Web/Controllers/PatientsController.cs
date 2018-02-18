using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Web.EF;
using Web.Models;
using X.PagedList;

namespace Web.Controllers
{
    public class PatientsController : Controller
    {
        private AlwaysEncryptedContext db = new AlwaysEncryptedContext();

        public async Task<ActionResult> Index(string search, int? page = null)
        {
            int pageSize = 25;
            int pageNumber = (page ?? 1);
            ViewBag.CurrentFilter = search;

            IQueryable<Patient> query = db.Patients;
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.LastName.StartsWith(search));
            }

            return View(await query.OrderBy(patient => patient.LastName)
                .ThenBy(patient => patient.FirstName)
                .ToPagedListAsync(pageNumber, pageSize));
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
        public async Task<ActionResult> Create([Bind(Include = "PatientId,SSN,FirstName,LastName,MiddleName,StreetAddress,City,State,BirthDate")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
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
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PatientId,SSN,FirstName,LastName,MiddleName,StreetAddress,City,State,BirthDate")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
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
            return View(patient);
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
    }
}
