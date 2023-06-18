﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Entities.Entities
{
    public class BaseEntity
    {
        [Column(Order = 1)]      //bütün oluşacak entitylerde bu kolon 1. sırada olacak şekilde ayarlandı.
        public int ID { get; set; }
        public bool IsActive { get; set; }
    }
}
