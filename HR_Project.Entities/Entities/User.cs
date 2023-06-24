
using HR_Project.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace HR_Project.Entities.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            Advances = new List<Advance>();
        }
        public virtual List<Advance> Advances { get; set; }

        [Required(ErrorMessage = "Please Enter First Name.")]
        public string? FirstName { get; set; }
        public string? FirstName2 { get; set; }
        [Required(ErrorMessage = "Please Enter First Surname.")]
        public string? LastName { get; set; }
        public string? LastName2 { get; set; }
        public string? Password { get; set; }

        public DateTime StartDateOfWork { get; set; } = DateTime.Now.Date;   //işe giriş

        private DateTime? dismissalDate; //işten çıkış
        public DateTime? DismissalDate { 
            get { return dismissalDate; } 
            set { dismissalDate = value; } }

        public string FormattedDismissalDate { 
            get { return dismissalDate.HasValue ? dismissalDate.Value.ToString("dd/MM/yyyy") : string.Empty; } 
        }     
        private DateTime? birthDate; 
        public DateTime? Birthdate
        {
            get { return birthDate; }
            set { birthDate = value; }
        }

        public string FormattedBirthdate
        {
            get { return birthDate.HasValue ? birthDate.Value.ToString("dd/MM/yyyy") : string.Empty; }
        }

        public string? BirthPlace { get; set; }                         //Doğum yeri
        public string? Email { get; set; }
        public string? PhoneNumber { get ; set; }
        public Gender? Gender { get; set; }
        public Roles? Role { get; set; }
        public City? City { get; set; } = Enums.City.İSTANBUL;
        public string? Address { get; set; }
        public decimal? Salary { get; set; }   //maaş
        public string? Photo { get; set; }

        public string? TCIdentificationNumber { get; set; }

        public bool? IsPasswordChange { get; set; } = false;


        //navigation properties

        public int? JobID { get; set; }
        public Job? Job { get; set; }
        public int? DepartmentID { get; set; }
        public Department? Department { get; set; }
        public int? CompanyID { get; set; }
        public Company? Company { get; set; }
        
    }
}
