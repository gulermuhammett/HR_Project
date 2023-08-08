using HR_Project.Entities.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HR_Project.UI.Models.DTOs
{
    public class AddUserDTO
    {

        public IEnumerable<SelectListItem> JobList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem>? CompanyList { get; set; }

    }
}