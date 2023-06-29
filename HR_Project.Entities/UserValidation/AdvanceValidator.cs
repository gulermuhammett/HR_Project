using FluentValidation;
using HR_Project.Entities.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static ServiceStack.Diagnostics.Events;
using HttpClient = System.Net.Http.HttpClient;

namespace HR_Project.Entities.UserValidation
{
    public class AdvanceValidator : AbstractValidator<Advance>
    {
        public AdvanceValidator()
        {                   
            RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount can not be empty !");
            //RuleFor(x => x.Amount).LessThan(x => Convert.ToDouble(UserSalary() * 3)).WithMessage("Requested amount is not valid !");
        }
            
        //decimal? UserSalary(int id)
        //{
        //    string baseURL = "https://mezaapi-v12.azurewebsites.net";
        //    User users = new();
        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var result = httpClient.GetAsync($"{baseURL}/api/User/GetUserByIdInclude/{id}").Result)
        //        {
        //            string apiResult = result.Content.ReadAsStringAsync().Result;
        //            users = JsonConvert.DeserializeObject<List<User>>(apiResult)[0];
        //        }
        //    }
        //    return Convert.ToDecimal(users.Salary);
        //}

    }
}
