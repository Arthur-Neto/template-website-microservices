using AutoMapper;
using Template.Domain.UsersModule;

namespace Template.Application.UsersModule.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }

    public class UserModelMapping : Profile
    {
        public UserModelMapping()
        {
            CreateMap<User, UserModel>()
                .ForMember(m => m.ID, opts => opts.MapFrom(src => src.ID))
                .ForMember(m => m.Username, opts => opts.MapFrom(src => src.Username))
                .ForMember(m => m.Role, opts => opts.MapFrom(src => src.Role))
                .ReverseMap();
        }
    }
}
