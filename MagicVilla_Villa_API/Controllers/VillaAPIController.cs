using AutoMapper;
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
        private readonly IMapper _mapper;

        public VillaAPIController(ILogger<VillaAPIController> logger, ApplicationDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.LogInformation("Getting all villas");

            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();

            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
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

            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public async Task< ActionResult<VillaDTO>> CreateVilla([FromBody]VillaCreateDTO createDTO){
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            if(createDTO == null)
            {
                return BadRequest(createDTO);
            }

            Villa model = _mapper.Map<Villa>(createDTO);


            //Villa model = new()
            //{
               
            //    Name = createDTO.Name,
            //    Amenity= createDTO.Amenity,
            //    Details = createDTO.Details,
            //    ImageUrl = createDTO.ImageUrl,
            //    Occupancy = createDTO.Occupancy,
            //    Rate = createDTO.Rate,
            //    Sqft = createDTO.Sqft
            //};

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
        public async Task< ActionResult> UpdateVilla(int id, [FromBody]VillaUpdateDTO updateDTO)
        {
            if(updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }


            Villa model = _mapper.Map<Villa>(updateDTO);



            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return NoContent() ;
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task< IActionResult > UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDto)
        {
            if(patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            if (villa == null)
            {
                return BadRequest();
            }

           
            patchDto.ApplyTo(villaDTO, ModelState);
         
            Villa model = _mapper.Map<Villa>(villaDTO);

            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
