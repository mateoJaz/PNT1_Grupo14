using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MVCVeterinaria.Context;

namespace MVCVeterinaria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<VeterinariaDatabaseContext>(options => options.UseSqlServer(builder.Configuration["ConnectionString:VeterinariaDBConnection"]));

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Acceso/Login";
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    option.AccessDeniedPath = "/Acceso/Login";
                });

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<TurnoService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Acceso}/{action=Login}/{id?}");

            app.Run();
        }
    }
}