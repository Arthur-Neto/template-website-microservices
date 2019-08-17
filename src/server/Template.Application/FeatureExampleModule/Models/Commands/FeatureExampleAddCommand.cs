using FluentValidation;
using Template.Domain.FeatureExampleModule;

namespace Template.Application.FeatureExampleModule.Models.Commands
{
    public class FeatureExampleAddCommand
    {
        public FeatureExampleEnum FeatureExampleType { get; set; }
    }

    public class FeatureExampleAddCommandCommandValidator : AbstractValidator<FeatureExampleAddCommand>
    {
        public FeatureExampleAddCommandCommandValidator()
        {
            RuleFor(x => x.FeatureExampleType)
                .IsInEnum();
        }
    }
}
