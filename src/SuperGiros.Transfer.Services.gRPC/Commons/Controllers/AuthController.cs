using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SuperGiros.Transfer.Services.gRPC.Commons.Auth;
using SuperGiros.Transfer.Application.UseCases.Features.Users.Querys.GetUserByUsername;

namespace SuperGiros.Transfer.Services.gRPC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IMediator _mediator;

        public AuthController(ITokenService tokenService, IMediator mediator)
        {
            _tokenService = tokenService;
            _mediator = mediator;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModels.LoginRequest request)
        {
            var user = await _mediator.Send(new GetUserByUsernameQuery(request.Username));

            // Validación con BCrypt (Guía 07)
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized(new LoginModels.LoginResponse
                {
                    IsSuccess = false,
                    Message = "Credenciales inválidas"
                });
            }

            var token = _tokenService.GenerateToken(user.Username, user.Role);

            return Ok(new LoginModels.LoginResponse
            {
                IsSuccess = true,
                Token = token,
                Message = "Autenticación exitosa",
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            });
        }
    }
}