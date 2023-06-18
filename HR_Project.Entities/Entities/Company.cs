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

        //navigation properties
        public virtual List<User> Users { get; set; }
        public virtual List<Department> Departments { get; set; }

    }
}
