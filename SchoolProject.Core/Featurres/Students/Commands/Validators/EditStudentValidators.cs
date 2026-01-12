using FluentValidation;
using SchoolProject.Core.Featurres.Students.Commands.Models;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Students.Commands.Validators
{
    public class EditStudentValidators : AbstractValidator<EditStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        #endregion
        #region Contstructors
        public EditStudentValidators(IStudentService studentService)
        {
            _studentService = studentService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        #endregion
        #region Methods
        public void ApplyValidationRules()
        {
            RuleFor(s => s.Name)
             .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(s => s.Address)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .MaximumLength(100).WithMessage("{PropertyValue} address must not exceed 200 characters.");

        }
        public void ApplyCustomValidationRules()
        {
            // check If another person have the same name (Only Validate if the person Id is valid)
            RuleFor(s => s.Name)
                .MustAsync(async (model, key, CancellationToken) => (!await _studentService.IsNameExistsExcludeSelfAsync(key, model.Id) || !await _studentService.IsIdExistsAsync(model.Id)))
                .WithMessage("The Name already Exists");
        }
        #endregion
    }
}
