using AutoMapper;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SuperGiros.Transfer.Services.gRPC.Protos;
using SuperGiros.Transfer.Application.UseCases.Features.Customer.Commands.CreateCustomer;
using SuperGiros.Transfer.Application.UseCases.Features.Customer.Commands.CancelCustomer;
using SuperGiros.Transfer.Application.UseCases.Features.Customer.Commands.UpdateCustomer;
using SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetCustomer;
using SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetAllCustomer;
using SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetCustomerByDni;

namespace SuperGiros.Transfer.Services.gRPC.Services
{
    [Authorize]
    public class CustomerService : Customers.CustomersBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CustomerService(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public override async Task<GetAllCustomerResponse> GetAllCustomer(GetAllCustomerRequest request, ServerCallContext context)
        {
            var customerDtos = await _mediator.Send(new GetAllCustomerQuery());
            var response = new GetAllCustomerResponse();
            var serverResponse = new ServerResponse();

            if (customerDtos != null && customerDtos.Any())
            {
                serverResponse.IsSuccess = true;
                serverResponse.Message = "Consulta Exitosa";
                response.Data.AddRange(_mapper.Map<List<CustomerResponse>>(customerDtos));
            }
            else
            {
                serverResponse.IsSuccess = false;
                serverResponse.Message = "No se encontraron clientes";
            }

            response.ServerResponse = serverResponse;
            return response;
        }

        public override async Task<GetCustomerResponse> GetCustomer(GetCustomerRequest request, ServerCallContext context)
        {
            // Mantiene la búsqueda tradicional por llave primaria (ID interno)
            var customerDto = await _mediator.Send(new GetCustomerQuery { Id = request.Id });
            var response = new GetCustomerResponse();
            var serverResponse = new ServerResponse();

            if (customerDto is null)
            {
                serverResponse.IsSuccess = false;
                serverResponse.Message = "Cliente no encontrado por ID.";
                response.ServerResponse = serverResponse;
                return response;
            }

            response.Data = _mapper.Map<CustomerResponse>(customerDto);
            serverResponse.IsSuccess = true;
            serverResponse.Message = "Cliente obtenido correctamente.";
            response.ServerResponse = serverResponse;
            return response;
        }

        public override async Task<GetCustomerResponse> GetCustomerByDni(GetCustomerByDniRequest request, ServerCallContext context)
        {
            // Endpoint semántico exclusivo para la ventanilla que mapea por DNI
            var customerDto = await _mediator.Send(new GetCustomerByDniQuery { NroDocumento = request.NroDocumento });
            var response = new GetCustomerResponse();
            var serverResponse = new ServerResponse();

            if (customerDto is null)
            {
                serverResponse.IsSuccess = false;
                serverResponse.Message = "Cliente no encontrado. Se requiere registro manual.";
                response.ServerResponse = serverResponse;
                return response;
            }

            response.Data = _mapper.Map<CustomerResponse>(customerDto);
            serverResponse.IsSuccess = true;
            serverResponse.Message = "Cliente encontrado con éxito";
            response.ServerResponse = serverResponse;
            return response;
        }

        public override async Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        {
            var createCustomerCommand = _mapper.Map<CreateCustomerCommand>(request);
            var estado = await _mediator.Send(createCustomerCommand);

            var response = new CreateCustomerResponse();
            var serverResponse = new ServerResponse();

            if (estado)
            {
                serverResponse.IsSuccess = true;
                serverResponse.Message = "Registro Exitoso";
            }
            else
            {
                serverResponse.IsSuccess = false;
                serverResponse.Message = "Error al crear el Cliente";
            }

            response.ServerResponse = serverResponse;
            return response;
        }

        public override async Task<UpdateCustomerResponse> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
        {
            var command = _mapper.Map<UpdateCustomerCommand>(request);
            var estado = await _mediator.Send(command);

            var response = new UpdateCustomerResponse();
            var serverResponse = new ServerResponse();

            if (estado)
            {
                var customerDto = await _mediator.Send(new GetCustomerQuery() { Id = request.Id });
                if (customerDto != null)
                {
                    response.Data = _mapper.Map<CustomerResponse>(customerDto);
                    serverResponse.IsSuccess = true;
                    serverResponse.Message = "Actualización Exitosa";
                }
                else
                {
                    serverResponse.IsSuccess = false;
                    serverResponse.Message = "Se actualizó pero no se pudo recuperar para la respuesta";
                }
            }
            else
            {
                serverResponse.IsSuccess = false;
                serverResponse.Message = $"No se encontró el Cliente #: {request.Id}";
            }

            response.ServerResponse = serverResponse;
            return response;
        }

        public override async Task<CancelCustomerResponse> CancelCustomer(CancelCustomerRequest request, ServerCallContext context)
        {
            var estado = await _mediator.Send(new CancelCustomerCommand() { Id = request.Id });

            var response = new CancelCustomerResponse();
            var serverResponse = new ServerResponse();

            if (estado)
            {
                serverResponse.IsSuccess = true;
                serverResponse.Message = "Cliente eliminado correctamente";
            }
            else
            {
                serverResponse.IsSuccess = false;
                serverResponse.Message = $"No se encontró el Cliente #: {request.Id}";
            }

            response.ServerResponse = serverResponse;
            return response;
        }
    }
}