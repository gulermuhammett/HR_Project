using HR_Project.Entities.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HR_Project.UI.Areas.Admin.Models
{
    public class UpdateCompanyVM
    {
        public Company OriginalCompany { get; set; }
        public List<SelectListItem> TitleList { get; set; }
        public string LogoPath { get; set; }
    }
}
