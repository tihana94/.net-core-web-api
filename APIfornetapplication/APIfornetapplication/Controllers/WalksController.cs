using APIfornetapplication.Models.DTO;
using APIfornetapplication.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APIfornetapplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalks()
        {
            //fetch data from database
            var walksDomain = await walkRepository.GetAllAsync();
            //convert domain walks to dto walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);
            //return response
            return Ok(walksDTO);

        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //get walk domain object from database
            var walksDomain=await walkRepository.GetAsync(id);

            //convert domain walks to dto walks
            var walksDTO= mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            //return response
            return Ok(walksDTO);



        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequest addWalkRequest)
        {
            //convert dto to domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId=addWalkRequest.RegionId,
                WalkDifficultyId=addWalkRequest.WalkDifficultyId
            };
            //passs domain object to repository
            walkDomain=await walkRepository.AddAsync(walkDomain);
            //convert domain object back to dto
            var walksDTO = new Models.DTO.Walk
            {
                Id=walkDomain.Id,
                Length=walkDomain.Length,
                Name=walkDomain.Name,
                RegionId=walkDomain.RegionId,
                WalkDifficultyId=walkDomain.WalkDifficultyId

            };
            //send dto response back to client
            return CreatedAtAction(nameof(GetWalkAsync), new {id=walksDTO.Id}, walksDTO);
        }

        [HttpPut] //for update method
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //convert dto to domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId

            };

            //pass details to repository
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            //handle null
            if (walkDomain == null)
            {
                return NotFound();
            }
            
                //convert domain to dto
               var walkDTO = new Models.DTO.Walk()
                {
                   Id = walkDomain.Id,
                   Length = walkDomain.Length,
                   Name= walkDomain.Name,  
                   RegionId=walkDomain.RegionId,
                   WalkDifficultyId= walkDomain.WalkDifficultyId

                };



            //return response
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync([FromRoute] Guid id)
        {
            //call repository to call walk
            var walkDomain = await walkRepository.DeleteAsync(id);
            if(walkDomain==null)
            {
                return NotFound();
            }
            var walkDTO=mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);

        }

    }
}
