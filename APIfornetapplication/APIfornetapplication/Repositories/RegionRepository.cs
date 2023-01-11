using APIfornetapplication.Data;
using APIfornetapplication.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace APIfornetapplication.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly dbcontext dbcontextDB;

        public RegionRepository(dbcontext dbcontextDB)
        {
            this.dbcontextDB = dbcontextDB;
        }
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await dbcontextDB.Regions.ToListAsync();
        }
    }
}
