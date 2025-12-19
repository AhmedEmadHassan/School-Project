using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolProject.Core.Featurres.Students.Queries.Response
{
    public class StudentResponse
    {

        public int StudID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string? DepartmentName { get; set; }
    }
}
