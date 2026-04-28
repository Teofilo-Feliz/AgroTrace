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

        [Authorize(Roles = "Administrador")]
        [HttpPost("agregarFinca")]
        public async Task<ActionResult<Response<AgregarFincaResponse>>> AgregarFinca(AgregarFincaRequest finca)
        {
            var response = await _finca.AgregarFinca(finca);
            if (!response.Successful)
                return BadRequest(response);

            return Ok(response);
        }



    }
}
