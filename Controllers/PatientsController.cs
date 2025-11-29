using Hospital_Project.Data;
using Hospital_Project.Entities;
using Hospital_Project.Entities.DTOs;
using Hospital_Project.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace Hospital_Project.Controllers;

        public class PatientsController : Controller
        {
            private readonly hospitaldbcontext dbcontext;

    public UserManager<UserInfo> USER { get; }

    public PatientsController(hospitaldbcontext Dbcontext,UserManager<UserInfo>uSER)
            {
                dbcontext = Dbcontext;
        USER = uSER;
    }
    [Authorize(Roles = "Admin,Employee,Nurse,Doctor,Patient")]
    [HttpGet]
    public async Task<IActionResult> ShowPatients(string searchengine = "")
    {
        var user = await USER.GetUserAsync(User);
        var roles = await USER.GetRolesAsync(user);
        var isAdminOrEmployee = roles.Contains("Admin") || roles.Contains("Employee");

        var query = dbcontext.patients
            .Include(p => p.Appointments)
            .AsQueryable();

        if (!isAdminOrEmployee)
        {
            query = query.Where(p => p.UserId == user.Id);
        }

        if (!string.IsNullOrWhiteSpace(searchengine))
        {
            var search = searchengine.Trim();
            query = query.Where(p => p.Phone.Contains(search));
        }

        var result = await query.ToListAsync();
        return View(result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Employee,Nurse,Doctor,Patient")]

    public IActionResult AddPatient()
    {
        return View();
    }
    [HttpPost]
            public async Task<IActionResult> AddPatient(patientDto Patient)
            {
                if (ModelState.IsValid)
                {
            var user = await USER.GetUserAsync(User);

            Patients patients = new()
                    {
                        Name = Patient.Name,
                        Age = Patient.Age,
                        Gender = Patient.Gender,
                        Phone = Patient.Phone,
                        UserId=user.Id
                    };
                    await dbcontext.patients.AddAsync(patients);
                    await dbcontext.SaveChangesAsync();
                    TempData["Success"] = "تمت إضافة المريض بنجاح";
                    return RedirectToAction(nameof(ShowPatients));
                }
        return View(Patient);
            }
    [HttpGet]
    [Authorize(Roles = "Admin,Employee,Nurse,Doctor,Patient")]
    public IActionResult UpdatePatient(int id)
    {
        var patient = dbcontext.patients.FirstOrDefault(p => p.Id == id);
        if (patient == null)
        {
            return NotFound();
        }
        patientDto dto = new()
        {
            Name=patient.Name,
            Age=patient.Age,
            Gender=patient.Gender,
            Phone=patient.Phone,

        };


        return View(dto);
    }
    [HttpPost]
            public async Task<IActionResult> UpdatePatient(patientDto patients, int id)
            {
                if (ModelState.IsValid)
                {
                    var patient = dbcontext.patients.FirstOrDefault(x => x.Id == id);
                    if (patient == null)
                    {
                        return NoContent();
                    }
                    patient.Name = patients.Name;
                    patient.Age = patients.Age;
                    patient.Gender = patients.Gender;
                    patient.Phone = patients.Phone;
                    patient.Appointments = patients.Appointments;
                    dbcontext.patients.Update(patient);
                    await dbcontext.SaveChangesAsync();
        TempData["Success"] = "تمت عملية التعديل بنجاح";
                    return RedirectToAction(nameof(ShowPatients));
                }

        return View(patients);
            }
    [HttpGet]
    [Authorize(Roles = "Admin,Employee,Patient,Nurse,Doctor")]
    public IActionResult DeletePatient(patientDto dto,int id)
    {

        var patient = dbcontext.patients.FirstOrDefault(p => p.Id == id);
        if (patient == null)
        {
            return NotFound();
        }
         dto = new()
        {
            Name = patient.Name,
            Age = patient.Age,
            Gender = patient.Gender,
            Phone = patient.Phone,

        };

        return View(dto);
       
    }
    [HttpPost]

    public async Task<IActionResult> DeletePatient(int id)
            {
                var patient = dbcontext.patients.FirstOrDefault(x => x.Id == id);
                if (patient == null)
                {
                    return NoContent();
                }
                dbcontext.patients.Remove(patient);
                await dbcontext.SaveChangesAsync();
        TempData["Success"] = "تمت عملية الحذف بنجاح";
        return RedirectToAction(nameof(ShowPatients));
            }
        }
 