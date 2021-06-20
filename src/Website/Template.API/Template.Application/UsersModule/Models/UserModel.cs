using AutoMapper;
using Template.Domain.UsersModule;

namespace Template.Application.UsersModule.Models
{
    public class UserModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class UserModelMapping : Profile
    {
        public UserModelMapping()
        {
            CreateMap<User, UserModel>()
                .ForMember(m => m.ID, opts => opts.MapFrom(src => src.ID))
                .ForMember(m => m.Name, opts => opts.MapFrom(src => src.Name))
                .ReverseMap();
        }
    }
}
