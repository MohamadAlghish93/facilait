using FacilaIT.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FacilaIT.Helper.Rabc
{
    public class RbacService
    {
        private readonly DBBContext _dbContext;

        public RbacService(DBBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool HasAccess(int userId, string roleName)
        {
            
            // var user = _dbContext.Users
            //     .Include(u => u.Roles)
            //     .FirstOrDefault(u => u.Id == userId);

            // if (user != null)
            // {
            //     return user.Roles.Any(r => r.Name == roleName);
            // }

            return false;
        }
    }
}