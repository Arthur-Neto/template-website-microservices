using AutoMapper;
using Template.Domain.UsersModule;

namespace Template.Application.UsersModule.Commands
{
    public class UserCreateCommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserCreateCommandMapping : Profile
    {
        public UserCreateCommandMapping()
        {
            CreateMap<User, UserCreateCommand>()
                .ForMember(m => m.Username, opts => opts.MapFrom(src => src.Username))
                .ForMember(m => m.Password, opts => opts.MapFrom(src => src.Password))
                .ReverseMap();
        }
    }
}
