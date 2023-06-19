using FluentValidation;
using HR_Project.Entities.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Entities.UserValidation
{
    public class AdvanceValidator : AbstractValidator<Advance>
    {
        //public AdvanceValidator()
        //{
        //    RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount can not be empty !");
        //    RuleFor(x => x.Amount).GreaterThan(x=> UserSalary(x.ID)*3).WithMessage("Requested amount is not valid !");
        //}


        //decimal? UserSalary(int id)
        //{
        //    string baseURL = "https://mezaapi-v11.azurewebsites.net";
        //    User user = new User();
        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var response = httpClient.GetAsync($"{baseURL}/api/User/GetUserByIdInclude/{id}").Result)
        //        {
        //            string apiResult = response.Content.ReadAsStringAsync().Result;
        //            user = JsonConvert.DeserializeObject<List<User>>(apiResult)[0];
        //        }
        //    }
        //    return Convert.ToDecimal(user.Salary);
        //}

    }
}
