using AutoMapper;
using HR_Project.Entities.Entities;
using HR_Project.UI.Areas.Admin.Models;
using HR_Project.UI.Models.DTOs;

namespace HR_Project.UI.Models
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateUserDTO,User>()
            .ForMember(dest => dest.CompanyID, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            CreateMap<User, UpdateUserDTO>();

            CreateMap<UpdateUserToSummaryInformationVM, User>()
            .ForMember(dest => dest.CompanyID, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            CreateMap<User, UpdateUserToSummaryInformationVM>();

        }
    }
}
