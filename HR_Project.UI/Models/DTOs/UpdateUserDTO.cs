using HR_Project.Entities.Entities;
using HR_Project.Entities.Enums;
using System;

namespace HR_Project.UI.Models.DTOs
{
    public class UpdateUserDTO
    {
        public int ID { get; set; }
        public string? FirstName { get; set; }
        public string? FirstName2 { get; set; }
        
        public string? LastName { get; set; }
        public string? LastName2 { get; set; }
        public City? City { get; set; } 

        public DateTime StartDateOfWork { get; set; } = DateTime.Now.Date;   //işe giriş

        private DateTime? dismissalDate; //işten çıkış
        public DateTime? DismissalDate
        {
            get { return dismissalDate; }
            set { dismissalDate = value; }
        }

        public string FormattedDismissalDate
        {
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
        
        public string? PhoneNumber { get; set; }

        public Gender? Gender { get; set; }
        public string? Address { get; set; }
        public decimal? Salary { get; set; }   //maaş
        public string? Photo { get; set; }

        private string? identificationNumber;

        public string? TCIdentificationNumber
        {
            get { return identificationNumber; }
            set
            {
                // Girilen değerin sadece rakamlardan oluştuğunu ve 11 haneli olmasını kontrol ediyoruz
                if (!string.IsNullOrEmpty(value) && value.Length == 11 && value.All(char.IsDigit))
                {
                    identificationNumber = value;
                }
                else
                {

                    throw new ArgumentException("TR ID number must be 11 digits and consist of numbers only.");//TC Kimlik numarası 11 haneli ve sadece rakamlardan oluşmalıdır
                }
            }
        }

        

        //navigation properties

        public int? JobID { get; set; }
        public Job? Job { get; set; }
        public int? DepartmentID { get; set; }
        public Department? Department { get; set; }
        public int? CompanyID { get; set; }
        public Company? Company { get; set; }
        
    }
}
