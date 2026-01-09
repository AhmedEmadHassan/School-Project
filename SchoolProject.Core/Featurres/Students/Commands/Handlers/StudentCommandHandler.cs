using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Students.Commands.Models;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Core.Featurres.Students.Commands.Handlers
{
    public class StudentCommandHandler : ResponseHandler, IRequestHandler<AddStudentCommand, Response<string>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        #endregion
        #region Constructors
        public StudentCommandHandler(IStudentService studentService,IMapper mapper)
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
            if(!result)
            {
                return BadRequest<string>("Failed to Add Student");
            }
            return Created("Added Successfully");
        }
        #endregion
    }
}
