using AutoMapper;
using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Students.Commands.Models;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Students.Commands.Handlers
{
    public class StudentCommandHandler : ResponseHandler, IRequestHandler<AddStudentCommand, Response<string>>
                                                        , IRequestHandler<EditStudentCommand, Response<string>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        #endregion
        #region Constructors
        public StudentCommandHandler(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }
        #endregion
        #region Methods
        public async Task<Response<string>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            // Mapping Between Request and Student
            var studentModel = _mapper.Map<Student>(request);
            // Add Student
            var result = await _studentService.AddAsync(studentModel);
            if (!result)
            {
                return BadRequest<string>("Failed to Add Student");
            }
            return Created("Added Successfully");
        }

        public async Task<Response<string>> Handle(EditStudentCommand request, CancellationToken cancellationToken)
        {
            // Check if student Id Exists or not
            var student = await _studentService.GetStudentByIdAsync(request.Id);
            if (student == null)
            {
                return NotFound<string>("Student Not Found");
            }
            // Map Between Response and Request
            student = _mapper.Map<Student>(request);
            // Edit Student
            var result = await _studentService.EditAsync(student);
            // Check if Editation Failed
            if (!result)
            {
                return BadRequest<string>("Failed to Edit Student");
            }
            // If Not Failed Return Success Response
            return Success<string>("Student Edited Successfully");
        }
        #endregion
    }
}
