using AutoMapper;
using Template.Domain.TenantsModule;

namespace Template.Application.TenantsModule.Models
{
    public class AuthenticatedTenantModel
    {
        public string ID { get; set; }
        public string Logon { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }

    public class AuthenticatedTenantModelMapping : Profile
    {
        public AuthenticatedTenantModelMapping()
        {
            CreateMap<Tenant, AuthenticatedTenantModel>()
                .ForMember(m => m.ID, opts => opts.MapFrom(src => src.ID))
                .ForMember(m => m.Logon, opts => opts.MapFrom(src => src.Logon))
                .ForMember(m => m.Role, opts => opts.MapFrom(src => src.Role))
                .ForMember(m => m.Token, opts => opts.MapFrom(src => src.Token))
                .ReverseMap();
        }
    }
}
