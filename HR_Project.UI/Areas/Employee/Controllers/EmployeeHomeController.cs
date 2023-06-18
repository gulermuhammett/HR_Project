using AutoMapper;
using HR_Project.Entities.Entities;
using HR_Project.Entities.Enums;
using HR_Project.Services.Abstract;
using HR_Project.UI.Models.DTOs;
using IK.Ui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Security.Policy;
using System.Text;

namespace HR_Project.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    //[Authorize(Roles = "Employee")]
    public class EmployeeHomeController : Controller
    {
        string baseURL = "https://localhost:7253";
        private readonly IWebHostEnvironment _environment;
        private readonly IGenericService<User> service;
        private readonly IMapper mapper;

        public EmployeeHomeController(IWebHostEnvironment environment, IGenericService<User> service, IMapper mapper)
        {
            _environment = environment;
            this.service = service;
            this.mapper = mapper;
        }
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


        static List<Advance> advances = new List<Advance>();
        public async Task<IActionResult> AdvanceIndex()
        {



            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/GetAllAdvances/{HttpContext.User.FindFirst("ID").Value}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    advances = JsonConvert.DeserializeObject<List<Advance>>(apiCevap);
                }
            }
            if (advances != null)
                return View(advances);
            else
                return View();
        }

        static User updateduser;
        [HttpGet]
        public async Task<IActionResult> UserUpdate(int id)
        {

            List<Department> departments = new List<Department>();
            List<Job> jobs = new List<Job>();

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
            if (!ModelState.IsValid)
            {
                return View("UserUpdate");
            }
            string returnedMessaage = Upload.ImageUpload(files, _environment, out bool imgresult);

            //updateduser.FirstName = updateUserDTO.FirstName;
            //updateduser.FirstName2 = updateUserDTO.FirstName2;
            //updateduser.LastName = updateUserDTO.FirstName2;
            //updateduser.BirthPlace = updateUserDTO.BirthPlace;
            //updateduser.PhoneNumber = updateUserDTO.PhoneNumber;
            //updateduser.Photo = returnedMessaage;
            //updateduser. = updateUserDTO.FirstName;

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


                StringContent content = new StringContent(JsonConvert.SerializeObject(updateduser), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{baseURL}/api/User/UpdateUser/{updateduser.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> AdvanceApprove()
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/ApproveAdvance/{HttpContext.User.FindFirst("ID").Value}"))
                {

                }
            }
            return RedirectToAction("AdvanceIndex");
        }

        [HttpGet]
        public async Task<IActionResult> AdvanceCancel(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/CancelAdvance/{HttpContext.User.FindFirst("ID").Value}"))
                {

                }
            }
            return RedirectToAction("AdvanceIndex");
        }
        [HttpGet]
        public async Task<IActionResult> AdvanceHold(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/HoldAdvance/{HttpContext.User.FindFirst("ID").Value}"))
                {

                }
            }
            return RedirectToAction("AdvanceIndex");
        }

        static List<Advance> advanceControl = new List<Advance>();
        bool advanceIndividual = true;
        bool advanceInstitutional = true;

        [HttpGet]
        public async Task<IActionResult> AdvanceCreate()
        {
            ViewBag.CurrencyList = Enum.GetValues(typeof(Currency))
                         .Cast<Currency>()
                         .Select(g => new SelectListItem
                         {
                             Value = g.ToString(),
                             Text = g.ToString()
                         })
                         .ToList();



            if (advances != null)
            {
                foreach (var item in advances)
                {
                    if (item.Type == AdvanceType.Individual && item.Status == Status.Pending)
                        advanceIndividual = false;
                    else if (item.Type == AdvanceType.Institutional && item.Status == Status.Pending)
                        advanceInstitutional = false;
                }
            }

            if (advanceInstitutional == true && advanceIndividual == true)
            {
                ViewBag.AdvanceTypeList = Enum.GetValues(typeof(AdvanceType))
                             .Cast<AdvanceType>()
                             .Select(g => new SelectListItem
                             {
                                 Value = g.ToString(),
                                 Text = g.ToString()
                             })
                             .ToList();
            }
            else if (advanceIndividual && !advanceInstitutional)
            {
                ViewBag.AdvanceTypeList = new List<SelectListItem>
                {
                    new SelectListItem { Value = AdvanceType.Individual.ToString(), Text = AdvanceType.Individual.ToString() }
                };
            }
            else if (!advanceIndividual && advanceInstitutional)
            {
                ViewBag.AdvanceTypeList = new List<SelectListItem>
                {
                    new SelectListItem { Value = AdvanceType.Institutional.ToString(), Text = AdvanceType.Institutional.ToString() }
                };
            }

            else
            {
                ViewBag.ErrorMessage = "You cannot create a new advance request unless your existing advance requests are processed by the admin.";
            }

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> AdvanceCreate(Advance advance)
        {
            if (advanceIndividual == false && advanceInstitutional == false)
            {

            }
            else
            {

                using (var httpClient = new HttpClient())
                {
                    advance.UserID = Convert.ToInt32(HttpContext.User.FindFirst("ID").Value);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(advance), Encoding.UTF8, "application/json");
                    using (var answ = await httpClient.PostAsync($"{baseURL}/api/User/CreateAdvance", content))
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();

                    }

                }

            }

            return RedirectToAction("AdvanceIndex");
        }

        [HttpGet]
        public async Task<IActionResult> AdvanceDelete(int id)
        {

            using (var httpClient = new HttpClient())
            {

                using (var answ = await httpClient.GetAsync($"{baseURL}/api/User/DeleteAdvance/{id}"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();

                }

            }
            return RedirectToAction("AdvanceIndex");
        }
    }
}
