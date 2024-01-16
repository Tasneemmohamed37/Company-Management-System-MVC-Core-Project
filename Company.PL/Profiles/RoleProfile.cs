using AutoMapper;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Company.PL.Mappers
{
    public class RoleProfile:Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>()
                .ForMember(d => d.RoleName, O => O.MapFrom(s => s.Name)).ReverseMap();
        }
    }
}
