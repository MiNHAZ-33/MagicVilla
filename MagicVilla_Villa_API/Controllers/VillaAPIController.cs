using MagicVilla_Villa_API.Data;
using MagicVilla_Villa_API.Models;
using MagicVilla_Villa_API.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Villa_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStrore.villaList) ;
        }

        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStrore.villaList.FirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villa){
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            if(villa == null)
            {
                return BadRequest(villa);
            }
            if(villa.Id < 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villa.Id = VillaStrore.villaList.OrderByDescending(u => u.Id).FirstOrDefault()!.Id + 1;

            VillaStrore.villaList.Add(villa);

            return CreatedAtRoute("GetVilla", new {id = villa.Id}, villa);
        }
        [HttpDelete("{id}", Name = "DeleteVilla")]
        public ActionResult DeleteVillaById(int id)
        {
            var villla = VillaStrore.villaList.FirstOrDefault(u => u.Id == id);
            if (villla is  null)
            {
                return BadRequest($"{id} not found");
            }
            VillaStrore.villaList.Remove(villla);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("{id:int}", Name="UpdateVilla")]
        public ActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
        {
            if(villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            var villa = VillaStrore.villaList.FirstOrDefault(v => v.Id == id);

            villa.Name = villaDTO.Name;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Sqft= villaDTO.Sqft;

            return NoContent() ;
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDto)
        {
            if(patchDto == null)
            {
                return BadRequest();
            }

            var villa = VillaStrore.villaList.FirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return BadRequest();
            }
            patchDto.ApplyTo(villa, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
