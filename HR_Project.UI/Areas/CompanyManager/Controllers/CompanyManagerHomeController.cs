﻿using HR_Project.Entities.Entities;
using HR_Project.Entities.Enums;
using IK.Ui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Policy;
using HR_Project.UI.Models.DTOs;
using System.Net.Http;
using HR_Project.Services.Abstract;
using AutoMapper;
using System.Net.Mail;
using HR_Project.UI.Areas.CompanyManager.Models;
using HR_Project.UI.Models;
using HR_Project.Repositories.Migrations;
using System.ComponentModel;
using HR_Project.UI.Areas.Admin.Models;
using Microsoft.Extensions.Hosting;

namespace HR_Project.UI.Areas.CompanyManagerArea.Controllers
{
    [Area("CompanyManager")]
    [Authorize(Roles = "CompanyManager")]
    public class CompanyManagerHomeController : Controller
    {
        string baseURL = "https://localhost:7253";
        private readonly IWebHostEnvironment _environment;
        private readonly IGenericService<User> service;
        private readonly IMapper mapper;



        public CompanyManagerHomeController(IWebHostEnvironment environment, IGenericService<User> service, IMapper mapper)
        {
            _environment = environment;
            this.service = service;
            this.mapper = mapper;
        }
        public async Task<IActionResult> ListUser()
        {
            List<User> users = new List<User>();

            using (var httpClient = new HttpClient())
            {
                using (var result = await httpClient.GetAsync($"{baseURL}/api/User/GetAllUserInclude"))
                {
                    string apiResult = await result.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<List<User>>(apiResult);
                }
            }
            List<User> empUsers = users.Where(x => x.Role == Roles.Employee && x.IsActive == true && x.Job != null && x.Department != null).ToList();

            return View(empUsers);
        }
        public async Task<IActionResult> SummaryInformation()
        {
            //var userClaimList = HttpContext.User.Claims.ToList();
            User user = new User();
            List<User> users = new List<User>();
            HashSet<int> departments = new HashSet<int>();
            HashSet<int> jobs = new HashSet<int>();
            HashSet<int> advancesPending = new HashSet<int>();
            HashSet<int> advancesCenceled = new HashSet<int>();
            HashSet<int> advancesConfirmed = new HashSet<int>();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/GetUserByIdInclude/{HttpContext.User.FindFirst("ID").Value}"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<List<User>>(apiResult)[0];
                }
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/GetAllUserInclude"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<List<User>>(apiResult);
                }
            }
            foreach (var item in users)
            {
                if (user.DepartmentID!=null)
                {
                    departments.Add((int)item.DepartmentID);
                }
                if (user.JobID != null)
                {
                    jobs.Add((int)item.JobID);
                }
                advancesPending.Add(item.Advances.Where(x=> x.Status == Status.Pending).ToList().Count());
                advancesCenceled.Add(item.Advances.Where(x=> x.Status == Status.Canceled).ToList().Count());
                advancesConfirmed.Add(item.Advances.Where(x=> x.Status == Status.Confirmed).ToList().Count());
            }
            ViewBag.UserCount = users.Where(x=>x.CompanyID==user.CompanyID).ToList().Count().ToString();
            ViewBag.DepartmentCount = departments.Count().ToString();
            ViewBag.JobCount = jobs.Count().ToString();
            ViewBag.AdvancesCount = advancesPending.Count().ToString();
            ViewBag.AdvancesCenceledCount = advancesCenceled.Count().ToString();
            ViewBag.AdvancesConfirmedCount = advancesConfirmed.Count().ToString();
            return View(user);
        }


        public async Task<IActionResult> Index()
        {            
            return View();
        }

        static List<Department> departments;
        static List<Job> jobs;

        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            ViewBag.GenderList = Enum.GetValues(typeof(Gender))
                         .Cast<Gender>()
                         .Select(g => new SelectListItem
                         {
                             Value = g.ToString(),
                             Text = g.ToString()
                         })
                         .ToList();
            departments = new();
            ViewBag.CityList = Enum.GetValues(typeof(City))
                         .Cast<City>()
                         .Select(g => new SelectListItem
                         {
                             Value = g.ToString(),
                             Text = g.ToString()
                         })
                         .ToList();
            departments = new();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/GetAllDepartment"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    departments = JsonConvert.DeserializeObject<List<Department>>(apiResult);
                }

            }
            jobs = new();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/GetAllJob"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    jobs = JsonConvert.DeserializeObject<List<Job>>(apiResult);
                }

            }

            var userDTo = new AddUserDTO()
            {
                JobList = jobs.Select(j => new SelectListItem { Value = j.ID.ToString(), Text = j.JobName }).ToList(),
                DepartmentList = departments.Select(d => new SelectListItem { Value = d.ID.ToString(), Text = d.DepartmentName }).ToList(),
            };
            ViewBag.AddUserDTO = userDTo;
            ViewBag.jobList = jobs;
            ViewBag.departmentList = departments;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user, List<IFormFile> files)
        {
            string returnedMessaage = Upload.ImageUpload(files, _environment, out bool imgresult);
            if (!ModelState.IsValid || imgresult==false)
            {
                ViewBag.GenderList = Enum.GetValues(typeof(Gender))
                         .Cast<Gender>()
                         .Select(g => new SelectListItem
                         {
                             Value = g.ToString(),
                             Text = g.ToString()
                         })
                         .ToList();
                ViewBag.CityList = Enum.GetValues(typeof(City))
                         .Cast<City>()
                         .Select(g => new SelectListItem
                         {
                             Value = g.ToString(),
                             Text = g.ToString()
                         })
                         .ToList();
                departments = new();
                using (var httpClient = new HttpClient())
                {
                    using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/GetAllDepartment"))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        departments = JsonConvert.DeserializeObject<List<Department>>(apiResult);
                    }

                }
                jobs = new();
                using (var httpClient = new HttpClient())
                {
                    using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/GetAllJob"))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        jobs = JsonConvert.DeserializeObject<List<Job>>(apiResult);
                    }
                }
                var userDTo = new AddUserDTO()
                {
                    JobList = jobs.Select(j => new SelectListItem { Value = j.ID.ToString(), Text = j.JobName }).ToList(),
                    DepartmentList = departments.Select(d => new SelectListItem { Value = d.ID.ToString(), Text = d.DepartmentName }).ToList(),
                };
                ViewBag.AddUserDTO = userDTo;
                ViewBag.jobList = jobs;
                ViewBag.departmentList = departments;
                ViewBag.PhotoMessage = returnedMessaage;
                ViewBag.EnumValues = Enum.GetValues(typeof(Roles));

                return View("AddUser");
            }
            user.Role = Roles.Employee;
            user.IsPasswordChange = false;
            user.CompanyID = Convert.ToInt32(HttpContext.User.FindFirst("CompanyID").Value);
            user.IsActive = true;
            if (user.FirstName2 == null)
                user.FirstName2 = "";
            user.Email = (user.FirstName.ToLower() + user.FirstName2.ToLower() + "." + user.LastName.ToLower() + "@bilgeadamboost.com").Replace("ü", "u").Replace("ö", "o").Replace("ı", "i").Replace("ş", "s").Replace("ç", "c").Replace("ğ", "g");
            user.Password = Password.GeneratePassword();
            

                user.Photo = returnedMessaage;//Eğer ImageUpload'dan fırlatılan değer true ise returnedMessage burda foto url'sini döndürcek
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                    using (var answ = await httpClient.PostAsync($"{baseURL}/api/User/CreateUser", content))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();

                    }

                }

                string emailBody = $"Hello {user.FirstName},\n\nWelcome to our company.\n\nYour registration to MEZA Human Resources Management System has been completed successfully. Please use the login link to login to your system.\n\nYour required information for login:\n\n Email: {user.Email},\n\nUser Password: {user.Password},\n\nURL:https://ciftcikemal.azurewebsites.net,\n\nWe wish you success in your working life. MEZA Human Resources.";
                EmailService.SendEmail(user.Email, "Welcome to MEZA Human Resources", emailBody);
                return RedirectToAction("ListUser");
           

        }
        
        static User updateduser;
        [HttpGet]
        public async Task<IActionResult> UserUpdate(int id)
        {

            departments = new List<Department>();
            jobs = new List<Job>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/GetUserById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updateduser = JsonConvert.DeserializeObject<User>(apiCevap);
                }

                using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/GetAllDepartment"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    departments = JsonConvert.DeserializeObject<List<Department>>(apiResult);
                }

                using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/GetAllJob"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    jobs = JsonConvert.DeserializeObject<List<Job>>(apiResult);
                }
            }

            ViewBag.GenderList = Enum.GetValues(typeof(Gender))
                         .Cast<Gender>()
                         .Select(g => new SelectListItem
                         {
                             Value = g.ToString(),
                             Text = g.ToString()
                         })
                         .ToList();

            var userDTo = new AddUserDTO()
            {
                JobList = jobs.Select(j => new SelectListItem { Value = j.ID.ToString(), Text = j.JobName }).ToList(),
                DepartmentList = departments.Select(d => new SelectListItem { Value = d.ID.ToString(), Text = d.DepartmentName }).ToList(),
            };
            ViewBag.AddUserDTO = userDTo;
            ViewBag.jobList = jobs;
            ViewBag.departmentList = departments;

            UpdateUserDTO updateUserDTO = mapper.Map<User, UpdateUserDTO>(updateduser);

            return View(updateUserDTO);
        }

        [HttpPost]
        public async Task<IActionResult> UserUpdate(UpdateUserDTO updateUserDTO, List<IFormFile> files)
        {
            string returnedMessaage = Upload.ImageUpload(files, _environment, out bool imgresult);
            if (!ModelState.IsValid || (imgresult==false && returnedMessaage!= "Dosya seçilmedi"))
            {
                departments = new List<Department>();
                jobs = new List<Job>();

                using (var httpClient = new HttpClient())
                {
                    using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/GetUserById/{updateUserDTO.ID}"))
                    {
                        string apiCevap = await cevap.Content.ReadAsStringAsync();
                        updateduser = JsonConvert.DeserializeObject<User>(apiCevap);
                    }

                    using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/GetAllDepartment"))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        departments = JsonConvert.DeserializeObject<List<Department>>(apiResult);
                    }

                    using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/GetAllJob"))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        jobs = JsonConvert.DeserializeObject<List<Job>>(apiResult);
                    }
                }

                ViewBag.GenderList = Enum.GetValues(typeof(Gender))
                             .Cast<Gender>()
                             .Select(g => new SelectListItem
                             {
                                 Value = g.ToString(),
                                 Text = g.ToString()
                             })
                             .ToList();

                var userDTo = new AddUserDTO()
                {
                    JobList = jobs.Select(j => new SelectListItem { Value = j.ID.ToString(), Text = j.JobName }).ToList(),
                    DepartmentList = departments.Select(d => new SelectListItem { Value = d.ID.ToString(), Text = d.DepartmentName }).ToList(),
                };
                ViewBag.AddUserDTO = userDTo;
                ViewBag.jobList = jobs;
                ViewBag.departmentList = departments;
                ViewBag.PhotoMessage = returnedMessaage;
                ViewBag.EnumValues = Enum.GetValues(typeof(Roles));

                UpdateUserDTO UserDTO = mapper.Map<User, UpdateUserDTO>(updateduser);
                return View(UserDTO);
            }
            


            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/GetUserByID/{updateUserDTO.ID}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updateduser = JsonConvert.DeserializeObject<User>(apiCevap);

                }
                if (imgresult)
                    updateduser.Photo = returnedMessaage;
                else if (returnedMessaage == "Dosya seçilmedi")
                    updateduser.Photo = updateduser.Photo;


                //updateduser = mapper.Map<UpdateUserDTO,User >(updateUserDTO);

                updateduser.FirstName = updateUserDTO.FirstName;
                updateduser.FirstName2 = updateUserDTO.FirstName2;
                updateduser.LastName = updateUserDTO.LastName;
                updateduser.LastName2 = updateUserDTO.LastName2;
                updateduser.Address = updateUserDTO.Address;
                updateduser.BirthPlace = updateUserDTO.BirthPlace;
                updateduser.PhoneNumber = updateUserDTO.PhoneNumber;
                updateduser.Birthdate = updateUserDTO.Birthdate;
                updateduser.StartDateOfWork = updateUserDTO.StartDateOfWork;
                updateduser.Salary = updateUserDTO.Salary;
                updateduser.TCIdentificationNumber = updateUserDTO.TCIdentificationNumber;
                updateduser.JobID = updateUserDTO.JobID;
                updateduser.DepartmentID = updateUserDTO.DepartmentID;


                StringContent content = new StringContent(JsonConvert.SerializeObject(updateduser), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{baseURL}/api/User/UpdateUser/{updateduser.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }

            if (updateduser.Role == Roles.CompanyManager)
            {
                return RedirectToAction("Index","CompanyManagerHome");
            }
            return RedirectToAction("ListUser");
        }

        public async Task<IActionResult> UserRemove(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{baseURL}/api/User/DeleteUser/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("ListUser");
        }

        private void SendEmail(string toEmail, string subject, string body)
        { // E-posta gönderme kodunu burada uygulayın // SmtpClient ve MailMessage sınıflarını kullanarak e-posta gönderme işlemi gerçekleştirilir // Örnek kullanım:
            using (var client = new SmtpClient())
            {
                var mailMessage = new MailMessage();
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body; // Diğer e-posta ayarları... client.Send(mailMessage); } 
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUserToSummaryInformation(int id)
        {

            User updateUserSummary = new();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/GetUserById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updateUserSummary = JsonConvert.DeserializeObject<User>(apiCevap);
                }
            }





            UpdateUserToSummaryInformationVM updateUserDTO = mapper.Map<User, UpdateUserToSummaryInformationVM>(updateUserSummary);

            return View("UpdateUserToSummaryInformation", updateUserDTO);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserToSummaryInformation(UpdateUserToSummaryInformationVM updateUserDTO, List<IFormFile> files)
        {
            string returnedMessaage = Upload.ImageUpload(files, _environment, out bool imgresult);
            if (!ModelState.IsValid || (imgresult == false && returnedMessaage != "Dosya seçilmedi"))
            {
                return View("UpdateUserToSummaryInformation");
            }


            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/GetUserByID/{updateUserDTO.ID}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updateduser = JsonConvert.DeserializeObject<User>(apiCevap);

                }
                if (imgresult)
                    updateduser.Photo = returnedMessaage;


                //updateduser = mapper.Map<UpdateUserDTO,User >(updateUserDTO);



                updateduser.Address = updateUserDTO.Address;
                updateduser.PhoneNumber = updateUserDTO.PhoneNumber;


                StringContent content = new StringContent(JsonConvert.SerializeObject(updateduser), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{baseURL}/api/User/UpdateUser/{updateduser.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");


        }
    }
}