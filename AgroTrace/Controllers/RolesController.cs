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
   

    public class RolesController : ControllerBase
    {
        private readonly IRoles _roles;
        public RolesController(IRoles roles)
        {
            _roles = roles;
        }

        [Authorize(Roles = "Administrador,Gerente")]
        [HttpGet("ObtenerRoles")]
        public async Task<ActionResult<Response>> ObtenerRoles([FromQuery]Filtro filtro)
        {
            var response = await _roles.ObtenerRoles(filtro);
            if (!response.Successful)
                return BadRequest(response);
            return Ok(response);


        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("AgregarRol")]
        public async Task<ActionResult<Response>> AgregarRol(AgregarRolRequest rol)
        {
            var response = await _roles.AgregarRol(rol);
            if (!response.Successful)
                return BadRequest(response);
            return Ok(response);

        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("ActualizarUsuario")]
        public async Task<ActionResult<Response>> ActualizarRol(int id, AgregarRolRequest rol)
        {
            var response = await _roles.ActualizarRol(id, rol);
            if (!response.Successful)
                return BadRequest(response);
            return Ok(response);
        } 
    }
}
