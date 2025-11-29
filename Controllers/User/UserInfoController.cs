#region precontroller
//using Hospital_Project.Data;
//using Hospital_Project.Entities.Enums;
//using Hospital_Project.Entities.User;
//using Hospital_Project.Entities.UserDTOs;
//using Hospital_Project.Entities.UserDTOs.EmailSender;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using System.Security.Cryptography;

//namespace Hospital_Project.Controllers.User
//{
//    public class UserInfoController : Controller
//    {
//        private readonly UserManager<UserInfo> USER;
//        private readonly SignInManager<UserInfo> signin;
//        private readonly IEmailSender emailSender;

//        public UserInfoController(UserManager<UserInfo> Registeruser,SignInManager<UserInfo> Signin,IEmailSender EmailSender )
//        {
//            USER = Registeruser;
//            signin = Signin;
//            emailSender = EmailSender;
//        }
//        [HttpGet]
//        public async Task<IActionResult> SignUP()
//        {
//            ViewBag.GenderList = EnumHelper.ToSelectList<Gender>();
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> SignUp(SignUp signUp)
//        {
//            if (ModelState.IsValid)
//            {
//                var verificationCode = new Random().Next(100000, 999999).ToString();
//                var application = new UserInfo
//                {
//                    FirstName = signUp.FirstName,
//                    LastName = signUp.LastName,
//                    Gender = signUp.Gender.Value,
//                    PhoneNumber = signUp.PhoneNumber,
//                    Email = signUp.Email,
//                    UserName = signUp.Email,
//                    VerificationCode = verificationCode,
//                    IsVerified = false
//                };
//                var result = await USER.CreateAsync(application,signUp.Password);
//                if (result.Succeeded)
//                {
//                    await USER.AddToRoleAsync(application,"Patient");
//                    await emailSender.SendEmailAsync(application.Email,"كود التفعيل",$"رمز التفعيل الخاص بك هو: {verificationCode}");
//                    TempData["UserEmail"] = application.Email;
//                    return RedirectToAction(nameof(VerifyAccount), new { email = application.Email });
//                }
//                foreach (var error in result.Errors)
//                {
//                    if (error.Code.Contains("Password"))
//                    {
//                        ModelState.AddModelError("Password","لابد من استخدام احرف كبيرة وصغيرة ورموز وأرقام ولا تقل عن 6 احرف");
//                    }
//                    else if (error.Code.Contains("DuplicateUserName"))
//                    {
//                        ModelState.AddModelError("Email", "هذا البريد الإلكترونى مستخدم من قبل.");
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("", error.Description); 
//                    }
//                }
//            }
//            ViewBag.GenderList = EnumHelper.ToSelectList<Gender>();
//            return View(signUp);
//        }
//        [HttpGet]
//        public IActionResult VerifyAccount(string email)
//        {
//            if (TempData["Error"] != null)
//            {
//                ModelState.AddModelError("", TempData["Error"].ToString());
//            }

//            return View(new VerifyCodeViewModel { Email = email });
//        }

//        [HttpPost]
//        public async Task<IActionResult> VerifyAccount(VerifyCodeViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }

//            var user = await USER.FindByEmailAsync(model.Email);

//            if (user == null)
//            {
//                ModelState.AddModelError("", "البريد الإلكتروني غير موجود.");
//                return View(model);
//            }

//            if (user.VerificationCode == model.Code)
//            {
//                user.IsVerified = true;
//                user.VerificationCode = null;
//                await USER.UpdateAsync(user);

//                return RedirectToAction("SignIn");
//            }

//            ModelState.AddModelError("Code", "رمز التفعيل غير صحيح.");
//            return View(model);
//        }
//        [HttpPost]
//        public async Task<IActionResult> ResendVerificationCode(string email)
//        {
//            if (string.IsNullOrEmpty(email))
//            {
//                TempData["Error"] = "البريد الإلكتروني مطلوب.";
//                return RedirectToAction("VerifyAccount", new { email = "" });
//            }
//            var user = await USER.FindByEmailAsync(email);
//            if (user == null)
//            {
//                return RedirectToAction("VerifyAccount", new { email });
//            }

//            if (user.CodeRequestCount >= 3)
//            {
//                if (user.CodeRequestTimeAgain.HasValue && user.CodeRequestTimeAgain.Value.AddHours(2) > DateTime.UtcNow)
//                {
//                    TempData["Error"] = "لقد تجاوزت عدد مرات إرسال رمز التفعيل. الرجاء المحاولة بعد ساعتين.";
//                    return RedirectToAction("VerifyAccount", new { email });
//                }
//                else
//                {
//                    user.CodeRequestCount = 0;
//                    user.CodeRequestTimeAgain = null;
//                }
//            }
//            var newCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
//            user.VerificationCode = newCode;
//            user.CodeRequestCount++;
//            user.CodeRequestTimeAgain = DateTime.UtcNow;

//            try
//            {
//                await USER.UpdateAsync(user);
//                await emailSender.SendEmailAsync(user.Email, "رمز التفعيل الجديد", $"رمز تفعيل الحساب الجديد الخاص بك هو: {newCode}");
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = "حدث خطأ أثناء إرسال رمز التفعيل. الرجاء المحاولة مرة أخرى.";
//                return RedirectToAction("VerifyAccount", new { email });
//            }

//            return RedirectToAction("VerifyAccount", new { email });
//        }
//        [HttpGet]
//        public async Task<IActionResult> Resetbyemail()
//        {
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> Resetbyemail(EmailForForgetPassword model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }

//            var user = await USER.FindByEmailAsync(model.Email);
//            if (user == null)
//            {
//                ViewBag.Error = "هذا البريد غير موجود";
//                return View(model);
//            }
//            var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
//            user.VerificationCode = code;

//            await USER.UpdateAsync(user);
//            await emailSender.SendEmailAsync(user.Email, "كود التحقق", $"رمز التحقق الخاص بك هو: {code}");
//            return RedirectToAction("ResetPasswordCode", new { email = model.Email });
//        }
//        [HttpGet]
//        public IActionResult ResetPasswordCode(string email)
//        {
//            var model = new VerifyCodeViewModel
//            {
//                Email = email
//            };

//            return View("ResetPasswordCode", model);
//        }
//        [HttpPost]
//        public async Task<IActionResult> ResetPasswordCode(VerifyCodeViewModel model)
//        {
//            if (!ModelState.IsValid)
//                return View("ResetPasswordCode", model);

//            var user = await USER.FindByEmailAsync(model.Email);

//            if (user == null)
//            {
//                ModelState.AddModelError("", "البريد الإلكتروني غير موجود.");
//                return View("ResetPasswordCode", model);
//            }

//            if (user.VerificationCode == model.Code)
//            {
//                user.VerificationCode = null;
//                await USER.UpdateAsync(user);
//                return RedirectToAction("ResetPassword", new { email = model.Email });
//            }
//            ModelState.AddModelError("", "رمز التحقق غير صحيح.");
//            return View("ResetPasswordCode", model);
//        }
//        [HttpPost]
//        public async Task<IActionResult> ResendResetPassword(string email)
//        {
//            var user = await USER.FindByEmailAsync(email);
//            if (user == null)
//            {
//                return RedirectToAction("ResetPasswordCode", new { email });
//            }

//            if (user.CodeRequestCount >= 3)
//            {
//                if (user.CodeRequestTimeAgain.HasValue && user.CodeRequestTimeAgain.Value.AddHours(2) > DateTime.Now)
//                {
//                    TempData["Error"] = "لقد تجاوزت عدد مرات إرسال الكود. برجاء المحاولة بعد ساعتين.";
//                    return RedirectToAction("ResetPasswordCode", new { email });
//                }
//                else
//                {
//                    user.CodeRequestCount = 0;
//                }
//            }

//            var newCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
//            user.VerificationCode = newCode;

//            user.CodeRequestCount++;
//            user.CodeRequestTimeAgain = DateTime.Now;

//            await USER.UpdateAsync(user);
//            await emailSender.SendEmailAsync(user.Email, "رمز إعادة التعيين الجديد", $"رمز إعادة تعيين كلمة المرور الجديد الخاص بك هو: {newCode}");

//            return RedirectToAction("ResetPasswordCode", new { email });
//        }
//        [HttpGet]
//        public IActionResult ResetPassword(string email)
//        {
//            if (string.IsNullOrEmpty(email))
//            {
//                return RedirectToAction("SignIN");
//            }

//            var model = new ResetPassword
//            {
//                Email = email
//            };
//            return View(model);
//        }

//        [HttpPost]
//        public async Task<IActionResult> ResetPassword(ResetPassword model)
//        {
//            if (!ModelState.IsValid)
//                return View(model);

//            var user = await USER.FindByEmailAsync(model.Email);
//            if (user == null)
//            {
//                ModelState.AddModelError("", "البريد الإلكتروني غير موجود.");
//                return View(model);
//            }

//            var generatepasstoken = await USER.GeneratePasswordResetTokenAsync(user);
//            var result = await USER.ResetPasswordAsync(user, generatepasstoken, model.NewPassword);

//            if (result.Succeeded)
//            {
//                return RedirectToAction("SignIN");
//            }

//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError("", error.Description);
//            }

//            return View(model);
//        }

//        [HttpGet]
//        public async Task<IActionResult> SignIN()
//        {
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> SignIN(SignIn signIn)
//        {
//            if (!ModelState.IsValid)
//                return View(signIn);

//            var user = await USER.FindByEmailAsync(signIn.Email);
//            if (user == null)
//            {
//                ModelState.AddModelError(nameof(signIn.Password), "البريد الإلكتروني أو كلمة المرور غير صحيحة");
//                return View(signIn);
//            }

//            if (!user.IsVerified)
//            {
//                ModelState.AddModelError("", "حسابك غير مفعل. برجاء تفعيل الحساب باستخدام الكود المرسل على بريدك الإلكتروني.");
//                return View(signIn);
//            }

//            var result = await signin.PasswordSignInAsync(signIn.Email, signIn.Password, isPersistent: false, lockoutOnFailure: true);

//            if (result.Succeeded)
//            {
//                return RedirectToAction("Index", "Home");
//            }
//            else if (result.IsLockedOut)
//            {
//                ModelState.AddModelError("", "تم غلق حسابك مؤقتًا لمدة 24 ساعة لتجاوزك عدد المحاولات المسموحة.");
//                return View(signIn);
//            }
//            else
//            {
//                var failedCount = await USER.GetAccessFailedCountAsync(user);
//                string errorMessage = "البريد الإلكتروني أو كلمة المرور غير صحيحة";

//                if (failedCount == 1)
//                    errorMessage += " لديك محاولتان متبقيتان.";
//                else if (failedCount == 2)
//                    errorMessage += " لديك محاولة واحدة متبقية.";

//                ModelState.AddModelError(nameof(signIn.Password), errorMessage);
//                return View(signIn);
//            }
//        }


//        [HttpPost]
//        public async Task<IActionResult> SignOut()
//        {
//            await signin.SignOutAsync();
//            return RedirectToAction(nameof(SignIN),nameof(UserInfo));
//        }
//        [HttpGet]
//        public async Task<IActionResult> AccountDetails()
//        {
//            var user = await USER.GetUserAsync(User); 
//            if (user == null)
//            {
//                return NotFound("المستخدم غير موجود");
//            }

//            return View(user); 
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteAccountConfirmed()
//        {
//            var user = await USER.GetUserAsync(User);
//            if (user == null)
//            {
//                return NotFound();
//            }

//            await USER.DeleteAsync(user);
//            await signin.SignOutAsync();
//            return RedirectToAction("SignIn", "UserInfo");
//        }


//    }
//}
#endregion
using Hospital_Project.Data;
using Hospital_Project.Entities.DTOs;
using Hospital_Project.Entities.Enums;
using Hospital_Project.Entities.User;
using Hospital_Project.Entities.UserDTOs;
using Hospital_Project.Entities.UserDTOs.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Data;
using System.Security.Cryptography;

namespace Hospital_Project.Controllers.User
{
    public class UserInfoController : Controller
    {
        private readonly UserManager<UserInfo> USER;
        private readonly SignInManager<UserInfo> signin;
        private readonly IEmailSender emailSender;

        public UserInfoController(UserManager<UserInfo> Registeruser, SignInManager<UserInfo> Signin, IEmailSender EmailSender)
        {
            USER = Registeruser;
            signin = Signin;
            emailSender = EmailSender;
        }

        //private bool IsCodeRequestLimited(UserInfo user, out string? errorMessage)
        //{
        //    errorMessage = null;

        //    if (user.CodeRequestCount >= 3)
        //    {
        //        if (user.CodeRequestTimeAgain.HasValue &&
        //            user.CodeRequestTimeAgain.Value.AddHours(2) > DateTime.UtcNow)
        //        {
        //            var remaining = user.CodeRequestTimeAgain.Value.AddHours(2) - DateTime.UtcNow;
        //            errorMessage = $"لقد تجاوزت عدد مرات إرسال الكود. الرجاء المحاولة بعد {remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}.";
        //            return true;
        //        }
        //        else
        //        {
        //            user.CodeRequestCount = 0;
        //            user.CodeRequestTimeAgain = null;
        //        }
        //    }

        //    return false;
        //}

        [HttpGet]
        public async Task<IActionResult> SignUP()
        {
            ViewBag.GenderList = EnumHelper.ToSelectList<Gender>();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUp signUp)
        {
            if (ModelState.IsValid)
            {
                var existemail=await USER.FindByEmailAsync(signUp.Email);
                if (existemail != null)
                {
                    if (!existemail.IsVerified)
                    {
                        await USER.DeleteAsync(existemail);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "هذا البريد الإلكتروني مستخدم من قبل.");
                        return View(signUp);
                    }
                }
                
                var verificationCode = new Random().Next(100000, 999999).ToString();
                var application = new UserInfo
                {
                    FirstName = signUp.FirstName,
                    LastName = signUp.LastName,
                    Gender = signUp.Gender.Value,
                    PhoneNumber = signUp.PhoneNumber,
                    Email = signUp.Email,
                    UserName = signUp.Email,
                    VerificationCode = verificationCode,
                    IsVerified = false
                };
                var result = await USER.CreateAsync(application, signUp.Password);
                if (result.Succeeded)
                {
                    await USER.AddToRoleAsync(application, "Patient");
                    await emailSender.SendEmailAsync(application.Email, "كود التفعيل", $"رمز التفعيل الخاص بك هو: {verificationCode}");
                    TempData["UserEmail"] = application.Email;
                    return RedirectToAction(nameof(VerifyAccount), new { email = application.Email });
                }
                foreach (var error in result.Errors)
                {
                    if (error.Code.Contains("Password"))
                    {
                        ModelState.AddModelError("Password", "لابد من استخدام أحرف كبيرة وصغيرة ورموز وأرقام ولا تقل عن 6 أحرف");
                    }
                    else if (error.Code.Contains("DuplicateUserName"))
                    {
                        ModelState.AddModelError("Email", "هذا البريد الإلكتروني مستخدم من قبل.");
                    }
                    else
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            ViewBag.GenderList = EnumHelper.ToSelectList<Gender>();
            return View(signUp);
        }
        [HttpGet]
        public IActionResult VerifyAccount(string email)
        {

            var model = new VerifyCodeViewModel
            {
                Email = email
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> VerifyAccount(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await USER.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "البريد الإلكتروني غير موجود.");
                return View(model);
            }

            if (user.VerificationCode == model.Code)
            {
                user.IsVerified = true;
                user.VerificationCode = null;
                await USER.UpdateAsync(user);

                return RedirectToAction("SignIn");
            }

            ModelState.AddModelError("Code", "رمز التفعيل غير صحيح.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResendVerificationCode(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "البريد الإلكتروني مطلوب.";
                return RedirectToAction("VerifyAccount", new { email = "" });
            }

            var user = await USER.FindByEmailAsync(email);
            if (user == null)
            {
                return RedirectToAction("VerifyAccount", new { email });
            }

            //if (user.CodeRequestCount >= 3)
            //{
            //    if (user.CodeRequestTimeAgain.HasValue && user.CodeRequestTimeAgain.Value.AddHours(2) > DateTime.UtcNow)
            //    {
            //        var remainingTime = user.CodeRequestTimeAgain.Value.AddHours(2) - DateTime.UtcNow;
            //        var hours = remainingTime.Hours;
            //        var minutes = remainingTime.Minutes;

            //        TempData["Error"] = $"لقد تجاوزت عدد محاولات إرسال الكود. الرجاء المحاولة مرة أخرى بعد {hours} ساعة و {minutes} دقيقة.";
            //        return RedirectToAction("VerifyAccount", new { email });
            //    }
            //    else
            //    {
            //        user.CodeRequestCount = 0;
            //        user.CodeRequestTimeAgain = null;
            //    }
            //}

            var newCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            user.VerificationCode = newCode;
            //user.CodeRequestCount++;
            //user.CodeRequestTimeAgain = DateTime.UtcNow;

            try
            {
                await USER.UpdateAsync(user);
                await emailSender.SendEmailAsync(user.Email, "رمز التفعيل الجديد", $"رمز تفعيل الحساب الجديد الخاص بك هو: {newCode}");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء إرسال رمز التفعيل. الرجاء المحاولة مرة أخرى.";
                return RedirectToAction("VerifyAccount", new { email });
            }

            TempData["Success"] = "تم إرسال رمز التفعيل بنجاح";
            return RedirectToAction("VerifyAccount", new { email });
        }


        [HttpGet]
        public IActionResult Resetbyemail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Resetbyemail(EmailForForgetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await USER.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewBag.Error = "هذا البريد غير موجود";
                return View(model);
            }

            var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            user.VerificationCode = code;
            //user.CodeRequestCount = 1;
            //user.CodeRequestTimeAgain = DateTime.UtcNow;

            await USER.UpdateAsync(user);
            await emailSender.SendEmailAsync(user.Email, "كود التحقق", $"رمز التحقق الخاص بك هو: {code}");
            return RedirectToAction("ResetPasswordCode", new { email = model.Email });
        }

        [HttpGet]
        public IActionResult ResetPasswordCode(string email)
        {
            var model = new VerifyCodeViewModel
            {
                Email = email
            };

            return View("ResetPasswordCode", model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
                return View("ResetPasswordCode", model);

            var user = await USER.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "البريد الإلكتروني غير موجود.");
                return View("ResetPasswordCode", model);
            }

            if (user.VerificationCode == model.Code)
            {
                user.VerificationCode = null;
                await USER.UpdateAsync(user);
                return RedirectToAction("ResetPassword", new { email = model.Email });
            }

            ModelState.AddModelError("", "رمز التحقق غير صحيح.");
            return View("ResetPasswordCode", model);
        }

        [HttpPost]
        public async Task<IActionResult> ResendResetPassword(string email)
        {
            var user = await USER.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["Error"] = "المستخدم غير موجود.";
                return RedirectToAction("ResetPasswordCode", new { email });
            }

            //if (IsCodeRequestLimited(user, out var errorMessage))
            //{
            //    TempData["Error"] = errorMessage;
            //    return RedirectToAction("ResetPasswordCode", new { email });
            //}

            var newCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            user.VerificationCode = newCode;
            //user.CodeRequestCount++;
            //user.CodeRequestTimeAgain = DateTime.UtcNow;

            var result = await USER.UpdateAsync(user);
            if (result.Succeeded)
            {
                await emailSender.SendEmailAsync(user.Email, "رمز إعادة التعيين الجديد", $"رمز إعادة تعيين كلمة المرور الجديد الخاص بك هو: {newCode}");
            }
            else
            {
                TempData["Error"] = "حدث خطأ أثناء تحديث البيانات.";
            }

            return RedirectToAction("ResetPasswordCode", new { email });
        }

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("SignIN");
            }

            var model = new ResetPassword
            {
                Email = email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await USER.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "البريد الإلكتروني غير موجود.");
                return View(model);
            }

            var generatepasstoken = await USER.GeneratePasswordResetTokenAsync(user);
            var result = await USER.ResetPasswordAsync(user, generatepasstoken, model.NewPassword);

            if (result.Succeeded)
            {
                return RedirectToAction("SignIN");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SignIN()
        {
            return View();
        }

        [HttpPost]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> SignIN(SignIn signIn)

        {
            if (!ModelState.IsValid)
                return View(signIn);

            var user = await USER.FindByEmailAsync(signIn.Email);
            if (user == null)
            {
                ModelState.AddModelError(nameof(signIn.Password), "البريد الإلكتروني أو كلمة المرور غير صحيحة");
                return View(signIn);
            }

            if (!user.IsVerified)
            {
                ModelState.AddModelError("", "حسابك غير مفعل. برجاء تفعيل الحساب قبل تسجيل الدخول.");
                return View(signIn);
            }

            var result = await signin.PasswordSignInAsync(signIn.Email, signIn.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            //    else if (result.IsLockedOut)
            //    {
            //        ModelState.AddModelError("", "تم غلق حسابك مؤقتًا لمدة 24 ساعة لتجاوزك عدد المحاولات المسموحة.");
            //        return View(signIn);
            //    }
            //    else
            //    {
            //        var failedCount = await USER.GetAccessFailedCountAsync(user);
            //        string errorMessage = "البريد الإلكتروني أو كلمة المرور غير صحيحة";

            //        if (failedCount == 1)
            //            errorMessage += " لديك محاولتان متبقيتان.";
            //        else if (failedCount == 2)
            //            errorMessage += " لديك محاولة واحدة متبقية.";

            //        ModelState.AddModelError(nameof(signIn.Password), errorMessage);
            //        return View(signIn);
            //    }
            return View(signIn);
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await signin.SignOutAsync();
            return RedirectToAction(nameof(SignIN), nameof(UserInfo));
        }

        [HttpGet]
        public async Task<IActionResult> AccountDetails()
        {
            var user = await USER.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("المستخدم غير موجود");
            }
            var roles = await USER.GetRolesAsync(user);
            var model = new AccountDetailsViewModel
            {
                User = user,
                Roles = roles.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccountConfirmed()
        {
            var user = await USER.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            await USER.DeleteAsync(user);
            await signin.SignOutAsync();
            return RedirectToAction("SignIn", "UserInfo");
        }
    }
}
