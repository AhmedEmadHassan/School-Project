using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Featurres.Users.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Featurres.Users.Commands.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion
        #region Constructors
        public UpdateUserValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        #endregion
        #region Methods
        public void ApplyValidationRules()
        {
            RuleFor(i => i.FullName)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.MustNotBeNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.NotExceed100Characters]);
            RuleFor(i => i.Email)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.MustNotBeNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .EmailAddress().WithMessage(_localizer[SharedResourcesKeys.BadRequest]);
            RuleFor(i => i.PhoneNumber)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.MustNotBeNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage(_localizer[SharedResourcesKeys.BadRequest]);
            RuleFor(i => i.UserName).NotNull().WithMessage(_localizer[SharedResourcesKeys.MustNotBeNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MaximumLength(50).WithMessage(_localizer[SharedResourcesKeys.NotExceed100Characters]);
        }
        public void ApplyCustomValidationRules()
        {

        }
        #endregion
    }
}
