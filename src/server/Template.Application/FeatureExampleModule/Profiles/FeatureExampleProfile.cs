using AutoMapper;
using Template.Application.FeatureExampleModule.Models.Commands;
using Template.Domain.FeatureExampleModule;

namespace Template.Application.FeatureExampleModule.Profiles
{
    public class FeatureExampleProfile : Profile
    {
        public FeatureExampleProfile()
        {
            CreateMap<FeatureExampleAddCommand, FeatureExample>()
                .ForMember(fe => fe.FeatureExampleType, fe => fe.MapFrom(cmd => cmd.FeatureExampleType));

            CreateMap<FeatureExampleUpdateCommand, FeatureExample>()
                .ForMember(fe => fe.ID, fe => fe.MapFrom(cmd => cmd.ID))
                .ForMember(fe => fe.FeatureExampleType, fe => fe.MapFrom(cmd => cmd.FeatureExampleType));

            CreateMap<int, FeatureExample>()
                .ForMember(fe => fe.ID, fe => fe.MapFrom(id => id));
        }
    }
}
