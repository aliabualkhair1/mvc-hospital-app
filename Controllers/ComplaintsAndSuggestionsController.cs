using Hospital_Project.Data;
using Hospital_Project.Entities;
using Hospital_Project.Entities.DTOs;
using Hospital_Project.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Project.Controllers
{
    public class ComplaintsAndSuggestionsController : Controller
    {
        private readonly hospitaldbcontext dbcontext;

        public UserManager<UserInfo> USER { get; }

        public ComplaintsAndSuggestionsController(hospitaldbcontext Dbcontext,UserManager<UserInfo> uSER)
        {
          dbcontext = Dbcontext;
            USER = uSER;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Employee,Nurse,Patient,Doctor")]
        public async Task<IActionResult> GetAllComplaintsAndSuggestions()
        {
            var user = await USER.GetUserAsync(User);              
            var roles = await USER.GetRolesAsync(user);           
            var isAdmin = roles.Contains("Admin");                 

            IQueryable<ComplaintsAndSuggestions> query = dbcontext.ComplaintsAndSuggestions
                .Include(c => c.User)                              
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(c => c.UserId == user.Id);     
            }

            var result = await query.ToListAsync();
            return View(result);
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Employee,Nurse,Patient,Doctor")]
        public IActionResult AddComplaintsAndSuggestions()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee,Nurse,Patient,Doctor")]
        public async Task<IActionResult> AddComplaintsAndSuggestions(ComplaintsAndSuggestionsDto casdto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                var CAS = new ComplaintsAndSuggestions
                {
                    UserId = userId,
                    Name = casdto.Name,
                    Phone = casdto.Phone,
                    ProblemandSuggestion = casdto.ProblemandSuggestion,
                    ProblemandSuggestionDate = DateTime.Now 
                };

                await dbcontext.ComplaintsAndSuggestions.AddAsync(CAS);
                await dbcontext.SaveChangesAsync();

                TempData["Success"] = "تمت إضافة الشكوى أو الاقتراح بنجاح";
                return RedirectToAction(nameof(GetAllComplaintsAndSuggestions));
            }

            return View(casdto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee,Nurse,Patient,Doctor")]
        public async Task<IActionResult> DeleteComplaintsAndSuggestions(int id)
        {
            var dcas=await dbcontext.ComplaintsAndSuggestions.FirstOrDefaultAsync(dc => dc.Id == id);
            if(dcas == null)
            {
                return NotFound();
            }
            ComplaintsAndSuggestionsDto cas = new()
            {
                Name = dcas.Name,
                Phone = dcas.Phone,
                ProblemandSuggestion = dcas.ProblemandSuggestion,
            };
            return View(cas);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteComplaintsAndSuggestions(ComplaintsAndSuggestionsDto casdto)
        {
            if (ModelState.IsValid)
            {
                var CAS = await dbcontext.ComplaintsAndSuggestions.FirstOrDefaultAsync(c => c.Id == casdto.Id);
                if (CAS == null)
                {
                    return NotFound();
                }
                dbcontext.ComplaintsAndSuggestions.Remove(CAS);
                await dbcontext.SaveChangesAsync();
                TempData["Success"] = "تم حذف الشكوى أو الإقتراح بنجاح";
                return RedirectToAction(nameof(GetAllComplaintsAndSuggestions));
            }
            return View(casdto);    
        }
    }
}
