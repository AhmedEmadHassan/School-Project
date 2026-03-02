using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Featurres.Users.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Featurres.Users.Commands.Validators
{
    public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        public ChangeUserPasswordValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        public void ApplyValidationRules()
        {

        }
        public void ApplyCustomValidationRules()
        {

        }
    }
}
