using Hospital_Project.Data;
using Hospital_Project.Entities;
using Hospital_Project.Entities.DTOs;
using Hospital_Project.Entities.Enums;
using Hospital_Project.Entities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Hospital_Project.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly hospitaldbcontext dbcontext;

        public DoctorsController(hospitaldbcontext Dbcontext)
        {
            dbcontext = Dbcontext;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,Employee,Nurse")]
        public async Task<IActionResult> GetDoctors(string searchengine = "")
        {
            if (string.IsNullOrEmpty(searchengine))
            {
                var doctors = await dbcontext.doctors
                    .Include(ad => ad.AvailableDate)
                    .Include(d => d.Department)
                    .Include(d => d.Nurses)
                        .ThenInclude(n => n.Nurse)
                    .ToListAsync();

                return View(doctors);
            }
            else
            {
                searchengine = searchengine.Trim();

                var doctors = await dbcontext.doctors
                    .Include(ad => ad.AvailableDate)
                    .Include(d => d.Department)
                    .Where(d => d.Name.Trim().Contains(searchengine))
                    .ToListAsync();

                return View(doctors);
            }
        }
        [Authorize(Roles = "Admin,Doctor,Employee")]

        [HttpGet("Doctors/GetNumberOfPatientsInSpecificDay/{doctorId}")]
        public async Task<IActionResult> GetNumberOfPatientsInSpecificDay(int doctorId, DateOnly? date) { 
            var patientsinaspecificday = await dbcontext.appointments.Where(d=>(d.DoctorDate==date||d.ConsultationDate==date)&&d.DoctorId==doctorId).CountAsync();
            //ViewBag.Date = date;
            ViewBag.DoctorId = doctorId;

            return View(patientsinaspecificday);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AddDoctors()
        {
            var departments = dbcontext.departments
                .Select(d => new { d.Id, Name=d.Name.ToString() })
                .ToList();

            ViewBag.DeptEnum = new SelectList(departments, "Id", "Name");
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> AddDoctors(DoctorsDto doctor)
        {
            if (ModelState.IsValid)
            {
                Doctors doctors = new()
                {
                    Name = doctor.Name,
                    Age = doctor.Age,
                    Gender = doctor.Gender,
                    Phone = doctor.Phone,
                    DoctorRank = doctor.DoctorRank,
                    Salary = doctor.Salary,
                    BookingSalary = doctor.BookingSalary,
                    DepartmentId = doctor.DepartmentId,
                    NumberOFPatientsonDaysofwork = doctor.NumberOFPatientsonDaysofwork,
                };

                await dbcontext.doctors.AddAsync(doctors);
                await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تمت إضافة الطبيب بنجاح";

                return RedirectToAction(nameof(GetDoctors));
            }

            var departments = dbcontext.departments
                .Select(d => new { d.Id, Name = d.Name.ToString() })
                .ToList();
            ViewBag.DeptEnum = new SelectList(departments, "Id", "Name");

            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"Key: {key} | Error: {error.ErrorMessage}");
                }
            }
            return View(doctor);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateDoctor(int id)
        {
            var doctor = await dbcontext.doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
            DoctorsDto doctors = new()
            {
              Name=doctor.Name,
              Age=doctor.Age,
              Gender=doctor.Gender,
              Phone=doctor.Phone,
              DoctorRank = doctor.DoctorRank,
              Salary = doctor.Salary,
              BookingSalary = doctor.BookingSalary, 
              NumberOFPatientsonDaysofwork= doctor.NumberOFPatientsonDaysofwork,
            };
           var docdept = await dbcontext.departments.ToListAsync();
            ViewBag.DocDept = docdept.FirstOrDefault(d=>d.Id==doctor.DepartmentId);


            return View(doctors);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDoctor(DoctorsDto doctors,int id)
        {
            if (ModelState.IsValid)
            {
                var doctor = await dbcontext.doctors.FirstOrDefaultAsync(d => d.Id == id);
                if(doctor == null)
                {
                    return NotFound();
                }
                doctor.Name = doctors.Name;  
                doctor.Age = doctors.Age;    
                doctor.Gender = doctors.Gender;  
                doctor.Phone = doctors.Phone;
                doctor.DoctorRank = doctors.DoctorRank;
                doctor.Salary = doctors.Salary;
                doctor.BookingSalary = doctors.BookingSalary; 
                doctor.NumberOFPatientsonDaysofwork = doctors.NumberOFPatientsonDaysofwork;
                dbcontext.doctors.Update(doctor);
                await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تمت عملية التعديل بنجاح";

                return RedirectToAction(nameof(GetDoctors));
            }
     
            return View(doctors);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteDoctor(DoctorsDto doctors, int id)
        {
            var doctor = await dbcontext.doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
             doctors = new()
            {
                Name = doctor.Name,
                Age = doctor.Age,
                Gender = doctor.Gender,
                Phone = doctor.Phone,
                DoctorRank = doctor.DoctorRank,
                Salary = doctor.Salary,
                BookingSalary = doctor.BookingSalary,
                NumberOFPatientsonDaysofwork=doctor.NumberOFPatientsonDaysofwork,
            };
            var docdept = await dbcontext.departments.ToListAsync();
            ViewBag.DocDept = docdept.FirstOrDefault(d => d.Id == doctor.DepartmentId);
            return View(doctors);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            if (ModelState.IsValid)
            {
                var doctor = dbcontext.doctors.FirstOrDefault(d => d.Id == id);
                if(doctor == null)
                {
                    return NotFound();
                }
                dbcontext.doctors.Remove(doctor);
                await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تمت عملية الحذف بنجاح";

                return RedirectToAction(nameof(GetDoctors));
            }
            return View();
        }
    }
}
