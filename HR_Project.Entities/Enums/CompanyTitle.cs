using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HR_Project.Entities.Enums
{
    public enum CompanyTitle
    {
        [Display(Name = "Anonymous Company")]
        AnonimSirket = 1,
        [Display(Name = "Limited Company")]
        LimitedSirket = 2,
    }
}
