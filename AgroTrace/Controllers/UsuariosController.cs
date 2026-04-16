using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Interfaces;
using AgroTrace.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace AgroTrace.Controllers

{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
   
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuario _usuario;
        private readonly ITokenGenerator _tokenService;
        public UsuariosController(IUsuario usuario, ITokenGenerator tokenGenerator)
        {
            _usuario = usuario;
            _tokenService = tokenGenerator;
        }

        [Authorize(Roles = "Administrador,Usuario")]
        [HttpGet("Usuarios")]
        public async Task<IActionResult> ObtenerUsuarios([FromQuery] UsuarioFiltro filtro)
        {
            var response = await _usuario.ObtenerUsuarios(filtro);

            if (!response.Successful)

                return NotFound(response);

            return Ok(response);
        }

        [Authorize(Roles = "Administrador,Usuario")]
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

        [Authorize(Roles = "Administrador")]
        [HttpPost("AgregarUsuario")]
        public async Task <ActionResult<Response<AgregarUsuarioResponse>>> AgregarUsuario(AgregarUsuarioRequest request)
        {
            var response = await _usuario.AgregarUsuario(request);
            if (!response.Successful)
                return BadRequest(response);

            return Ok(response);

        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("ActualizarUsuario")]
        public async Task<ActionResult<Response<ActualizarUsuarioResponse>>> ActualizarUsuario(int id, ActualizarUsuarioRequest request)
        { 
           var response = await _usuario.ActualizarUsuario(id, request);
            if (!response.Successful)
                return BadRequest(response);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<Response<LoginResponse>>> LoguearUsuario([FromBody] LoginRequest request)
        {
            var response = await _usuario.LoguearUsuario(request.Username, request.Password);

            if (!response.Successful)
                return Unauthorized(response);

            return Ok(response);
        }

        [Authorize(Roles = "Administrador,Usuario")]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<Response<LoginResponse>>> RefreshToken([FromBody] RefreshRequest request)
        {
            var response = await _tokenService.RefreshToken(request.RefreshToken);

            if (!response.Successful)
                return Unauthorized(response);

            return Ok(response);
        }
    }
}
