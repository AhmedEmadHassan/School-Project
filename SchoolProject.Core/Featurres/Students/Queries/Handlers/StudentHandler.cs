using AutoMapper;
using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Students.Queries.Models;
using SchoolProject.Core.Featurres.Students.Queries.Response;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Core.Featurres.Students.Queries.Handlers
{
    public class StudentHandler : ResponseHandler,IRequestHandler<GetStudentListQuery, Response<List<StudentResponse>>>
    {
        #region Fields
        IStudentService _studentService;
        IMapper _mapper;
        #endregion
        #region Constructors
        public StudentHandler(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public async Task<Response<List<StudentResponse>>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            var studentList = await _studentService.GetStudentsListAsync();
            var mappedStudentList = _mapper.Map<List<StudentResponse>>(studentList);
            return Success(mappedStudentList);
        }
        #endregion
    }
}
