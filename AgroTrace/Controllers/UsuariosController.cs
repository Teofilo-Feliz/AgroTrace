using AgroTrace.DTO;
using AgroTrace.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroTrace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuario _usuario;
        public UsuariosController(IUsuario usuario)
        {
            _usuario = usuario;
        }


        [HttpGet("Usuarios")]
        public async Task<IActionResult> ObtenerUsuarios([FromQuery] UsuarioFiltro filtro)
        {
            var response = await _usuario.ObtenerUsuarios(filtro);

            if (!response.Successful)

                return NotFound(response);

            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> ObtenerUsuariosId(int id)
        {
            var response = await _usuario.ObtenerUsuariosId(id);

            if (!response.Successful)
            {
                if (response.Message == "Usuario no encontrado")
                    return NotFound(response);

                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost("AgregarUsuario")]
        public async Task <ActionResult<Response<AgregarUsuarioResponse>>> AgregarUsuario(AgregarUsuarioRequest request)
        {
            var response = await _usuario.AgregarUsuario(request);
            if (!response.Successful)
                return BadRequest(response);

            return Ok(response);




        }
    }
}
