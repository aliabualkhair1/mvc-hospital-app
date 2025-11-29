using Hospital_Project.Data;
using Hospital_Project.Entities;
using Hospital_Project.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Project.Controllers
{
    public class NursesController : Controller
    {
        private readonly hospitaldbcontext dbcontext;

        public NursesController(hospitaldbcontext Dbcontext)
        {
            dbcontext = Dbcontext;
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> ShowNurses(string searchengine = "")
        {
            var nurses = dbcontext.nurses
                .Include(n => n.Department)
                .Include(n => n.Doctors)
                    .ThenInclude(dn => dn.Doctor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchengine))
            {
                nurses = nurses.Where(n => n.Name.Trim().Contains(searchengine.Trim()));
            }

            return View(await nurses.ToListAsync()); 
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AddNurse()
        {
                ViewBag.Departments = new SelectList(await dbcontext.departments.ToListAsync(), "Id", "Name");
                return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetDoctorsByDepartment(int departmentId)
        {
            var doctors = await dbcontext.doctors
                .Where(d => d.DepartmentId == departmentId)
                .Select(d => new { d.Id, d.Name })
                .ToListAsync();

            return Json(doctors);
        }


        [HttpPost]
        public async Task<IActionResult> AddNurse(NursesDto nurse, List<int> selectedDoctorIds)
        {
            if (ModelState.IsValid)
            {
                var newNurse = new Nurses
                {
                    Name = nurse.Name,
                    Age = nurse.Age,
                    Gender = nurse.Gender,
                    StartTime = nurse.StartTime,
                    EndTime = nurse.EndTime,
                    Salary = nurse.Salary,
                    Phone = nurse.Phone,
                    Departmentid = nurse.Departmentid
                };

                await dbcontext.nurses.AddAsync(newNurse);
                await dbcontext.SaveChangesAsync();

                foreach (var docId in selectedDoctorIds)
                {
                    dbcontext.DocNurse.Add(new DocNurse
                    {
                        NurseId = newNurse.Id,
                        DoctorId = docId
                    });
                }

                await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تمت إضافة الممرض بنجاح";

                return RedirectToAction(nameof(ShowNurses));
            }

            ViewBag.Departments = new SelectList(await dbcontext.departments.ToListAsync(), "Id", "Name");
            ViewBag.Doctors = new MultiSelectList(await dbcontext.doctors.ToListAsync(), "Id", "Name");
            return View(nurse);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> UpdateNurse(int id)
        {
            var nurse = await dbcontext.nurses
                .Include(n => n.Doctors)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (nurse == null)
                return NotFound();

            var dto = new NursesDto
            {
                Id = nurse.Id,
                Name = nurse.Name,
                Age = nurse.Age,
                Gender = nurse.Gender,
                StartTime = nurse.StartTime,
                EndTime = nurse.EndTime,
                Salary = nurse.Salary,
                Phone = nurse.Phone,
                Departmentid = nurse.Departmentid
            };

            var selectedDoctorIds = nurse.Doctors.Select(dn => dn.DoctorId).ToList();

            ViewBag.Departments = new SelectList(await dbcontext.departments.ToListAsync(), "Id", "Name", nurse.Departmentid);
            ViewBag.SelectedDoctorIds = selectedDoctorIds;
            return View(dto);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateNurse(int id, NursesDto dto, List<int> selectedDoctorIds)
        {
            if (ModelState.IsValid)
            {
                var nurse = await dbcontext.nurses
                    .Include(n => n.Doctors)
                    .FirstOrDefaultAsync(n => n.Id == id);

                if (nurse == null)
                    return NotFound();

                nurse.Name = dto.Name;
                nurse.Age = dto.Age;
                nurse.Gender = dto.Gender;
                nurse.StartTime = dto.StartTime;
                nurse.EndTime = dto.EndTime;
                nurse.Salary = dto.Salary;
                nurse.Phone = dto.Phone;
                nurse.Departmentid = dto.Departmentid;

                dbcontext.DocNurse.RemoveRange(nurse.Doctors);

                foreach (var docId in selectedDoctorIds)
                {
                    dbcontext.DocNurse.Add(new DocNurse
                    {
                        NurseId = nurse.Id,
                        DoctorId = docId
                    });
                }

                await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تمت عملية التعديل بنجاح";
                return RedirectToAction(nameof(ShowNurses));
            }

            ViewBag.Departments = new SelectList(await dbcontext.departments.ToListAsync(), "Id", "Name", dto.Departmentid);
            ViewBag.Doctors = new MultiSelectList(await dbcontext.doctors.ToListAsync(), "Id", "Name", selectedDoctorIds);
            return View(dto);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> DeleteNurse(int id)
        {
            var nurse = await dbcontext.nurses
                .Include(n => n.Doctors)
                .Include(n => n.Department)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (nurse == null)
                return NotFound();

            var dto = new NursesDto
            {
                Id = nurse.Id,
                Name = nurse.Name,
                Age = nurse.Age,
                Gender = nurse.Gender,
                StartTime = nurse.StartTime,
                EndTime = nurse.EndTime,
                Salary = nurse.Salary,
                Phone = nurse.Phone,
                Departmentid = nurse.Departmentid
            };

            ViewBag.Department = nurse.Department;
            ViewBag.Doctors = await dbcontext.doctors
                .Where(d => nurse.Doctors.Select(x => x.DoctorId).Contains(d.Id))
                .ToListAsync();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNurse(int id, NursesDto dto)
        {
            var nurse = await dbcontext.nurses
                .Include(n => n.Doctors)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (nurse == null)
                return NotFound();

            dbcontext.DocNurse.RemoveRange(nurse.Doctors);
            dbcontext.nurses.Remove(nurse);
            await dbcontext.SaveChangesAsync();
            TempData["Success"] = "تمت عملية الحذف بنجاح";

            return RedirectToAction(nameof(ShowNurses));
        }
    }
}
