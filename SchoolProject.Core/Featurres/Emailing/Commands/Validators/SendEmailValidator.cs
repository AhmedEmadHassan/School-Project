using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Featurres.Emailing.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Featurres.Emailing.Commands.Validators
{
    public class SendEmailValidator : AbstractValidator<SendEmailCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion
        #region Constructors
        public SendEmailValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        #endregion
        #region Actions
        public void ApplyValidationRules()
        {

        }
        public void ApplyCustomValidationRules()
        {

        }
        #endregion
    }
}
