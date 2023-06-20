using HR_Project.Entities.Entities;
using HR_Project.Entities.Enums;
using HR_Project.UI.Models;
using HR_Project.UI.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace HR_Project.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient httpClient;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://mezaapi-v11.azurewebsites.net");
        }
        public IActionResult Index()
        {
            return View();
        }
        string url = "https://mezaapi-v11.azurewebsites.net";
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (loginDTO.Email==null)
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "Please entry your email !");
                return View("Index");
            }
            else
            {
                if (loginDTO.Password==null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, "Please entry your password !");
                    return View("Index");
                }
            }
            User loggedUser = new User();
            User user = new User();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{url}/api/User/Login?email={loginDTO.Email}&password={loginDTO.Password}"))
                {
                    
                    if (cevap.IsSuccessStatusCode)
                    {
                        string apiCevap = await cevap.Content.ReadAsStringAsync();
                        loggedUser = JsonConvert.DeserializeObject<User>(apiCevap);
                    }
                    else if (cevap.StatusCode == HttpStatusCode.BadRequest || cevap.StatusCode == HttpStatusCode.NotFound)
                    {
                        string errorMessage = await cevap.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, errorMessage);
                        return View("Index");
                    }
                    
                }
                using (var answ = await httpClient.GetAsync($"{url}/api/User/GetUserByIdInclude/{loggedUser.ID}"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<List<User>>(apiResult)[0];
                }
            }
            if (loggedUser != null)
            {
                if (user.FirstName2==null)
                    user.FirstName2 = "";
                if (user.LastName2 == null)
                    user.LastName2 = "";
                if (user.Photo == null)
                    user.Photo = "";
                var claims = new List<Claim>()
                {
                    new Claim("ID", loggedUser.ID.ToString()),
                    new Claim("Firstname", loggedUser.FirstName),
                    new Claim("Firstname2", user.FirstName2),
                    new Claim("Lastname", loggedUser.LastName),
                    new Claim("Lastname2", user.LastName2),
                    new Claim("Job", user.Job.JobName),
                    new Claim("CompanyID", user.Company.ID.ToString()),
                    new Claim("Photo", user.Photo),
                    new Claim(ClaimTypes.Email, loggedUser.Email),
                    new Claim(ClaimTypes.Role, loggedUser.Role.ToString())
                };
                var userIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
            }
            else
            {
                return View(loginDTO);
            }
            switch (loggedUser.Role)
            {
                case Roles.Admin:
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });

                case Roles.CompanyManager:                    
                    return RedirectToAction("Index", "CompanyManagerHome", new { Area = "CompanyManager" });


                case Roles.Employee:
                    return RedirectToAction("Index", "EmployeeHome", new { Area = "Employee" });
                default:
                    return View(loginDTO);
            }
        }
        
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync();            
            return RedirectToAction("Index", "Home");
        }
    }
}