using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Featurres.Authentication.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Featurres.Authentication.Commands.Validators
{
    public class SignInValidators : AbstractValidator<SignInCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion
        #region Contstructors
        public SignInValidators(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        #endregion

        #region Methods
        public void ApplyValidationRules()
        {
            RuleFor(i => i.UserName)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
            RuleFor(i => i.Password)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidationRules()
        {

        }
        #endregion
    }
}
