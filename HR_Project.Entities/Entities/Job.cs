using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Entities.Entities
{
    public class Job : BaseEntity
    {
        public Job()
        {
            Users = new List<User>();
        }
        public string JobName { get; set; }

        //navigation properties
        public virtual List<User> Users { get; set; }

        public Department? Department { get; set; }

        public int? DepartmentID { get; set; }
    }
}
