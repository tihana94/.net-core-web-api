using APIfornetapplication.Models.Domain;
using APIfornetapplication.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APIfornetapplication.Controllers
{
    [ApiController]
    [Route("[controller]")] //it means Regions and it will populate controller from Regions
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        

        [HttpGet]
        public async Task<IActionResult> GetAllRegions ()
        {
            var regions= await regionRepository.GetAllAsync();

            //return dto regions
            /* var regionsDTO = new List<Models.DTO.Region>();
             regions.ToList().ForEach(region =>
             {
                 var regionDTO = new Models.DTO.Region()
                 {
                     Id = region.Id,
                     Code = region.Code,
                     Name = region.Name,
                     Area = region.Area,
                     Long=region.Long,
                     Population = region.Population

                 };
                 regionsDTO.Add(regionDTO);
             });
            */
            var regionsDTO=mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);

        }
    }
}
