using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroTrace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimal _Animal;
        
        public AnimalController(IAnimal animal)
        {
            _Animal = animal;
        }




        [HttpPost("AgregarAnimal")]
        public async Task<ActionResult<Response<AgregarAnimalesResponse>>> AgregarAnimal(AgregarAnimalesRequest request)
        {
            var response = await _Animal.AgregarAnimal(request);
            if (!response.Successful)
                return BadRequest(response);

            return Ok(response);

        }



    }
}
