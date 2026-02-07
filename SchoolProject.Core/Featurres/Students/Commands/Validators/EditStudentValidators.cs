using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Featurres.Students.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Students.Commands.Validators
{
    public class EditStudentValidators : AbstractValidator<EditStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion
        #region Contstructors
        public EditStudentValidators(IStudentService studentService, IStringLocalizer<SharedResources> localizer)
        {
            _studentService = studentService;
            _localizer = localizer;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        #endregion
        #region Methods
        public void ApplyValidationRules()
        {
            RuleFor(s => s.Name)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
            .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.NotExceed100Characters]);

            RuleFor(s => s.Address)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.MustNotBeNull])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.NotExceed100Characters]);

        }
        public void ApplyCustomValidationRules()
        {
            // check If another person have the same name (Only Validate if the person Id is valid)
            RuleFor(s => s.Name)
                .MustAsync(async (model, key, CancellationToken) => (!await _studentService.IsNameExistsExcludeSelfAsync(key, model.Id) || !await _studentService.IsIdExistsAsync(model.Id)))
                .WithMessage(_localizer[SharedResourcesKeys.AlreadyExists]);
        }
        #endregion
    }
}
