using AutoMapper;
using HR_Project.Entities.Entities;
using HR_Project.Entities.Enums;
using HR_Project.Services.Abstract;
using HR_Project.UI.Areas.Admin.Models;
using HR_Project.UI.Areas.CompanyManager.Models;
using HR_Project.UI.Models;
using HR_Project.UI.Models.DTOs;
using IK.Ui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Security.Cryptography;
using System.Text;

namespace HR_Project.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminHomeController : Controller
    {
        public AdminHomeController(IWebHostEnvironment environment, IGenericService<User> service, IMapper mapper)
        {
            this.environment = environment;
            this.service = service;
            this.mapper = mapper;
        }
        string baseURL = "https://localhost:7253";
        private readonly IWebHostEnvironment environment;
        private readonly IGenericService<User> service;
        private readonly IMapper mapper;

        public async Task<IActionResult> Index()
        {
            //var userClaimList = HttpContext.User.Claims.ToList();
            User user = new User();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/GetUserByIdInclude/{HttpContext.User.FindFirst("ID").Value}"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<List<User>>(apiResult)[0];
                }
            }
            return View(user);
        }

        public async Task<IActionResult> SummaryInformation()
        {
            //var userClaimList = HttpContext.User.Claims.ToList();
            User user = new User();
            List<User> users = new List<User>();
            HashSet<int> departments = new HashSet<int>();
            HashSet<int> jobs = new HashSet<int>();
            HashSet<int> advances = new HashSet<int>();
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
                if (user.DepartmentID != null)
                {
                    departments.Add((int)item.DepartmentID);
                }
                if (user.JobID != null)
                {
                    jobs.Add((int)item.JobID);
                }
                advances.Add(item.Advances.Where(x => x.Status == Status.Pending).ToList().Count());
            }
            ViewBag.UserCount = users.Count().ToString();
            ViewBag.DepartmentCount = departments.Count().ToString();
            ViewBag.JobCount = jobs.Count().ToString();
            ViewBag.AdvancesCount = advances.Count().ToString();
            return View(user);
        }

        public async Task<IActionResult> ListCompany()
        {
            List<Company> companies = new List<Company>();

            using (var httpClient = new HttpClient())
            {
                using (var result = await httpClient.GetAsync($"{baseURL}/api/Company/GetAllCompanies"))
                {
                    string apiResult = await result.Content.ReadAsStringAsync();
                    companies = JsonConvert.DeserializeObject<List<Company>>(apiResult);
                }
            }
            var companiess = companies.Where(x => x.IsActive == true).ToList();

            return View(companiess);
        }

        Dictionary<CompanyTitle, string> enumDisplayNames = new Dictionary<CompanyTitle, string>
        {
            { CompanyTitle.AnonimSirket, "Anonim Şirket" },
            { CompanyTitle.LimitedSirket, "Limited Şirket" }
        };

        [HttpGet]
        public async Task<IActionResult> AddCompany()
        {
            ViewBag.TitleList = Enum.GetValues(typeof(CompanyTitle))
             .Cast<CompanyTitle>()
             .Select(g => new SelectListItem
             {
                 Value = g.ToString(),
                 Text = enumDisplayNames.ContainsKey(g) ? enumDisplayNames[g] : g.ToString()
             })
             .ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(Company company, List<IFormFile> files)
        {
            string returnedMessaage = Upload.ImageUpload(files, environment, out bool imgresult);

            if (!ModelState.IsValid || (imgresult == false && returnedMessaage != "Dosya seçilmedi"))
            {
                ViewBag.TitleList = Enum.GetValues(typeof(CompanyTitle))
             .Cast<CompanyTitle>()
             .Select(g => new SelectListItem
             {
                 Value = g.ToString(),
                 Text = enumDisplayNames.ContainsKey(g) ? enumDisplayNames[g] : g.ToString()
             })
             .ToList();
                return View("AddCompany");
            }

            company.IsActive = true;
            if (imgresult)
            {
                company.Logo = returnedMessaage;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json");
                    using (var answ = await httpClient.PostAsync($"{baseURL}/api/Company/CreateCompany", content))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                    }
                }
            }
            return RedirectToAction("ListCompany");
        }

        static List<Department> departments;
        static List<Job> jobs;
        static List<Company> companies;
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
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/GetAllCompanies"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    companies = JsonConvert.DeserializeObject<List<Company>>(apiResult);
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
            ViewBag.companyList = companies;

            UpdateUserDTO updateUserDTO = mapper.Map<User, UpdateUserDTO>(updateduser);

            return View(updateUserDTO);
        }

        [HttpPost]
        public async Task<IActionResult> UserUpdate(UpdateUserDTO updateUserDTO, List<IFormFile> files)
        {
            if (!ModelState.IsValid)
            {
                return View("UserUpdate");
            }
            string returnedMessaage = Upload.ImageUpload(files, environment, out bool imgresult);

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
                updateduser.CompanyID = updateUserDTO.CompanyID;


                StringContent content = new StringContent(JsonConvert.SerializeObject(updateduser), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{baseURL}/api/User/UpdateUser/{updateduser.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }

            if (updateduser.Role == Roles.Admin)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("ListCompany");
        }

        public async Task<IActionResult> CompanyRemove(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{baseURL}/api/Company/DeleteCompany/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("ListCompany");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCompany(int id)
        {
            var company = new Company();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{baseURL}/api/Company/GetCompanyByID/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    company = JsonConvert.DeserializeObject<Company>(apiResponse);
                }
            }

            ViewBag.TitleList = Enum.GetValues(typeof(CompanyTitle))
                        .Cast<CompanyTitle>()
                        .Select(g => new SelectListItem
                        {
                            Value = g.ToString(),
                            Text = g.ToString()
                        })
                        .ToList();


            return View(company);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCompany(Company company, List<IFormFile> files)
        {
            string returnedMessaage = Upload.ImageUpload(files, environment, out bool imgresult);

            if (!ModelState.IsValid || (imgresult == false && returnedMessaage != "Dosya seçilmedi"))
            {
                ViewBag.TitleList = Enum.GetValues(typeof(CompanyTitle))
                         .Cast<CompanyTitle>()
                         .Select(g => new SelectListItem
                         {
                             Value = g.ToString(),
                             Text = g.ToString()
                         })
                         .ToList();

                ViewBag.PhotoMessage = returnedMessaage;
                ViewBag.EnumValues = Enum.GetValues(typeof(Roles));
                return View("UpdateCompany");
            }
            var updatedCompany = new Company();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Company/GetCompanyByID/{company.ID}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedCompany = JsonConvert.DeserializeObject<Company>(apiCevap);
                }
            }

            updatedCompany.CompanyName = company.CompanyName;
            updatedCompany.CompanyTitle = company.CompanyTitle;
            updatedCompany.MersisNo = company.MersisNo;
            updatedCompany.TaxNumber = company.TaxNumber;
            updatedCompany.TaxOffice = company.TaxOffice;
            updatedCompany.PhoneNumber = company.PhoneNumber;
            updatedCompany.Address = company.Address;
            updatedCompany.Email = company.Email;
            updatedCompany.NumberOfEmployees = company.NumberOfEmployees;
            updatedCompany.YearOfFoundation = company.YearOfFoundation;
            updatedCompany.ContractStartDate = company.ContractStartDate;
            updatedCompany.ContractFinishDate = company.ContractFinishDate;
            updatedCompany.IsActive = true;

            if (imgresult)
            {
                updatedCompany.Logo = returnedMessaage;
            }

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(updatedCompany), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync($"{baseURL}/api/Company/UpdateCompany/{updatedCompany.ID}", content))
                {
                    string apiResult = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("ListCompany");
        }


        [HttpGet]
        public async Task<IActionResult> AddCompanyManager()
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

            companies = new();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Company/GetAllCompanies"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    companies = JsonConvert.DeserializeObject<List<Company>>(apiResult);
                }
            }

            var userDTo = new AddUserDTO()
            {
                JobList = jobs.Select(j => new SelectListItem { Value = j.ID.ToString(), Text = j.JobName }).ToList(),
                DepartmentList = departments.Select(d => new SelectListItem { Value = d.ID.ToString(), Text = d.DepartmentName }).ToList(),
                CompanyList = companies.Select(d => new SelectListItem { Value = d.ID.ToString(), Text = d.CompanyName }).ToList(),
            };
            ViewBag.AddUserDTO = userDTo;
            ViewBag.jobList = jobs;
            ViewBag.departmentList = departments;
            ViewBag.companylist = companies;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanyManager(User user, List<IFormFile> files)
        {
            string returnedMessaage = Upload.ImageUpload(files, environment, out bool imgresult);

            if (!ModelState.IsValid || imgresult == false)
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

                companies = new();
                using (var httpClient = new HttpClient())
                {
                    using (var answ = await httpClient.GetAsync($"{baseURL}/api/Company/GetAllCompanies"))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        companies = JsonConvert.DeserializeObject<List<Company>>(apiResult);
                    }
                }

                var userDTo = new AddUserDTO()
                {
                    JobList = jobs.Select(j => new SelectListItem { Value = j.ID.ToString(), Text = j.JobName }).ToList(),
                    DepartmentList = departments.Select(d => new SelectListItem { Value = d.ID.ToString(), Text = d.DepartmentName }).ToList(),
                    CompanyList = companies.Select(d => new SelectListItem { Value = d.ID.ToString(), Text = d.CompanyName }).ToList(),
                };
                ViewBag.AddUserDTO = userDTo;
                ViewBag.jobList = jobs;
                ViewBag.departmentList = departments;
                ViewBag.companyList = companies;
                ViewBag.PhotoMessage = returnedMessaage;
                ViewBag.EnumValues = Enum.GetValues(typeof(Roles));
                return View("AddCompanyManager");
            }
            user.Role = Roles.CompanyManager;
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

            string emailBody = $"Hello {user.FirstName},\n\nWelcome to our company.\n\nYour registration to MEZA Human Resources Management System has been completed successfully. Please use the login link to login to your system.\n\nYour required information for login:\n\n Email: {user.Email},\n\nUser Password: {user.Password} \n\nURL:https://ciftcikemal.azurewebsites.net,\n\nWe wish you success in your working life. MEZA Human Resources.";
            EmailService.SendEmail(user.Email, "Welcome to MEZA Human Resources", emailBody);
            return RedirectToAction("ListCompanyManager");



        }

        public async Task<IActionResult> ListCompanyManager()
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
            List<User> empUsers = users.Where(x => x.Role == Roles.CompanyManager && x.IsActive == true && x.Job != null && x.Department != null).ToList();

            return View(empUsers);
        }

        public async Task<IActionResult> CompanyManagerRemove(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{baseURL}/api/User/DeleteUser/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("ListCompanyManager");
        }


        static User updateCompanyManager;
        [HttpGet]
        public async Task<IActionResult> CompanyManagerUpdate(int id)
        {

            departments = new List<Department>();
            jobs = new List<Job>();
            companies = new List<Company>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/GetUserById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updateCompanyManager = JsonConvert.DeserializeObject<User>(apiCevap);
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

                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Company/GetAllCompanies"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    companies = JsonConvert.DeserializeObject<List<Company>>(apiResult);
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
            ViewBag.CityList = Enum.GetValues(typeof(City))
                         .Cast<City>()
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
            ViewBag.companyList = companies;

            UpdateUserDTO updateUserDTO = mapper.Map<User, UpdateUserDTO>(updateCompanyManager);

            return View(updateUserDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CompanyManagerUpdate(UpdateUserDTO updateUserDTO, List<IFormFile> files)
        {
            string returnedMessaage = Upload.ImageUpload(files, environment, out bool imgresult);

            if (!ModelState.IsValid || (imgresult == false && returnedMessaage == "Dosya seçilmedi"))
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

                companies = new();
                using (var httpClient = new HttpClient())
                {
                    using (var answ = await httpClient.GetAsync($"{baseURL}/api/Company/GetAllCompanies"))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        companies = JsonConvert.DeserializeObject<List<Company>>(apiResult);
                    }
                }

                var userDTo = new AddUserDTO()
                {
                    JobList = jobs.Select(j => new SelectListItem { Value = j.ID.ToString(), Text = j.JobName }).ToList(),
                    DepartmentList = departments.Select(d => new SelectListItem { Value = d.ID.ToString(), Text = d.DepartmentName }).ToList(),
                    CompanyList = companies.Select(d => new SelectListItem { Value = d.ID.ToString(), Text = d.CompanyName }).ToList(),
                };
                ViewBag.AddUserDTO = userDTo;
                ViewBag.jobList = jobs;
                ViewBag.departmentList = departments;
                ViewBag.companyList = companies;
                ViewBag.PhotoMessage = returnedMessaage;
                ViewBag.EnumValues = Enum.GetValues(typeof(Roles));
                return View("CompanyManagerUpdate");
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{baseURL}/api/User/GetUserByID/{updateUserDTO.ID}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    updateCompanyManager = JsonConvert.DeserializeObject<User>(apiResponse);
                }
                if (imgresult)
                    updateCompanyManager.Photo = returnedMessaage;


                //updateduser = mapper.Map<UpdateUserDTO,User >(updateUserDTO);

                updateCompanyManager.FirstName = updateUserDTO.FirstName;
                updateCompanyManager.FirstName2 = updateUserDTO.FirstName2;
                updateCompanyManager.LastName = updateUserDTO.LastName;
                updateCompanyManager.LastName2 = updateUserDTO.LastName2;
                updateCompanyManager.Address = updateUserDTO.Address;
                updateCompanyManager.BirthPlace = updateUserDTO.BirthPlace;
                updateCompanyManager.PhoneNumber = updateUserDTO.PhoneNumber;
                updateCompanyManager.Birthdate = updateUserDTO.Birthdate;
                updateCompanyManager.StartDateOfWork = updateUserDTO.StartDateOfWork;
                updateCompanyManager.Salary = updateUserDTO.Salary;
                updateCompanyManager.TCIdentificationNumber = updateUserDTO.TCIdentificationNumber;
                updateCompanyManager.JobID = updateUserDTO.JobID;
                updateCompanyManager.DepartmentID = updateUserDTO.DepartmentID;


                StringContent content = new StringContent(JsonConvert.SerializeObject(updateCompanyManager), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{baseURL}/api/User/UpdateUser/{updateUserDTO.ID}", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                }
            }

            return RedirectToAction("ListCompanyManager");
        }
    }
}
