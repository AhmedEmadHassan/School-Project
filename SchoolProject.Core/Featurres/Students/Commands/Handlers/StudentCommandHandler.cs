using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Students.Commands.Models;
using SchoolProject.Core.Featurres.Students.Queries.Response;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Featurres.Students.Commands.Handlers
{
    public class StudentCommandHandler : ResponseHandler, IRequestHandler<AddStudentCommand, Response<GetStudentResponse>>
                                                        , IRequestHandler<EditStudentCommand, Response<GetStudentResponse>>
                                                        , IRequestHandler<DeleteStudentCommand, Response<string>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion
        #region Constructors
        public StudentCommandHandler(IStringLocalizer<SharedResources> localizer, IStudentService studentService, IMapper mapper) : base(localizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _localizer = localizer;
        }
        #endregion
        #region Methods
        public async Task<Response<GetStudentResponse>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            // Mapping Between Request and Student
            var studentModel = _mapper.Map<Student>(request);
            // Add Student
            var result = await _studentService.AddAsync(studentModel);

            if (result == null)
            {
                return BadRequest<GetStudentResponse>(_localizer[SharedResourcesKeys.FailedToAddStudent]);
            }
            var createdStudentResponse = _mapper.Map<GetStudentResponse>(result);
            return Created<GetStudentResponse>(createdStudentResponse);
        }

        public async Task<Response<GetStudentResponse>> Handle(EditStudentCommand request, CancellationToken cancellationToken)
        {
            // Check if student Id Exists or not
            var student = await _studentService.GetStudentByIdAsync(request.Id);
            if (student == null)
            {
                return NotFound<GetStudentResponse>(_localizer[SharedResourcesKeys.StudentNotFound]);
            }
            // Map Between Response and Request 
            var mappedStudent = _mapper.Map(request, student);
            // Edit Student
            var result = await _studentService.EditAsync(mappedStudent);
            // Check if Editation Failed
            if (!result)
            {
                return BadRequest<GetStudentResponse>(_localizer[SharedResourcesKeys.FailedToEditStudent]);
            }
            // If Not Failed Return Success Response
            var editedStudent = await _studentService.GetStudentByIdWithIncludeAsync(request.Id);

            GetStudentResponse editedStudentResponse = _mapper.Map<GetStudentResponse>(editedStudent);
            return Success<GetStudentResponse>(editedStudentResponse);
        }

        public async Task<Response<string>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentService.GetStudentByIdAsync(request.Id);
            if (student == null)
            {
                return NotFound<string>(_localizer[SharedResourcesKeys.StudentNotFound]);
            }
            bool result = await _studentService.DeleteAsync(student);
            if (!result)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToDeleteStudent]);
            }
            return Deleted<string>(_localizer[SharedResourcesKeys.StudentDeletedSuccessfully]);
        }
        #endregion
    }
}
