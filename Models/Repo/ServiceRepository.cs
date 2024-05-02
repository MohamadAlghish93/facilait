using FacilaIT.Models;

namespace FacilaIT.Models.Repo
{
    public class ServiceRepository
    {
        private readonly DBBContext _dbContext;

        public ServiceRepository(DBBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Implement your repository methods that use the DbContext
        // public ServiceItem GetById(int id)
        // {
        //     return _dbContext.ServiceItem.Find(id);
        // }

        // Other repository methods
    }
}