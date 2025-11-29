using Hospital_Project.Data;
using Hospital_Project.Entities;
using Hospital_Project.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Project.Controllers
{
    public class DoctorAvailableTimeController : Controller
    {
        private readonly hospitaldbcontext dbcontext;
        public DoctorAvailableTimeController(hospitaldbcontext Dbcontext)
        {
            dbcontext = Dbcontext;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Employee,Nurse,Patient,Doctor")]

        public async Task<IActionResult> GetAllavailabledates(string searchengine = "")
        {
            if (searchengine is null)
            {
                var dates = await dbcontext.DoctorAvailableDate
                .Include(d => d.Doctor.Department)
                .Include(d => d.Appointments)
                .ToListAsync();
                return View(dates);
            }
            var search = searchengine.Trim();
            var engine = await dbcontext.DoctorAvailableDate.Include(d => d.Doctor.Department).Include(DAT => DAT.Appointments).Where(e => e.Doctor.Name.Trim().Contains(search)).ToListAsync();
            return View(engine);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AddDoctorAvailableTime()
        {
            ViewBag.Doctors = await dbcontext.doctors.Include(d=>d.Department).ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddDoctorAvailableTime(DoctorAvailableTimeDTO dto)
        {
            if (ModelState.IsValid)
            {
                DoctorsTimeTable timetable = new()
                {
                    DoctorId = dto.DoctorId,
                    AvailableTimes = dto.AvailableTimes,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime
                };

                await dbcontext.DoctorAvailableDate.AddAsync(timetable);
                await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تم إضافة الموعد بنجاح";

                return RedirectToAction(nameof(GetAllavailabledates));
            }

            ViewBag.Doctors = await dbcontext.doctors.Include(d => d.Department).ToListAsync();

            return View(dto);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> UpdateDoctorAvailableTime(int id)
        {
            var docavailabletime = await dbcontext.DoctorAvailableDate.FirstOrDefaultAsync(d => d.Id == id);
            if (docavailabletime == null)
            {
                NotFound();
            }
            DoctorAvailableTimeDTO doctor = new()
            {
                DoctorId = docavailabletime.DoctorId,
                AvailableTimes = docavailabletime.AvailableTimes,
                StartTime = docavailabletime.StartTime,
                EndTime = docavailabletime.EndTime,
            };
            ViewBag.Doctors = await dbcontext.doctors.Include(d=>d.Department).ToListAsync();
            return View(doctor);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDoctorAvailableTime(DoctorAvailableTimeDTO doc, int id)
        {
            if (ModelState.IsValid)
            {
                var availabletime = await dbcontext.DoctorAvailableDate.FirstOrDefaultAsync(at => at.Id == id);
                if(availabletime == null)
                {
                    return NotFound();
                }

                availabletime.DoctorId = doc.DoctorId;
                availabletime.AvailableTimes = doc.AvailableTimes;
                availabletime.StartTime = doc.StartTime;
                availabletime.EndTime = doc.EndTime;
                
                 dbcontext.DoctorAvailableDate.Update(availabletime);
                await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تمت عملية التعديل بنجاح";

                return RedirectToAction(nameof(GetAllavailabledates));
            }
            return View(doc);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> DeleteDoctorAvailableTime(DoctorAvailableTimeDTO doctor,int id)
        {
            var docavailabletime = await dbcontext.DoctorAvailableDate.FirstOrDefaultAsync(d => d.Id == id);
            if (docavailabletime == null)
            {
                NotFound();
            }
             doctor = new()
            {
                DoctorId = docavailabletime.DoctorId,
                AvailableTimes = docavailabletime.AvailableTimes,
                StartTime = docavailabletime.StartTime,
                EndTime = docavailabletime.EndTime
            };
            var docname = dbcontext.doctors.FirstOrDefault(dn => dn.Id == doctor.DoctorId);
            ViewBag.DocName = docname?.Name ?? "هذا الطبيب غير موجود";
            return View(doctor);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDoctorAvailableTime(int id)
        {
            if (ModelState.IsValid)
            {
                var availabletime = await dbcontext.DoctorAvailableDate.FirstOrDefaultAsync(at => at.Id == id);
                if (availabletime == null)
                {
                    return NotFound();
                }
                dbcontext.DoctorAvailableDate.Remove(availabletime);
                await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تمت عملية الحذف بنجاح";
                return RedirectToAction(nameof(GetAllavailabledates));
            }
            return View();
        }
    }
}
