using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace AgroTrace.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class AnimalController : ControllerBase
    {
        private readonly IAnimal _Animal;
        
        public AnimalController(IAnimal animal)
        {
            _Animal = animal;
        }


        [Authorize(Roles = "Usuario, Administrador")]
        [HttpGet("ObtenerAnimal")]
        public async Task<ActionResult<Response<ObtenerAnimalResponse>>> ObtenerAnimales([FromQuery]FiltroAnimal filtro)
        {
            var response = await _Animal.ObtenerAnimales(filtro);
            if (!response.Successful)
            
                return BadRequest(response);

            
            return Ok(response);


        }


        [Authorize(Roles = "Administrador")]
        [HttpPost("AgregarAnimal")]
        public async Task<ActionResult<Response<AgregarAnimalesResponse>>> AgregarAnimal(AgregarAnimalesRequest request)
        {
            var response = await _Animal.AgregarAnimal(request);
            if (!response.Successful)
                return BadRequest(response);

            return Ok(response);

        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("actualizarAnimal")]

        public async Task<ActionResult<Response<ActualizarAnimalResponse>>> ActualizarAnimal([FromQuery]int id, ActualizarAnimalRequest request)
        {
            var response = await _Animal.ActualizarAnimal(id, request);
            if (!response.Successful)
                return BadRequest(response);

            return Ok(response);

        }



    }
}
