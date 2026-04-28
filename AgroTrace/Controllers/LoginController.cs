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
    
    public class LoginController : ControllerBase
    {
        private readonly ITokenGenerator _tokenService;
        private readonly IUsuario _usuario;
        public LoginController(ITokenGenerator tokenService, IUsuario usuario)
        {
            _tokenService = tokenService;
            _usuario = usuario;
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

        [AllowAnonymous]
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

