using APIfornetapplication.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace APIfornetapplication.Data
{
    public class dbcontext:DbContext
    {
        public dbcontext(DbContextOptions<dbcontext> options) : base(options)
        {

        }
        public DbSet<Region>Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }
    }
}
