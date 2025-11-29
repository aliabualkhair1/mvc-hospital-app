using Hospital_Project.Data;
using Hospital_Project.Entities.Extensions;
using Hospital_Project.Entities.User;
using Hospital_Project.Entities.UserDTOs.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

namespace Hospital_Project
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<hospitaldbcontext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("hdbcont")));

            builder.Services.AddIdentity<UserInfo, IdentityRole>()
                .AddEntityFrameworkStores<hospitaldbcontext>()
                .AddDefaultTokenProviders();

            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    if (httpContext.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                    {
                        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(15),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        });
                    }

                    return RateLimitPartition.GetNoLimiter("");
                });

                options.RejectionStatusCode = 429;
                options.OnRejected = async (context, token) =>
                {
                    var http = context.HttpContext;

                    http.Response.Redirect("/RateLimit/Blocked");
                    await Task.CompletedTask;
                };

            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Home/AccessDenied";
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.Redirect("/Home/AccessDenied");
                    return Task.CompletedTask;
                };
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedRole.InitializeAsync(services);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRateLimiter(); 
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=UserInfo}/{action=SignIn}/{id?}");

            app.Run();
        }
    }
}
