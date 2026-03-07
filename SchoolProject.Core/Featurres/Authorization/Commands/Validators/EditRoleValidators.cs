using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Featurres.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Authorization.Commands.Validators
{
    public class EditRoleValidators : AbstractValidator<EditRoleCommand>
    {
        #region Fields

        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthorizationService _authorizationService;
        #endregion
        public EditRoleValidators(IStringLocalizer<SharedResources> stringLocalizer, IAuthorizationService authorizationService)
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
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);
            RuleFor(i => i.Name)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.MustNotBeNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.NotExceed100Characters])
                .MustAsync(async (Name, cancellationToken) => await _authorizationService.GetRoleByNameAsync(Name) == null)
                .WithMessage(_localizer[SharedResourcesKeys.RoleAlreadyExists]);
        }
        public void ApplyCustomValidationRules()
        {

        }
    }
}
