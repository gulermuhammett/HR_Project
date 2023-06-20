using FluentValidation.AspNetCore;
using HR_Project.Repositories.Abstract;
using HR_Project.Repositories.Concrete;
using HR_Project.Repositories.Context;
using HR_Project.Services.Abstract;
using HR_Project.Services.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HR_Project.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation().AddFluentValidation
            (fv => fv.RegisterValidatorsFromAssemblyContaining<HR_Project.Entities.UserValidator.UserValidator>()).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<HR_Project.Entities.UserValidation.AdvanceValidator>());

            builder.Services.AddDbContext<HRProjectContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Con"));
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(x =>
            {
                x.LoginPath = "/Home/Login";
                x.AccessDeniedPath = "/Home/ErisimEngellendi";
                x.ExpireTimeSpan = TimeSpan.FromMinutes(200);
                x.SlidingExpiration = true;
                x.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            }
            );
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CompanyManagerPolicy", policy =>
                    policy.RequireRole("CompanyManager"));
            });

            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));

            //AutoMapper
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=CompanyManagerHome}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "areas2",
                pattern: "{area:exists}/{controller=EmployeeHome}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}