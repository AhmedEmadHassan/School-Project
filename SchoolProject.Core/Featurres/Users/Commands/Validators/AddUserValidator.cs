using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Featurres.Users.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Featurres.Users.Commands.Validators
{
    public class AddUserValidator : AbstractValidator<AddUserCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion
        #region Constructors
        public AddUserValidator(IStringLocalizer<SharedResources> localizer)
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
            RuleFor(i => i.Password)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.MustNotBeNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MinimumLength(6).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThan0]);
            RuleFor(i => i.ConfirmPassword)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.MustNotBeNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .Equal(i => i.Password).WithMessage(_localizer[SharedResourcesKeys.BadRequest]);
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
