using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Featurres.Students.Queries.Response;
using System.Collections.Generic;

namespace SchoolProject.Core.Featurres.Students.Queries.Models
{
    public class GetStudentListQuery : IRequest<Response<List<StudentResponse>>>
    {

    }
}
