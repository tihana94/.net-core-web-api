using APIfornetapplication.Data;
using APIfornetapplication.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace APIfornetapplication.Repositories
{

    public class WalkRepository : IWalkRepository
    {
        private readonly dbcontext dbContextDB;

        public WalkRepository(dbcontext dbContextDB)
        {
            this.dbContextDB = dbContextDB;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //asing new id
            walk.Id = Guid.NewGuid();
            await dbContextDB.Walks.AddAsync(walk);
            await dbContextDB.SaveChangesAsync();
            return walk;
            
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await dbContextDB.Walks.FindAsync(id);
            if(existingWalk == null)
            {
                return null;
            }
            dbContextDB.Walks.Remove(existingWalk);
            dbContextDB.SaveChangesAsync();
            return existingWalk;

        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await 
                dbContextDB.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public Task<Walk> GetAsync(Guid id)
        {
           return dbContextDB.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContextDB.Walks.FindAsync(id);

            if(existingWalk!=null)
            {
                existingWalk.Length = walk.Length;
                existingWalk.Name = walk.Name;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;
                await dbContextDB.SaveChangesAsync();
                return existingWalk;
            }
            return null;
        }
    }
}
