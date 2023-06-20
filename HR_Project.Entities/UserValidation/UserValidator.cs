using FluentValidation;
using HR_Project.Entities.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HR_Project.Entities.UserValidator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Firstname can not be empty !");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Firstname can not be empty !");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address can not be empty !").MinimumLength(3).WithMessage("Address must be at least 3 character").MaximumLength(100).WithMessage("Address must be max 100 character");
            RuleFor(x => x.Birthdate).LessThan(p => DateTime.Now.AddYears(-18)).WithMessage("Employee must be at least 18 years old !");
            RuleFor(x => x.Salary).NotEmpty().WithMessage("Salary can not be empty !").GreaterThan(8500).WithMessage("Salary can not less than 8500.. It's minimum salary !");
            RuleFor(x => x.TCIdentificationNumber).NotEmpty().WithMessage("Identification Number can not be empty !");
            RuleFor(x => x.TCIdentificationNumber).Must(CheckTCKimlikNo).WithMessage("Invalid Idendification Number");
            RuleFor(x => x.JobID).NotEmpty().WithMessage("Job can not be empty.");
            RuleFor(x => x.DepartmentID).NotEmpty().WithMessage("Department can not be empty.");
            RuleFor(x => x.BirthPlace).NotNull().WithMessage("Birth Place can not be empty.").MaximumLength(50).WithMessage("Birth Place must not exceed 50 characters").MinimumLength(3).WithMessage("Birth lace must be more than 3 characters");
            RuleFor(x => x.TCIdentificationNumber).Must(BeUniqueIdentification).WithMessage("Identification must be unique.");

        }

        private bool BeUniqueIdentification(string identification)
        {
            string baseURL = "https://mezaapi-v11.azurewebsites.net";
            List<User> users = new List<User>();
            using (var httpClient = new HttpClient())
            {
                using (var result = httpClient.GetAsync($"{baseURL}/api/User/GetAllUserInclude").Result)
                {
                    string apiResult = result.Content.ReadAsStringAsync().Result;
                    users = JsonConvert.DeserializeObject<List<User>>(apiResult);
                }
            }
            List<User> controlUser = users.Where(x => x.TCIdentificationNumber == identification).ToList();
            if (controlUser.Count()>0)
            {
                return false;
            }
            return true;
        }

        private bool CheckTCKimlikNo(string tcKimlikNo)
        {
            tcKimlikNo = tcKimlikNo.ToString();

            if (!Regex.IsMatch(tcKimlikNo, "^[0-9]{11}$"))
                return false;

            int totalX = 0;
            for (int i = 0; i < 10; i++)
            {
                totalX += int.Parse(tcKimlikNo[i].ToString());
            }

            bool isRuleX = totalX % 10 == int.Parse(tcKimlikNo[10].ToString());

            int totalY1 = 0;
            int totalY2 = 0;
            for (int i = 0; i < 10; i += 2)
            {
                totalY1 += int.Parse(tcKimlikNo[i].ToString());
            }

            for (int i = 1; i < 10; i += 2)
            {
                totalY2 += int.Parse(tcKimlikNo[i].ToString());
            }

            bool isRuleY = ((totalY1 * 7) - totalY2) % 10 == 0;

            return isRuleX && isRuleY;
        }
    }
}
