using Hospital_Project.Data;
using Hospital_Project.Entities;
using Hospital_Project.Entities.DTOs;
using Hospital_Project.Entities.Enums;
using Hospital_Project.Entities.Extensions;
using Hospital_Project.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hospital_Project.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly hospitaldbcontext dbcontext;

        public UserManager<UserInfo> USER { get; }

        public AppointmentsController(hospitaldbcontext Dbcontext,UserManager<UserInfo> uSER)
        {
            dbcontext = Dbcontext;
            this.USER = uSER;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Employee,Patient,Nurse,Doctor")]
        public async Task<IActionResult> GetAppointments(DateOnly? searchengine, string? phone = "", string search = "")
        {
            var user = await USER.GetUserAsync(User);
            var roles = await USER.GetRolesAsync(user);
            var isAdminOrEmployee = roles.Contains("Admin") || roles.Contains("Employee");

            var patient = await dbcontext.patients.FirstOrDefaultAsync(p => p.UserId == user.Id);

            var query = dbcontext.appointments
                .Include(a => a.Patient)
                .Include(d => d.Doctor)
                .Include(d => d.Doctor.Department)
                .Include(e => e.Employee)
                .Include(dad => dad.DoctorAvailableTime)
                .AsQueryable();

            if (!isAdminOrEmployee)
            {
                if (patient != null)
                {
                    query = query.Where(a => a.PatientId == patient.Id);
                }
                else
                {
           
                    return View(new List<Appointments>());
                }
            }
            if (searchengine.HasValue && !string.IsNullOrEmpty(phone))
            {
                query = query.Where(a =>
                    a.DoctorDate == searchengine.Value ||
                    a.Patient.Phone.Contains(phone.Trim()));
            }
            else if (searchengine.HasValue)
            {
                query = query.Where(a => a.DoctorDate == searchengine.Value);
            }
            else if (!string.IsNullOrEmpty(search))
            {
                var searchen = search.Trim();
                query = query.Where(a => a.Patient.Name.Contains(searchen));
            }
            else if (!string.IsNullOrEmpty(phone))
            {
                var phoneengine = phone.Trim();
                query = query.Where(a => a.Patient.Phone.Contains(phoneengine));
            }

            var result = await query.ToListAsync();
            return View(result);
        }
        [Authorize(Roles = "Admin,Employee,Patient,Nurse,Doctor")]
        [HttpGet]
        public async Task<IActionResult> GetAvailableTimesByDoctor(int doctorId)
        {
            var availableTimes = await dbcontext.DoctorAvailableDate
                .Where(x => x.DoctorId == doctorId)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.AvailableTimes.ToString()  + " من " + x.StartTime.ToString("hh:mm tt", new System.Globalization.CultureInfo("ar-EG")) +" إلى "+ x.EndTime.ToString("hh:mm tt", new System.Globalization.CultureInfo("ar-EG"))
                }).ToListAsync();

            return Json(availableTimes);
        }

        [HttpGet]

        [Authorize(Roles = "Admin,Employee,Doctor,Nurse,Patient")]
        public async Task<IActionResult> AddAppointment()
        {
            var user = await USER.GetUserAsync(User);
            var roles = await USER.GetRolesAsync(user);
            var isAdminOrEmployee = roles.Contains("Admin") || roles.Contains("Employee");

            List<Patients> patients;

            if (isAdminOrEmployee)
            {
                patients = await dbcontext.patients.ToListAsync();
            }
            else
            {
                patients = await dbcontext.patients
                    .Where(p => p.UserId == user.Id)
                    .ToListAsync();
            }

            var doctors = await dbcontext.doctors
                .Include(d => d.Department)
                .ToListAsync();

            var employees = await dbcontext.employees
                .Where(e => e.NatureOfWork == NatureOfWork.موظف_استقبال
                         || e.NatureOfWork == NatureOfWork.موظف_سجلات_طبية)
                .ToListAsync();

            var DTT = await dbcontext.DoctorAvailableDate.ToListAsync();
            var departments = await dbcontext.departments.ToListAsync();

            ViewBag.Patients = patients;
            ViewBag.Doctors = doctors;
            ViewBag.Employees = employees;
            ViewBag.DTT = DTT;
            ViewBag.DeptEnum = new SelectList(departments, "Id", "Name");

            return View();
        }

        private async Task LoadViewBags()
        {
            ViewBag.Doctors = await dbcontext.doctors.Include(d => d.Department).ToListAsync();
            ViewBag.Patients = await dbcontext.patients.ToListAsync();
            ViewBag.Employees = await dbcontext.employees.ToListAsync();
            ViewBag.AvailableTimes = await dbcontext.DoctorAvailableDate.ToListAsync();
            var departments = await dbcontext.departments.ToListAsync();
            ViewBag.DeptEnum = new SelectList(departments, "Id", "Name");
        }

        [HttpPost]
        public async Task<IActionResult> AddAppointment(AppointmentsDto appointments)
        {
            if (ModelState.IsValid)
            {
                Appointments appointment = new()
                {
                    DateOfBooking = DateTime.Now,
                    DoctorDate = appointments.DoctorDate,
                    PatientId = appointments.PatientId,
                    DoctorId = appointments.DoctorId,
                    EmployeeId = appointments.EmployeeId,
                    DoctorAvailableTimeId = appointments.DoctorAvailableTimeId,
                    ConsultationDate = appointments.ConsultationDate,
                    EndOfConsultationDate = appointments.DoctorDate.AddDays(12)
                };

                if (appointment.DoctorDate != null)
                {
                    var doctor = await dbcontext.doctors
                        .FirstOrDefaultAsync(d => d.Id == appointment.DoctorId);

                    if (doctor == null)
                    {
                        ModelState.AddModelError("", "هذا الطبيب غير موجود.");
                        await LoadViewBags(); 
                        return View(appointments);
                    }

                    int NumberOfBooking = await dbcontext.appointments
                        .Where(d => d.DoctorId == appointment.DoctorId &&
                                    d.DoctorDate == appointment.DoctorDate &&
                                    d.DateOfBooking < appointment.DateOfBooking)
                        .CountAsync();

                    appointment.NumberOfBooking = NumberOfBooking + 1;

                    if (appointment.NumberOfBooking > doctor.NumberOFPatientsonDaysofwork)
                    {
                        ModelState.AddModelError("", "الحجز مكتمل في هذا اليوم");
                        await LoadViewBags();
                        return View(appointments);
                    }
                }

                if (appointment.ConsultationDate != null)
                {
                    int NumberOfConsultation = await dbcontext.appointments
                        .Where(d => d.DoctorId == appointment.DoctorId &&
                                    d.ConsultationDate != null &&
                                    d.ConsultationDate == appointment.ConsultationDate &&
                                    d.ConsultationDateOfBooking < appointment.ConsultationDateOfBooking)
                        .CountAsync();

                    appointment.NumberOfConsultation = NumberOfConsultation + 1;
                }

                await dbcontext.appointments.AddAsync(appointment);
                await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تم إضافة الكشف بنجاح";

                return RedirectToAction(nameof(GetAppointments));
            }

            await LoadViewBags(); 
            return View(appointments);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Employee,Patient,Nurse,Doctor")]

        public async Task<IActionResult> GetDoctorsByDepartment(int departmentId)
        {
            var doctors = await dbcontext.doctors
                .Where(d => d.DepartmentId == departmentId)
                .Select(d => new {
                    id = d.Id,
                    name = d.Name + " - " + d.DoctorRank
                })
                .ToListAsync();

            return Json(doctors);
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Employee,Patient,Doctor,Nurse")]

        public async Task<IActionResult> GetAvailableTimes(int doctorId)
        {
            var times = await dbcontext.DoctorAvailableDate
                .Where(t => t.DoctorId == doctorId)
                .Select(t => new
                {
                    id = t.Id,
                    time = t.AvailableTimes,
                    start = t.StartTime.ToString("hh\\:mm"),
                    end = t.EndTime.ToString("hh\\:mm")
                }).ToListAsync();

            return Json(times);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee,Nurse,Doctor,Patient")]

        public async Task<IActionResult> UpdateAppointment(int id)
        {
            var appointment = await dbcontext.appointments
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            AppointmentsDto dto = new()
            {
                Id = appointment.Id,
                DoctorDate = appointment.DoctorDate,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                EmployeeId = appointment.EmployeeId,
                DoctorAvailableTimeId = appointment.DoctorAvailableTimeId,
                EndOfConsultationDate = appointment.DoctorDate.AddDays(12),
                ConsultationDate = appointment.ConsultationDate,
            };


            await LoadViewBags();
            return View(dto);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateAppointment(AppointmentsDto appoint, int id)
        {
            if (ModelState.IsValid)
            {
                var appoints = await dbcontext.appointments.FirstOrDefaultAsync(a => a.Id == id);

                if (appoints == null)
                {
                    return NotFound();
                }
                appoints.DoctorDate = appoint.DoctorDate;
                appoints.PatientId = appoint.PatientId;
                appoints.DoctorId = appoint.DoctorId;
                appoints.EmployeeId = appoint.EmployeeId;
                appoints.DoctorAvailableTimeId = appoint.DoctorAvailableTimeId;
                appoints.EndOfConsultationDate = appoint.DoctorDate.AddDays(12);
                appoints.ConsultationDate = appoint.ConsultationDate;

                if (appoints.DoctorDate != null)
                {
                    var doctor = await dbcontext.doctors.FirstOrDefaultAsync(d => d.Id == appoints.DoctorId);
                    if (doctor == null)
                    {
                        ModelState.AddModelError("", "الطبيب غير موجود");
                        return View(appoint);
                    }

                    int NumberOfBooking = await dbcontext.appointments
                        .Where(d => d.DoctorId == appoints.DoctorId &&
                                    d.DoctorDate == appoints.DoctorDate &&
                                    d.Id != appoints.Id)
                        .CountAsync();

                    appoints.NumberOfBooking = NumberOfBooking + 1;

                    if (appoints.NumberOfBooking > doctor.NumberOFPatientsonDaysofwork)
                    {
                        ModelState.AddModelError("", "الحجز مكتمل في هذا اليوم");
                        return View(appoint);
                    }
                }

                if (appoints.ConsultationDate != null)
                {
                    int NumberOfConsultation = await dbcontext.appointments
                        .Where(d => d.DoctorId == appoints.DoctorId &&
                                    d.ConsultationDate != null &&
                                    d.ConsultationDate == appoints.ConsultationDate &&
                                    d.Id != appoints.Id)
                        .CountAsync();

                    appoints.NumberOfConsultation = NumberOfConsultation + 1;
                }

                dbcontext.appointments.Update(appoints);
                await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تمت عملية التعديل بنجاح";
                return RedirectToAction(nameof(GetAppointments));
            }
            await LoadViewBags();
            return View(appoint);
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Employee,Nurse,Doctor,Patient")]

        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await dbcontext.appointments.FirstOrDefaultAsync(a => a.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            AppointmentsDto appointments = new()
            {
                DoctorDate = appointment.DoctorDate,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                EmployeeId = appointment.EmployeeId,
                EndOfConsultationDate=appointment.EndOfConsultationDate,
                ConsultationDate=appointment.ConsultationDate,
            };
            var patient = dbcontext.patients.ToList();
            var doctors = dbcontext.doctors.Include(d=>d.Department).ToList();
            var employees = dbcontext.employees.ToList();
            ViewBag.Patients = patient.FirstOrDefault(p=>p.Id==appointment.Patient.Id);
            ViewBag.Doctors = doctors.FirstOrDefault(d => d.Id == appointment.Doctor.Id);
            ViewBag.Employees = employees?.FirstOrDefault(e => e.Id == appointment.Employee?.Id);
            return View(appointments);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(AppointmentsDto appointment,int id)
        {
            if (ModelState.IsValid)
            {
              
                    var apointments = await dbcontext.appointments.FirstOrDefaultAsync(a => a.Id == id);
                    if (apointments == null)
                    {
                        return NotFound();
                    }
                    dbcontext.appointments.Remove(apointments);
                    await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تمت عملية الحذف بنجاح";
                return RedirectToAction(nameof(GetAppointments));

            }
            return View(appointment);
        }
        [HttpGet]
        [Authorize(Roles = "Employee")]

        public ActionResult Print(int id)
        {
            var booking = dbcontext.appointments.Include(a => a.Patient)
        .Include(a => a.Doctor)
        .Include(a => a.Doctor.Department)
        .Include(a => a.Employee)
        .Include(a => a.DoctorAvailableTime).FirstOrDefault(b => b.Id == id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }
    }
}
