using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumbersController : ControllerBase
    {

        private readonly ILogger<VillaNumbersController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly IVillaNumberRepositorio _villaNumberRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public VillaNumbersController(ILogger<VillaNumbersController> logger, IVillaRepositorio villaRepo,IMapper mapper,IVillaNumberRepositorio villaNumberRepo)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _villaNumberRepo = villaNumberRepo;
            _mapper = mapper;
            _response = new ();
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                _logger.LogInformation("Obtain all villageNumbers");
                IEnumerable<VillaNumber> villaNumberList = await _villaNumberRepo.GetAll();
                _response.Result = _mapper.Map<IEnumerable<VillaDto>>(villaNumberList);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString()};
             }
            return _response;
           
        }

        [HttpGet("id:int", Name = "GetVillaNumber")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error with number of village id");
                    _response.statusCode=HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var villaNumber = await _villaNumberRepo.Get(v => v.VillaNo == id);

                if (villaNumber == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso=false;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaDto>(villaNumber);
                _response.statusCode= HttpStatusCode.OK;    

                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso=false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
           
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _villaNumberRepo.Get(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("NameExist", "The village with this name exist");
                    return BadRequest(ModelState);
                }
                if (await _villaRepo.Get(v=>v.Id==createDto.VillaId)==null)
                {
                    ModelState.AddModelError("Foreign key", "The village with this id not exist");
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest();
                }
               
                VillaNumber model = _mapper.Map<VillaNumber>(createDto);

                model.CreatedAt = DateTime.Now;
                model.UpdatedAt = DateTime.Now;   
                await _villaNumberRepo.Create(model);
                _response.Result = model;
                _response.statusCode= HttpStatusCode.Created;
                return CreatedAtRoute("GetVillaNumber", new { id = model.VillaNo }, _response);
            }

            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso= false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villaNumber = await _villaNumberRepo.Get(v => v.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.IsExitoso=false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _villaNumberRepo.Remove(villaNumber);
                _response.statusCode=HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
           
        }
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.VillaNo)
            {
                _response.IsExitoso=false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (await _villaRepo.Get(v => v.Id == updateDto.VillaId) == null)
            {
                ModelState.AddModelError("Foreign key", "The village with this id not exist");
                return BadRequest(ModelState);
            }

            VillaNumber model =_mapper.Map<VillaNumber>(updateDto);

        
            await _villaNumberRepo.Update(model);
            _response.statusCode=HttpStatusCode.NoContent;

            return Ok(_response);


        }


    }
}
