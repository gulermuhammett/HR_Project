using FluentValidation;
using HR_Project.Entities.Entities;
using System.Text.RegularExpressions;

namespace IK.Ui.Models.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotNull().WithMessage("First Name can not be empty.").MaximumLength(50).WithMessage("FirstName must not exceed 50 characters").MinimumLength(10).WithMessage("First Name must be more than 10 characters");
            RuleFor(x => x.LastName).NotNull().WithMessage("Last Name can not be empty.").MaximumLength(50).WithMessage("LastName must not exceed 50 characters").MinimumLength(10).WithMessage("Last Name must be more than 10 characters");
            RuleFor(x => x.BirthPlace).NotNull().WithMessage("Birth Place can not be empty.").MaximumLength(50).WithMessage("Birth Place must not exceed 50 characters").MinimumLength(3).WithMessage("Birth lace must be more than 3 characters");
           
            RuleFor(x => x.CompanyID).NotNull().WithMessage("Company can not be empty.");
            RuleFor(x => x.JobID).NotNull().WithMessage("Job can not be empty.");
            RuleFor(x => x.StartDateOfWork).NotNull().WithMessage("Address can not be empty.");
            RuleFor(x => x.Birthdate).NotNull().WithMessage("Birthdate can not be empty.");
           
            RuleFor(x => x.Address).NotNull().WithMessage("Address can not be empty.").MaximumLength(50).WithMessage("Address must not exceed 50 characters").MinimumLength(10).WithMessage("Address must be more than 10 characters");
            RuleFor(x => x.Photo).NotNull().WithMessage("Photo can not be empty.");
            RuleFor(x => x.PhoneNumber).NotNull().WithMessage("Phone number can not be empty.").Length(11).WithMessage("Phone number must be 11 characters.").Matches(new Regex(@"^0[5-9][0-9]{9}$")).WithMessage("Phone number not valid");
        }
    }
}
