using Hospital_Project.Data;
using Hospital_Project.Entities;
using Hospital_Project.Entities.DTOs;
using Hospital_Project.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace Hospital_Project.Controllers;

public class EmployeesController : Controller
{
    private readonly hospitaldbcontext dbcontext;
    public EmployeesController(hospitaldbcontext Dbcontext)
    {
        dbcontext = Dbcontext;
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]

    public IActionResult ShowEmployees(string searchengine="")
    {
        if (string.IsNullOrEmpty(searchengine))
        {
            var employees = dbcontext.employees.Include(a => a.Appointments).ToList();
            return View(employees);
        }
        else
        {
            var emp = dbcontext.employees.Include(a => a.Appointments).ToList();
            searchengine = searchengine.Trim();
            emp = emp.Where(d => d.Phone.Trim().Contains(searchengine)).ToList();
            return View(emp);
        }
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]

    public IActionResult AddEmployee()
    {
        var jobList = Enum.GetValues(typeof(NatureOfWork))
    .Cast<NatureOfWork>()
    .Select(j => new SelectListItem
    {
        Value = ((int)j).ToString(),
        Text = j.GetType()
                .GetMember(j.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()?.Name ?? j.ToString()
    }).ToList();

        ViewBag.JobList = jobList;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddEmployee(EmployeesDto employee)
    {
        if (ModelState.IsValid)
        {
            Employees employees = new()
            {
                Name = employee.Name,
                Age = employee.Age,
                Gender = employee.Gender,
                Phone = employee.Phone,
                NatureOfWork= employee.NatureOfWork,
                Salary= employee.Salary,
            };
            await dbcontext.employees.AddAsync(employees);
            await dbcontext.SaveChangesAsync();
            TempData["Success"] = "تمت إضافة الموظف بنجاح";

            return RedirectToAction(nameof(ShowEmployees));
        }

        return View(employee);
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]

    public IActionResult UpdateEmployee(int id)
    {
        var employee = dbcontext.employees.FirstOrDefault(p => p.Id == id);
        if (employee == null)
        {
            return NotFound();
        }
        EmployeesDto dto = new()
        {
            Name = employee.Name,
            Age = employee.Age,
            Gender = employee.Gender,
            Phone = employee.Phone,
            NatureOfWork= employee.NatureOfWork,
            Salary = employee.Salary,

        };
        var jobList = Enum.GetValues(typeof(NatureOfWork))
.Cast<NatureOfWork>()
.Select(j => new SelectListItem
{
Value = ((int)j).ToString(),
Text = j.GetType()
        .GetMember(j.ToString())
        .First()
        .GetCustomAttribute<DisplayAttribute>()?.Name ?? j.ToString()
}).ToList();

        ViewBag.JobList = jobList;

        return View(dto);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateEmployee(EmployeesDto employees, int id)
    {
        if (ModelState.IsValid)
        {
            var employee = dbcontext.employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            employee.Name = employees.Name;
            employee.Age = employees.Age;
            employee.Gender = employees.Gender;
            employee.Phone = employees.Phone;
            employee.NatureOfWork = employees.NatureOfWork;
            employee.Salary = employees.Salary; 
            dbcontext.employees.Update(employee);
            await dbcontext.SaveChangesAsync();
            TempData["Success"] = "تمت عملية التعديل بنجاح";

            return RedirectToAction(nameof(ShowEmployees));
        }
        return View(employees);
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]

    public IActionResult DeleteEmployee(EmployeesDto dto, int id)
    {

        var employee = dbcontext.employees.FirstOrDefault(p => p.Id == id);
        if (employee == null)
        {
            return NotFound();
        }
        dto = new()
        {
            Name = employee.Name,
            Age = employee.Age,
            Gender = employee.Gender,
            Phone = employee.Phone,
            NatureOfWork = employee.NatureOfWork,
            Salary = employee.Salary
        };
        return View(dto);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = dbcontext.employees.FirstOrDefault(x => x.Id == id);
        if (employee == null)
        {
            return NotFound();
        }
        dbcontext.employees.Remove(employee);
        await dbcontext.SaveChangesAsync();
        TempData["Success"] = "تمت عملية الحذف بنجاح";

        return RedirectToAction(nameof(ShowEmployees));
    }
}
