using APIfornetapplication.Models.Domain;
using APIfornetapplication.Models.DTO;
using APIfornetapplication.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APIfornetapplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(Repositories.IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulty()
        {
            var walkdifficultyDomain = await walkDifficultyRepository.GetAllAsync();
            var walkdifficutlyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkdifficultyDomain);
            return Ok(walkdifficutlyDTO);
        }
        [HttpGet]
        [Route("{id:gui}")]
        [ActionName ("GetWalkDifficultyId")]
        public async Task<IActionResult> GetWalkDifficultyId(Guid id)
        {
            var walkDifficulty = walkDifficultyRepository.GetAsync(id);
            if(walkDifficulty == null)
            {
                return NotFound();
            }
            //convert domain to dto
            var walkdifficultyDTO=mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            
            return Ok(walkdifficultyDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDiffucultyRequest addWalkDiffucultyRequest)
        {
            //validate response
            if(!ValidateAddWalkDifficutlyAsync(addWalkDiffucultyRequest))
            {
                return BadRequest(ModelState);

            }
            //convert dto to domain model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDiffucultyRequest.Code
            };
            //call repository

            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            //check if null
            if(walkDifficultyDomain==null)
            {
                return NotFound();
            }
            //conver edomain to dto
            var difficultyDTO=mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return CreatedAtAction(nameof(GetWalkDifficultyId), new { id = difficultyDTO.Id },difficultyDTO);
        }

        [HttpPut]
        [Route("{id:gui}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsing(Guid id, Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //validate response
            if(!ValidateUpdateWalkDifficutlyAsync(updateWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }
            //convert dto to domain
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };
            //call repository update
            walkDifficultyDomain=await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }
            //convert dto to domain
            var walkDifficutlyDTO=mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            return Ok(walkDifficutlyDTO);
        }
        [HttpDelete]
        [Route("{id:gui}")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            var walkdifficultyDomain=await walkDifficultyRepository.DeleteAsync(id);
            if(walkdifficultyDomain == null)
            {
                return NotFound();
            }
            //convert to dto
            var walkdifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkdifficultyDomain);
            return Ok(walkdifficultyDTO);
        }

        #region Private methods

        private bool ValidateAddWalkDifficutlyAsync(Models.DTO.AddWalkDiffucultyRequest addWalkDiffucultyRequest)
        {
            if(addWalkDiffucultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDiffucultyRequest), $"{nameof(addWalkDiffucultyRequest)} is required");
                return false;
            }
            if(string.IsNullOrWhiteSpace(addWalkDiffucultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDiffucultyRequest.Code), $"{nameof(addWalkDiffucultyRequest.Code)} is required");
                return false;
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        private bool ValidateUpdateWalkDifficutlyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDiffucultyRequest)
        {
            if (updateWalkDiffucultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDiffucultyRequest), $"{nameof(updateWalkDiffucultyRequest)} is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkDiffucultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDiffucultyRequest.Code), $"{nameof(updateWalkDiffucultyRequest.Code)} is required");
                return false;
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

