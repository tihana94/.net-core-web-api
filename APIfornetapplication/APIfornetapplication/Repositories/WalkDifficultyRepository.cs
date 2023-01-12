using APIfornetapplication.Data;
using APIfornetapplication.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace APIfornetapplication.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly dbcontext dbcontextDB;

        public WalkDifficultyRepository(dbcontext dbcontextDB)
        {
            this.dbcontextDB = dbcontextDB;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await dbcontextDB.WalkDifficulty.AddAsync(walkDifficulty);
            await dbcontextDB.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingwalkdifficulty = await dbcontextDB.WalkDifficulty.FindAsync(id);
            if (existingwalkdifficulty != null)
            {
                dbcontextDB.WalkDifficulty.Remove(existingwalkdifficulty);
                dbcontextDB.SaveChangesAsync();  
                return existingwalkdifficulty;
            }
            return null;

        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            await dbcontextDB.WalkDifficulty.ToListAsync();
            return dbcontextDB.WalkDifficulty;
            
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await dbcontextDB.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingwalkdifficulty=await dbcontextDB.WalkDifficulty.FindAsync(id);
            if(existingwalkdifficulty == null)
            {
                return null;
            }
            existingwalkdifficulty.Code=walkDifficulty.Code;
            dbcontextDB.SaveChangesAsync();
            return existingwalkdifficulty;
        }
    }
}
