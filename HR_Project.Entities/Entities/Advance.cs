using HR_Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Entities.Entities
{
    public class Advance : BaseEntity
    {
        public int UserID { get; set; }
        public virtual User? User { get; set; }
        public AdvanceType Type { get; set; } //bireysel,kurumsal
        public Currency Currency { get; set; } //para cinsi
        public Status Status { get; set; } //Avans işlem Durumu
        public double Amount { get; set; } //miktar

        public DateTime RequestDate { get; set; } = DateTime.Now; //Talep Tarihi
        public DateTime? TransactionDate { get; set; } //İşlem Tarihi

    }
}
