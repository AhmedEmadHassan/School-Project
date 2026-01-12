using FluentValidation;
using SchoolProject.Core.Featurres.Students.Commands.Models;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Students.Commands.Validators
{
    public class DeleteStudentValidators : AbstractValidator<DeleteStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        #endregion
        #region Contstructors
        public DeleteStudentValidators(IStudentService studentService)
        {
            _studentService = studentService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        #endregion

        #region Methods
        public void ApplyValidationRules()
        {
            RuleFor(s => s.Id)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id Must Be Greater that 0");

        }
        public void ApplyCustomValidationRules()
        {

        }
        #endregion
    }
}
