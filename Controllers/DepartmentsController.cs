using Hospital_Project.Data;
using Hospital_Project.Entities;
using Hospital_Project.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Project.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly hospitaldbcontext dbcontext;

        public DepartmentsController(hospitaldbcontext Dbcontext)
        {
            dbcontext = Dbcontext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee,Nurse,Patient,Doctor")]
        public async Task<IActionResult> ShowDepartments(string searchengine = "")
        {
            var filtration = searchengine.Trim();

            var depts = await dbcontext.departments
                .Include(d => d.Doctors)
                .Include(n => n.Nurses)
                .ToListAsync();

            if (!string.IsNullOrEmpty(filtration))
            {
                depts = depts
                    .Where(d => d.Name.ToString().Contains(filtration, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(depts);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddDept()
        {
            var enumValues = Enum.GetValues(typeof(Departments))
                                 .Cast<Departments>()
                                 .Select(e => new { Id = (int)e, Name = e.ToString() })
                                 .ToList();

            ViewBag.DeptEnum = new SelectList(enumValues, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDept(DepartmentDto dept)
        {
            if(ModelState.IsValid)
            {
                Department depts = new()
                {
                    Id = dept.Id,
                    Name = dept.Name,
                };
                await dbcontext.AddAsync(depts);
                await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تمت إضافة القسم بنجاح";

                return RedirectToAction(nameof(ShowDepartments));
             
            }
            return View(dept);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateDept(int id)
        {
            var depts = await dbcontext.departments.FirstOrDefaultAsync(d => d.Id == id);
            if(depts == null)
            {
                return NotFound();
            }
            DepartmentDto dept = new()
            {
                Id = depts.Id,
                Name = depts.Name,
            };
            var enumValues = Enum.GetValues(typeof(Departments))
                     .Cast<Departments>()
                     .Select(e => new { Id = (int)e, Name = e.ToString() })
                     .ToList();

            ViewBag.DeptEnum = new SelectList(enumValues, "Id", "Name");
            return View(dept);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDept(int id ,DepartmentDto dept)
        {
            if(ModelState.IsValid)
            {
                var depts=await dbcontext.departments.FirstOrDefaultAsync(d => d.Id == id);
                if(depts == null)
                {
                    return NotFound();
                }
                depts.Name = dept.Name;
                dbcontext.departments.Update(depts);
              await  dbcontext.SaveChangesAsync();
                TempData["Success"] = "تمت عملية التعديل بنجاح";

                return RedirectToAction(nameof(ShowDepartments));
            }
            return View(dept);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteDept(int id)
        {
            var depts = await dbcontext.departments.FirstOrDefaultAsync(d => d.Id == id);
            if (depts == null)
            {
                return NotFound();
            }
            DepartmentDto dept = new()
            {
                Id = depts.Id,
                Name = depts.Name,
            };
            return View(dept);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDept(int id,DepartmentDto dept)
        {
            if (ModelState.IsValid)
            {
                var depts = await dbcontext.departments.FirstOrDefaultAsync(d => d.Id == id);
                if (depts == null)
                {
                    return NotFound();
                }
               
                dbcontext.departments.Remove(depts);
                await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تمت عملية الحذف بنجاح";
                return RedirectToAction(nameof(ShowDepartments));
            }
            return View(dept);
        }
    }
}
