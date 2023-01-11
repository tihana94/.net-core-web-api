using APIfornetapplication.Models.Domain;
using APIfornetapplication.Models.DTO;
using APIfornetapplication.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

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
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();

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
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);

        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region=await regionRepository.GetAsync(id);
            if(region==null)
            {
                return NotFound();
            }
            var regionDTO=mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //request (DTO) to Domain model
            var region = new Models.Domain.Region()
            {
                Code= addRegionRequest.Code,
                Area=addRegionRequest.Area, 
                Long=addRegionRequest.Long,
                Name=addRegionRequest.Name,
                Population=addRegionRequest.Population
            };

            //pass details to Repository
            region=await regionRepository.AddAsync(region);
            //convert back to DTP
            var regionDTO = new Models.DTO.Region()
            {
                Id=region.Id,  
                Code = region.Code,
                Area = region.Area,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            return CreatedAtAction(nameof(GetRegionAsync),new { id=regionDTO.Id}, regionDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //get region from database
            var region=await regionRepository.DeleteAsync(id);
            //if null NotFound
            if(region==null)
            {
                return NotFound();
            }
            //convert response back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Long = region.Long,
                Name=region.Name,
                Population=region.Population

            };
            //return Ok response
            return Ok(regionDTO);
        }

        [HttpPut] //this is update method and that is the reason why http put
        [Route("{id:guid}")]
        public async Task<IActionResult> PublicRegionAsync([FromRoute]Guid id, [FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest )
        {
            //Convert DTO to Domain Model
            var region = new Models.Domain.Region()
            {
                
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population
             };

            //update region using repository
            region=await regionRepository.UpdateAsync(id, region);
            //if Null then not Found
            if(region==null)
            {
                return NotFound();
            }
            //Covert domain back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population

            };

            //return Ok response
            return Ok(region);
        }
    }

}
