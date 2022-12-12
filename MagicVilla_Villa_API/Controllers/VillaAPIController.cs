using MagicVilla_Villa_API.Data;
using MagicVilla_Villa_API.Models;
using MagicVilla_Villa_API.Models.DTO;
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

        [HttpGet("{id:int}")]
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
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villa){
            if(villa == null)
            {
                return BadRequest(villa);
            }
            if(villa.Id < 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villa.Id = VillaStrore.villaList.OrderByDescending(u=>u.Id).FirstOrDefault().Id + 1;

            VillaStrore.villaList.Add(villa);

            return Ok(villa);
        }
    }
}
