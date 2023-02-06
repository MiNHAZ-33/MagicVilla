using AutoMapper;
using MagicVilla_Villa_API.Models;
using MagicVilla_Villa_API.Models.DTO;
using MagicVilla_Villa_API.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_Villa_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;

        private readonly IVillaRespository _dbVilla;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaAPIController(ILogger<VillaAPIController> logger, IVillaRespository dbVilla, IMapper mapper)
        {
            _logger = logger;
            _dbVilla= dbVilla;
            _mapper = mapper;
            this._response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            _logger.LogInformation("Getting all villas");

            try
            {
                IEnumerable<Villa> villaList = await _dbVilla.GetAll();

                _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess= false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }

            return _response;

           
        }
        
        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task <ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {


                if (id == 0)
                {
                    _logger.LogError("Get villa error with id " + id);
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _dbVilla.Get(u => u.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO) {

            try
            {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }

            Villa villa = _mapper.Map<Villa>(createDTO);


            await _dbVilla.Create(villa);

            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
        }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }

            return _response;
        }
        [HttpDelete("{id}", Name = "DeleteVilla")]
        public async Task< ActionResult<APIResponse> > DeleteVillaById(int id)
        {
            try
            {


                var villla = await _dbVilla.Get(u => u.Id == id);
                if (villla is null)
                {
                    return BadRequest($"{id} not found");
                }
                await _dbVilla.Remove(villla);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("{id:int}", Name="UpdateVilla")]
        public async Task< ActionResult<APIResponse>> UpdateVilla(int id, [FromBody]VillaUpdateDTO updateDTO)
        {
            try
            {

                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                Villa model = _mapper.Map<Villa>(updateDTO);

                await _dbVilla.Update(model);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            
                }
                 catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }

            return _response;
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

            var villa = await _dbVilla.Get(u => u.Id == id, tracked: false);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            if (villa == null)
            {
                return BadRequest();
            }

           
            patchDto.ApplyTo(villaDTO, ModelState);
         
            Villa model = _mapper.Map<Villa>(villaDTO);

            await _dbVilla.Update(model);
 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
