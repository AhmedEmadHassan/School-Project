using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Infrustructure.Context;
using SchoolProject.Infrustructure.Infrastructure_Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolProject.Infrustructure.Repositories
{
    public class StudentRepository : GenericRepositoryAsync<Student>, IStudentRepository
    {
        #region Fields
        private readonly DbSet<Student> _students;
        #endregion
        #region Constructors
        public StudentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _students = dbContext.students;
        }
        #endregion
        #region Methods
        public Task<List<Student>> GetAllStudentsListAsync()
        {   
            return _dbContext.students.Include(s => s.Department).ToListAsync();
        }
        #endregion

    }
}
