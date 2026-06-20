using Grpc.Core;
using MediatR;
using SuperGiros.Transfer.Application.UseCases.Features.Users.Commands.ChangeUserState;
using SuperGiros.Transfer.Application.UseCases.Features.Users.Commands.CreateUser;
using SuperGiros.Transfer.Application.UseCases.Features.Users.Commands.UpdateUser;
using SuperGiros.Transfer.Application.UseCases.Features.Users.Querys.GetUser;
using SuperGiros.Transfer.Application.UseCases.Features.Users.Querys.GetUserByUsername;
using SuperGiros.Transfer.Services.gRPC.Commons.Auth;
using SuperGiros.Transfer.Services.gRPC.Protos;
namespace SuperGiros.Transfer.Services.gRPC.Services
{
    public class UserService : UserServices.UserServicesBase
    {
        // En UserService.cs
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;

        public UserService(IMediator mediator, ITokenService tokenService)
        {
            _mediator = mediator;
            _tokenService = tokenService;
        }

        // 1. LOGIN: Busca el usuario para iniciar el flujo de autenticación
        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var user = await _mediator.Send(new GetUserByUsernameQuery(request.Username));

            if (user == null)
                return new LoginResponse { Success = false, Message = "Usuario no encontrado" };

            // Aquí generaríamos el token con el rol real de la BD
            var token = _tokenService.GenerateToken(user.Username, user.Role);

            return new LoginResponse
            {
                Success = true,
                Token = token, // <-- ESTO ES LO QUE LEERÁ POSTMAN
                Message = "Login exitoso",
                Role = user.Role
            };
        }

        // 2. CREATE: Registra un nuevo usuario con hasheo automático
        public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            var command = new CreateUserCommand
            {
                Username = request.Username,
                Password = request.Password,
                Role = request.Role
            };

            var userId = await _mediator.Send(command);

            return new CreateUserResponse
            {
                Id = userId,
                Message = "Usuario creado exitosamente con auditoría inicial"
            };
        }

        // 3. UPDATE: Actualiza datos y gestiona cambio de password
        public override async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var command = new UpdateUserCommand
            {
                Id = request.Id,
                Username = request.Username,
                Password = request.Password,
                Role = request.Role
            };

            var result = await _mediator.Send(command);

            return new UpdateUserResponse
            {
                Success = result,
                Message = result ? "Perfil actualizado correctamente" : "No se pudo actualizar el usuario"
            };
        }

        // 4. GET USERS: Obtiene la lista completa de usuarios registrados[cite: 1]
        public override async Task<GetUsersResponse> GetUsers(GetUsersRequest request, ServerCallContext context)
        {
            var users = await _mediator.Send(new GetUsersQuery());
            var response = new GetUsersResponse();

            foreach (var user in users)
            {
                response.Users.Add(new GetUsersResponse.Types.UserInfo
                {
                    Id = user.Id,
                    Username = user.Username,
                    Role = user.Role,
                    State = (int)user.State
                });
            }

            return response;
        }

        // 5. CHANGE STATE: Activa o suspende usuarios (Dispara auditoría)[cite: 1]
        public override async Task<ChangeUserStateResponse> ChangeUserState(ChangeUserStateRequest request, ServerCallContext context)
        {
            var command = new ChangeUserStateCommand
            {
                Id = request.Id,
                NewState = (Domain.Enums.State)request.NewState
            };

            var result = await _mediator.Send(command);

            return new ChangeUserStateResponse
            {
                Success = result,
                Message = result ? "Estado de usuario modificado" : "Error al cambiar el estado"
            };
        }
    }
}
