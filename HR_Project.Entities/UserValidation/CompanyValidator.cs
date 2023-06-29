using FluentValidation;
using HR_Project.Entities.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Entities.UserValidation
{
    public class CompanyValidator : AbstractValidator<Company>
    {


        public CompanyValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty().WithMessage("Company Name can not be empty !").MaximumLength(50).WithMessage("Company Name must not exceed 50 characters");
            RuleFor(x => x.CompanyTitle).NotEmpty().WithMessage("Company Title can not be empty !");
            RuleFor(x => x.MersisNo).NotEmpty().WithMessage("Mersis No Name can not be empty !");
            RuleFor(x => x.TaxNumber).NotEmpty().WithMessage("Tax Number can not be empty !");
            RuleFor(x => x.TaxOffice).NotEmpty().WithMessage("Tax Office can not be empty !");
            RuleFor(x => x.YearOfFoundation).NotEmpty().WithMessage("Year Of Foundation can not be empty !");
            RuleFor(x => x.NumberOfEmployees).NotEmpty().WithMessage("Number Of Employees can not be empty !");
            RuleFor(x => x.Logo).NotEmpty().WithMessage("Tax Office can not be empty !");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Logo can not be empty !");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address can not be empty !").MinimumLength(3).WithMessage("Address must be at least 3 character").MaximumLength(100).WithMessage("Address must be max 100 character");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email can not be empty !");
            RuleFor(x => x.ContractStartDate).NotEmpty().WithMessage("Contract Start Date can not be empty !");
            RuleFor(x => x.ContractFinishDate).NotEmpty().WithMessage("Contract Finish Date can not be empty !");



            RuleFor(x => x.MersisNo).Must(BeUniqueMersisNo).WithMessage("Mersis No must be unique.");
            RuleFor(x => x.TaxNumber).Must(BeUniqueTaxNumber).WithMessage("Tax Number must be unique.");
        }

        private bool BeUniqueMersisNo(string mersisNo)
        {
            string baseURL = "https://localhost:7253";
            List<Company> company = new List<Company>();
            using (var httpClient = new HttpClient())
            {
                using (var result = httpClient.GetAsync($"{baseURL}/api/User/GetAllUserInclude").Result)
                {
                    string apiResult = result.Content.ReadAsStringAsync().Result;
                    company = JsonConvert.DeserializeObject<List<Company>>(apiResult);
                }
            }
            List<Company> controlCompany = company.Where(x => x.MersisNo == mersisNo).ToList();
            if (controlCompany.Count() > 0)
            {
                return false;
            }
            return true;
        }

        private bool BeUniqueTaxNumber(string taxNumber)
        {
            string baseURL = "https://localhost:7253";
            List<Company> company = new List<Company>();
            using (var httpClient = new HttpClient())
            {
                using (var result = httpClient.GetAsync($"{baseURL}/api/User/GetAllUserInclude").Result)
                {
                    string apiResult = result.Content.ReadAsStringAsync().Result;
                    company = JsonConvert.DeserializeObject<List<Company>>(apiResult);
                }
            }
            List<Company> controlCompany = company.Where(x => x.TaxNumber == taxNumber).ToList();
            if (controlCompany.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}
