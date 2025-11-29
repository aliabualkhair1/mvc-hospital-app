using Microsoft.AspNetCore.Mvc;

namespace Hospital_Project.Controllers
{
    public class RateLimitController : Controller
    {
        public IActionResult Blocked()
        {
            TempData["RateLimitMessage"] = "تم تجاوز الحد المسموح به من المحاولات. برجاء المحاولة لاحقًا.";
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

}
