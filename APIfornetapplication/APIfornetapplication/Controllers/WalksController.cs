using APIfornetapplication.Models.DTO;
using APIfornetapplication.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace APIfornetapplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;

        public IWalkDifficultyRepository WalkDifficultyRepository { get; }

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            WalkDifficultyRepository = walkDifficultyRepository;
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
            //validate request
            if(await ValidateAddWalkAsync(addWalkRequest))
            {
                return BadRequest(ModelState);
            }
           
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
            //validate request
            if(!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);

            }
           
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

        #region Private methods 
        private async Task<bool> ValidateAddWalkAsync(AddWalkRequest addWalkRequest)
        {
           /* if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"{nameof(addWalkRequest)} cannot be empty.");
            }
            if (!string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} is required.");

            }
           
            if (addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} should be greater then zero.");

            }
            */
            var region =await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is invalid");

            }
            var walkdifficulty = await WalkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkdifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid");

            }
            if(ModelState.ErrorCount > 0)
            {
                return false;

            }
            return true;


        }
        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            /*
            if (updateWalkRequest.Name == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} cannot be empty.");
            }
            if (!string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} is required.");

            }
            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} should be greater then zero.");

            }
            */
            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is invalid");

            }
            var walkdifficulty = await WalkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkdifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid");

            }
            if (ModelState.ErrorCount > 0)
            {
                return false;

            }
            return true;


        }
        #endregion

    }
}
