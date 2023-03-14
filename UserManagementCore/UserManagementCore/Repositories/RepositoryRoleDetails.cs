using UserManagementCore.Interfaces;
using UserManagementCore.Models;

namespace UserManagementCore.Repositories
{
    public class RepositoryRoleDetails : IRepository<ApplicationRoleDetails>
    {
        ApplicationDbContext _dbContext;

        public RepositoryRoleDetails(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }
        public async Task<ApplicationRoleDetails> Create(ApplicationRoleDetails _object)
        {
            var obj = await _dbContext.RoleDetails.AddAsync(_object);
            _dbContext.SaveChanges();
            return obj.Entity;
        }

        public void Delete(ApplicationRoleDetails _object)
        {
            _dbContext.Remove(_object);
            _dbContext.SaveChanges();
        }

        public IEnumerable<ApplicationRoleDetails> GetAll()
        {
            try
            {
                return _dbContext.RoleDetails.ToList();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw;
            }
        }

        public ApplicationRoleDetails GetById(int Id)
        {
            return _dbContext.RoleDetails.Where(x => x.Id == Id).FirstOrDefault();
        }

        public void Update(ApplicationRoleDetails _object)
        {
            _dbContext.RoleDetails.Update(_object);
            _dbContext.SaveChanges();
        }
    }
}
