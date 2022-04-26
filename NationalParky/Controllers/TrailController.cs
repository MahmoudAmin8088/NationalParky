using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalParky.Models;
using NationalParky.Models.Dtos;
using NationalParky.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParky.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class TrailController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITrailRepo _trailRepo;
        public TrailController(IMapper mapper, ITrailRepo trailRepo)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;

        }
        /// <summary>
        /// Get List Of Trails
        /// </summary>
        /// <returns>ObjDto</returns>
       // [Authorize(Roles ="Admin")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var ObjList = _trailRepo.GetTrails();
            if (ObjList == null)
            {
                return NotFound();
            }
            var ObjDto = new List<TrailDto>();
            foreach (var obj in ObjList)
            {
                ObjDto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(ObjDto);
        }
        /// <summary>
        /// Get Individual Trail
        /// </summary>
        /// <param name="trailId">The Id of the Trail</param>
        /// <returns>objDto</returns>
        //[Authorize(Roles ="Admin")]
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _trailRepo.GetTrail(trailId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<TrailDto>(obj);

            return Ok(objDto);
        }
        /// <summary>
        /// Get  Trail In NationalPark
        /// </summary>
        /// <param name="nationalParkId">The Id of the NationalPark</param>
        /// <returns>objDto</returns>
        [HttpGet("[action]/{nationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var objList = _trailRepo.GetTrailsInNationalPark(nationalParkId);

            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<TrailDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Create Trail
        /// </summary>
        /// <param name="trailDto">The Trail Details</param>
        /// <returns>TrailDto</returns>


       // [Authorize(Roles = "User")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", " Trail Exists");
                return StatusCode(404, ModelState);
            }
            
            var trailObj = _mapper.Map<Trail>(trailDto);

            trailObj.DateCreated = DateTime.Now;
                
            if (!_trailRepo.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something Went Wrong When Saving The Record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id }, trailObj);

        }
        /// <summary>
        /// Update Trail
        /// </summary>
        /// <param name="trailId">>The Id of the Trail</param>
        /// <param name="trailDto">The Trail Details</param>
        /// <returns>NoContent</returns>
        
       // [Authorize(Roles ="Admin")]
        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
        {
            if (!ModelState.IsValid || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }

           

            var trailObj = _mapper.Map<Trail>(trailDto);

            

            if (!_trailRepo.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something Went Wrong When Updating The Record {trailObj.Name}");
                return StatusCode(500, ModelState);

            }
            return NoContent();

        }
        /// <summary>
        /// Delete Trail 
        /// </summary>
        /// <param name="trailId">>The Id of the Trail</param>
        /// <returns>NoContent</returns>
        
        //[Authorize(Roles ="Admin")]
        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepo.TrailExists(trailId))
            {
                return NotFound();
            }

            var trailObj = _trailRepo.GetTrail(trailId);
            if (!_trailRepo.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something Went Wrong When Deleteing The Record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }




    }
}
