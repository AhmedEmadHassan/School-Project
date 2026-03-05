using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Infrustructure.Context;
using SchoolProject.Infrustructure.Infrastructure_Bases;

namespace SchoolProject.Infrustructure.Repositories
{
    public class UserRefreshTokenRepository : GenericRepositoryAsync<UserRefreshToken>, IUserRefreshTokenRepository
    {
        #region Fields
        private readonly DbSet<UserRefreshToken> _userRefreshTokens;
        #endregion
        #region Constructor
        public UserRefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _userRefreshTokens = dbContext.userRefreshTokens;
        }
        #endregion
        #region Methods

        #endregion
    }
}
