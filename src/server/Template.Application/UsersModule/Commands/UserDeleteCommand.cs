using AutoMapper;
using Template.Domain.UsersModule;

namespace Template.Application.UsersModule.Commands
{
    public class UserDeleteCommand
    {
        public int ID { get; set; }
    }

    public class UserDeleteCommandMapping : Profile
    {
        public UserDeleteCommandMapping()
        {
            CreateMap<User, UserDeleteCommand>()
                .ForMember(m => m.ID, opts => opts.MapFrom(src => src.ID))
                .ReverseMap();
        }
    }
}
