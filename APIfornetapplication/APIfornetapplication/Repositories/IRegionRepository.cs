using APIfornetapplication.Models.Domain;

namespace APIfornetapplication.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
