using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Featurres.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Authorization.Commands.Validators
{
    public class DeleteRoleValidators : AbstractValidator<DeleteRoleCommand>
    {
        #region Fields

        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthorizationService _authorizationService;
        #endregion
        public DeleteRoleValidators(IStringLocalizer<SharedResources> stringLocalizer, IAuthorizationService authorizationService)
        {
            _localizer = stringLocalizer;
            _authorizationService = authorizationService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        public void ApplyValidationRules()
        {
            RuleFor(i => i.Id)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.MustNotBeNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThan0]);
        }
        public void ApplyCustomValidationRules()
        {

        }
    }
}
