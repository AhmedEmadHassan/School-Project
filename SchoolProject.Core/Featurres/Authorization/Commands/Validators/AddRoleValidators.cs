using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Featurres.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Authorization.Commands.Validators
{
    public class AddRoleValidators : AbstractValidator<AddRoleCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthorizationService _authorizationService;
        #endregion
        #region Constructors
        public AddRoleValidators(IStringLocalizer<SharedResources> localizer, IAuthorizationService authorizationService)
        {
            _localizer = localizer;
            _authorizationService = authorizationService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        #endregion

        #region Methods
        public void ApplyValidationRules()
        {
            RuleFor(i => i.RoleName)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.MustNotBeNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);
        }
        public void ApplyCustomValidationRules()
        {
            RuleFor(i => i.RoleName)
                .MustAsync(async (roleName, cancellationToken) => roleName != null ? !await _authorizationService.IsRoleExist(roleName) : true)
                .WithMessage(_localizer[SharedResourcesKeys.RoleAlreadyExists]);
        }
        #endregion
    }
}
