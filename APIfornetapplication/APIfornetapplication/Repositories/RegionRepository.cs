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

        public async Task<Region> AddAsync(Region region)
        {
            region.Id=Guid.NewGuid();
            await dbcontextDB.AddAsync(region);
            await dbcontextDB.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await dbcontextDB.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(region == null)
            {
                return region;
            }
            //delete region from database
            dbcontextDB.Regions.Remove(region);
            await dbcontextDB.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await dbcontextDB.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await dbcontextDB.Regions.FirstOrDefaultAsync(x => x.Id == id);
           
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existingregion=await dbcontextDB.Regions.FirstOrDefaultAsync(x => x.Id == id);
            
            if(existingregion==null)
            {
                return null;
            }
            existingregion.Code = region.Code;
            existingregion.Name = region.Name;
            existingregion.Area = region.Area;
            existingregion.Long = region.Long;
            existingregion.Population = region.Population;

            dbcontextDB.SaveChangesAsync();

            return existingregion;

        }
    }
}
