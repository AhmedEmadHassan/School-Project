using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Featurres.Students.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;
namespace SchoolProject.Core.Featurres.Students.Commands.Validators
{
    public class AddStudentValidators : AbstractValidator<AddStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion
        #region Contstructors
        public AddStudentValidators(IStudentService studentService, IStringLocalizer<SharedResources> localizer)
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

            RuleFor(s => s.DepartmentId)
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThan0]);

        }
        public void ApplyCustomValidationRules()
        {
            RuleFor(s => s.Name)
                .MustAsync(async (key, CancellationToken) => !await _studentService.IsNameExistsAsync(key))
                .WithMessage(_localizer[SharedResourcesKeys.AlreadyExists]);
        }
        #endregion

    }
}
