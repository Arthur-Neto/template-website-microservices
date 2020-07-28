using AutoMapper;
using Template.Domain.UsersModule;

namespace Template.Application.UsersModule.Commands
{
    public class UserUpdateCommand
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserUpdateCommandMapping : Profile
    {
        public UserUpdateCommandMapping()
        {
            CreateMap<User, UserUpdateCommand>()
                .ForMember(m => m.ID, opts => opts.MapFrom(src => src.ID))
                .ForMember(m => m.Username, opts => opts.MapFrom(src => src.Username))
                .ForMember(m => m.Password, opts => opts.MapFrom(src => src.Password))
                .ReverseMap();
        }
    }
}
