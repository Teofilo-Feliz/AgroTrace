using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroTrace.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FincaController : ControllerBase
    {
        private readonly IFinca _finca;

        public FincaController(IFinca finca)
        { 
         _finca = finca;
        }

        [Authorize(Roles = "Administrador, Usuario")]
        [HttpGet("obtenerFincas")]
        public async Task<ActionResult<Response<FincasResponse>>> ObtenerFincas([FromQuery] Filtro filtro)
        {
            var response = await _finca.ObtenerFincas(filtro);
            if (!response.Successful)
                return BadRequest(response);

            return Ok(response);
        }





        [Authorize(Roles = "Administrador")]
        [HttpPost("agregarFinca")]
        public async Task<ActionResult<Response<AgregarFincaResponse>>> AgregarFinca(AgregarFincaRequest finca)
        {
            var response = await _finca.AgregarFinca(finca);
            if (!response.Successful)
                return BadRequest(response);

            return Ok(response);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("actualizar")]

        public async Task<ActionResult<Response<ActualizarAnimalRequest>>> ActualizarFinca(ActualizarFincaRequest finca, int id)
        {
            var response = await _finca.ActualizarFinca(finca, id);
            if (!response.Successful)
                return BadRequest(response);

            return Ok(response);
        }



    }
}
