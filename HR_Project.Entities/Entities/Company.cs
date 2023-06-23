using HR_Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Entities.Entities
{
    public class Company : BaseEntity
    {
        public Company()
        {
            Users = new List<User>();
            Departments = new List<Department>();
        }
        public string CompanyName { get; set; }
        public CompanyTitle? CompanyTitle { get; set; } //ünvan
        public string? MersisNo { get; set; }
        public string? TaxNumber { get; set; }
        public string? TaxOffice { get; set; }
        public string? Logo { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public int? NumberOfEmployees { get; set; }
        public DateTime? YearOfFoundation { get; set; }   //kuruluş tarihi
        public DateTime? ContractStartDate { get; set; }   //sözleşme başlangıç tarihi
        public DateTime? ContractFinishDate { get; set; }   //sözleşme bitiş tarihi

        //navigation properties
        public virtual List<User> Users { get; set; }
        public virtual List<Department> Departments { get; set; }

    }
}
