using AgroTrace.DTO;
using AgroTrace.Service;
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

        [Authorize(Roles = "Administrador,Usuario")]
        [HttpGet("ObtenerRoles")]
        public async Task<ActionResult<Response>> ObtenerRoles()
        {
            var response = await _roles.ObtenerRoles();
            if (!response.Successful)
                return NotFound(response);
            return Ok(response);


        }
    }
}
