using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {

        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext db;
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext _db, IMapper mapper)
        {
            _logger = logger;
            db = _db;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obtener las Villas");

            IEnumerable<Villa> villaList = await db.Villas.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer Villa con Id" + id);
                return BadRequest();

            }

            var data = await db.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {

                return NotFound();

            }

            return Ok(_mapper.Map<VillaDto>(data));

        }//Fin GetVilla

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto credateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await db.Villas.FirstOrDefaultAsync(x => x.Nombre.ToLower() == credateDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La Villa con ese nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (credateDto == null)
            {

                return BadRequest(credateDto);

            }

            /* Para esto sirve el mapeo para pasarle el registro que creamos al modelo en una sola linea,
            asi ya no es necesario pasarlo uno a uno como esta comentado abajo */

            Villa modelo = _mapper.Map<Villa>(credateDto);

            //Villa modelo = new()
            //{
            //    Nombre = credateDto.Nombre,
            //    Detalle = credateDto.Detalle,
            //    ImageUrl = credateDto.ImageUrl,
            //    Ocupantes = credateDto.Ocupantes,
            //    Tarifa = credateDto.Tarifa,
            //    MetrosCuadrados = credateDto.MetrosCuadrados,
            //    Amenidad = credateDto.Amenidad
            //};

            await db.Villas.AddAsync(modelo);
            await db.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
        }//Fin CrearVilla

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await db.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null)
            {

                return NotFound();

            }
            db.Villas.Remove(villa);
            await db.SaveChangesAsync();

            return NoContent();
        }//Fin DeleteVilla


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.Id)
            {
                return BadRequest();
            }

            Villa modelo = _mapper.Map<Villa>(updateDto);

            db.Villas.Update(modelo);
            await db.SaveChangesAsync();

            return NoContent();
        }//Fin UpdateVilla


        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var villa = await db.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = _mapper.Map<Villa>(villaDto);

            db.Villas.Update(modelo);
            await db.SaveChangesAsync();

            return NoContent();
        }//Fin UpdatePartialVilla
    }
}
