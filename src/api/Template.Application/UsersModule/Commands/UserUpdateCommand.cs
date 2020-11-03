using AutoMapper;
using FluentValidation;
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
            CreateMap<UserUpdateCommand, User>()
                .ForMember(m => m.ID, opts => opts.MapFrom(src => src.ID))
                .ForMember(m => m.Username, opts => opts.MapFrom(src => src.Username))
                .ForMember(m => m.Password, opts => opts.MapFrom(src => src.Password));
        }
    }

    public class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
    {
        public UserUpdateCommandValidator()
        {
            RuleFor(x => x.ID).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Username).NotEmpty().Length(1, 50);
            RuleFor(x => x.Password).NotEmpty().Length(1, 50);
        }
    }
}
