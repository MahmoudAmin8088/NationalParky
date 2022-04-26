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
    public class NationalParkController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INationalParkRepo _npRepo;

        public NationalParkController(INationalParkRepo npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }
        /// <summary>
        /// Get List Of NationalParks
        /// </summary>
        /// <returns>ObjDto</returns>

        //[Authorize(Roles ="Admin")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetNationalPark()
        {
            var objList = _npRepo.GetNationalParks();

            if (objList is null)
                return NotFound();
            var objDto = new List<NationalParkDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
            }
            return Ok(objDto);
        }
        /// <summary>
        ///  Get Individual NationalPark
        /// </summary>
        /// <param name="nationalParkId">The Id of the National Park</param>
        /// <returns>ObjDto</returns>
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var Obj = _npRepo.GetNationalPark(nationalParkId);

            if (Obj is null)
                return NotFound();

            var ObjDto = _mapper.Map<NationalParkDto>(Obj);

            return Ok(ObjDto);
        }
        /// <summary>
        /// Create NationalPark
        /// </summary>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        
       // [Authorize(Roles ="User")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists");
                return StatusCode(404, ModelState);
            }

            nationalParkDto.Created = DateTime.Now;
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepo.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something Went Wrong When Saving The Record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkDto.Id }, nationalParkDto);
            //return Ok();

        }
        /// <summary>
        /// Update NationalPark
        /// </summary>
        /// <param name="nationalParkId">The Id of the National Park</param>
        /// <param name="nationalParkDto">NationalParkDto</param>
        /// <returns>NoContent</returns>
       // [Authorize(Roles ="Admin")]
        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (!ModelState.IsValid || nationalParkDto.Id != nationalParkId)
                return BadRequest(ModelState);
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something Went Wrong When Updating The Record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
        /// <summary>
        /// Delete NationalPark
        /// </summary>
        /// <param name="nationalParkId">The Id of the National Park</param>
        /// <returns>NoContent</returns>
        
        //[Authorize(Roles ="Admin")]
        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_npRepo.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }
            var nationalParkObj = _npRepo.GetNationalPark(nationalParkId);
            if (!_npRepo.DeleteteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something Went Wrong When Deleteing The Record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
