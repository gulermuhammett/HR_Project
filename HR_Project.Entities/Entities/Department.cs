using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Entities.Entities
{
    public class Department : BaseEntity
    {
        public Department()
        {
            Users = new List<User>();
            Jobs = new List<Job>();
        }
        public string DepartmentName { get; set; }

        //navigation properties
        public virtual List<User> Users { get; set; }
        public virtual List<Job> Jobs { get; set; }
        public Company? Company { get; set; }
        public int? CompanyID { get; set; }
    }

}
