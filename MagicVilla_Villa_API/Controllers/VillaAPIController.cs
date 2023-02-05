using MagicVilla_Villa_API.Data;
using MagicVilla_Villa_API.Models;
using MagicVilla_Villa_API.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_Villa_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;

        private readonly ApplicationDbContext _db;

        public VillaAPIController(ILogger<VillaAPIController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task< ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.LogInformation("Getting all villas");
            return Ok(await _db.Villas.ToListAsync()) ;
        }

        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task <ActionResult<VillaDTO>> GetVilla(int id)
        {
            if(id == 0)
            {
                _logger.LogError("Get villa error with id " + id);
                return BadRequest();
            } 

            var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
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
        public async Task< ActionResult<VillaDTO>> CreateVilla([FromBody]VillaCreateDTO villa){
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            if(villa == null)
            {
                return BadRequest(villa);
            }

            Villa model = new()
            {
               
                Name = villa.Name,
                Amenity= villa.Amenity,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };

            await _db.Villas.AddAsync(model);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new {id = model.Id}, model);
        }
        [HttpDelete("{id}", Name = "DeleteVilla")]
        public async Task< ActionResult > DeleteVillaById(int id)
        {
            var villla = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
            if (villla is  null)
            {
                return BadRequest($"{id} not found");
            }
            _db.Villas.Remove(villla);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("{id:int}", Name="UpdateVilla")]
        public async Task< ActionResult> UpdateVilla(int id, [FromBody]VillaUpdateDTO villa)
        {
            if(villa == null || id != villa.Id)
            {
                return BadRequest();
            }
            //var villa = VillaStrore.villaList.FirstOrDefault(v => v.Id == id);

            //villa.Name = villaDTO.Name;
            //villa.Occupancy = villaDTO.Occupancy;
            //villa.Sqft= villaDTO.Sqft;

            Villa model = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Amenity=villa.Amenity,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };

            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return NoContent() ;
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task< IActionResult > UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDto)
        {
            if(patchDto == null)
            {
                return BadRequest();
            }

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if(villa == null)
            {
                return BadRequest();
            }
            VillaUpdateDTO model = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Amenity = villa.Amenity,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };

            patchDto.ApplyTo(model, ModelState);
            Villa updatedVilla = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Amenity = villa.Amenity,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };

            _db.Villas.Update(updatedVilla);
            await _db.SaveChangesAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
