using HR_Project.Entities.Entities;
using HR_Project.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HR_Project.UI.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager")]
    [Authorize(Roles = "CompanyManager")]
    public class AdvanceController : Controller
    {
        string baseURL = "https://mezaapi-v11.azurewebsites.net";
        static List<Advance> advances = new List<Advance>();

        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/GetAllCompanyAdvances/{HttpContext.User.FindFirst("ID").Value}"))
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
        public async Task<IActionResult> AdvanceApprove(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/ApproveAdvance/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AdvanceCancel(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/CancelAdvance/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> AdvanceHold(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/HoldAdvance/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
    }
}
