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
    public class EstadoAnimalController : ControllerBase
    {
        private readonly IEstadoAnimal _estadoAnimal;

        public EstadoAnimalController(IEstadoAnimal estadoAnimal)
        {
          _estadoAnimal = estadoAnimal;

        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("agregarEstado")]

        public async Task<ActionResult<Response<AgregarEstadoAnimalResponse>>> AgregarEstadoAnimal(AgregarEstadoAnimalRequest request)
        {
            var response = await _estadoAnimal.AgregarEstadoAnimal(request);
            if (!response.Successful)
                return BadRequest(response);
            return Ok(response);
           
        }

    }
}
