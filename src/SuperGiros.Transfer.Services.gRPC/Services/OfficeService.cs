using AutoMapper;
using Grpc.Core;
using MediatR;
using SuperGiros.Transfer.Services.gRPC.Protos;
using SuperGiros.Transfer.Application.UseCases.Features.Offices.Commands.CreateOffice;
using SuperGiros.Transfer.Application.UseCases.Features.Offices.Commands.CancelOffice;
using SuperGiros.Transfer.Application.UseCases.Features.Offices.Commands.UpdateOffice;
using SuperGiros.Transfer.Application.UseCases.Features.Offices.Querys.GetOffice;
using SuperGiros.Transfer.Application.UseCases.Features.Offices.Querys.GetAllOffice;

namespace SuperGiros.Transfer.Services.gRPC.Services
{
    public class OfficeService : Offices.OfficesBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public OfficeService(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public override async Task<GetAllOfficeResponse> GetAllOffice(GetAllOfficeRequest getAllOfficeRequest, ServerCallContext context)
        {
            var officeDtos = await _mediator.Send(new GetAllOfficeQuery());
            var response = new GetAllOfficeResponse();
            var serverResponse = new ServerResponse();

            if (officeDtos != null && officeDtos.Any())
            {
                serverResponse.IsSuccess = true;
                serverResponse.Message = "Consulta Exitosa";
                response.Data.AddRange(_mapper.Map<List<OfficeResponse>>(officeDtos));
            }
            else
            {
                serverResponse.Message = "No se encontraron oficinas";
            }

            response.ServerResponse = serverResponse;
            return response;
        }

        public override async Task<GetOfficeResponse> GetOffice(GetOfficeRequest getOfficeRequest, ServerCallContext context)
        {
            var officeDto = await _mediator.Send(new GetOfficeQuery() { Id = getOfficeRequest.Id });
            var response = new GetOfficeResponse();
            var serverResponse = new ServerResponse();

            if (officeDto is null)
            {
                serverResponse.Message = $"No se encontró la Oficina #: {getOfficeRequest.Id}";
                response.ServerResponse = serverResponse;
                return response;
            }

            response.Data = _mapper.Map<OfficeResponse>(officeDto);
            serverResponse.IsSuccess = true;
            serverResponse.Message = "Consulta Exitosa";
            response.ServerResponse = serverResponse;
            return response;
        }

        public override async Task<CreateOfficeResponse> CreateOffice(CreateOfficeRequest createOfficeRequest, ServerCallContext context)
        {
            var createOfficeCommand = _mapper.Map<CreateOfficeCommand>(createOfficeRequest);
            var newId = await _mediator.Send(createOfficeCommand);

            var response = new CreateOfficeResponse();
            var serverResponse = new ServerResponse();

            if (newId > 0)
            {
                var officeDto = await _mediator.Send(new GetOfficeQuery() { Id = newId });
                if (officeDto != null)
                {
                    response.Data = _mapper.Map<OfficeResponse>(officeDto);
                    serverResponse.IsSuccess = true;
                    serverResponse.Message = "Registro Exitoso";
                }
                else
                {
                    serverResponse.Message = "Oficina creada pero no se pudo recuperar";
                }
            }
            else
            {
                serverResponse.Message = "Error al crear la Oficina";
            }

            response.ServerResponse = serverResponse;
            return response;
        }

        public override async Task<UpdateOfficeResponse> UpdateOffice(UpdateOfficeRequest request, ServerCallContext context)
        {
            var command = _mapper.Map<UpdateOfficeCommand>(request);
            var estado = await _mediator.Send(command);

            var response = new UpdateOfficeResponse();
            var serverResponse = new ServerResponse();

            if (estado)
            {
                var officeDto = await _mediator.Send(new GetOfficeQuery() { Id = request.Id });
                if (officeDto != null)
                {
                    response.Data = _mapper.Map<OfficeResponse>(officeDto);
                    serverResponse.IsSuccess = true;
                    serverResponse.Message = "Actualización Exitosa";
                }
                else
                {
                    serverResponse.Message = "Se actualizó pero no se pudo recuperar para la respuesta";
                }
            }
            else
            {
                serverResponse.Message = $"No se encontró la Oficina #: {request.Id}";
            }

            response.ServerResponse = serverResponse;
            return response;
        }

        public override async Task<CancelOfficeResponse> CancelOffice(CancelOfficeRequest cancelOfficeRequest, ServerCallContext context)
        {
            var estado = await _mediator.Send(new CancelOfficeCommand() { Id = cancelOfficeRequest.Id });

            var response = new CancelOfficeResponse();
            var serverResponse = new ServerResponse();

            if (estado)
            {
                serverResponse.IsSuccess = true;
                serverResponse.Message = "Oficina desactivada correctamente (Estado: Inactiva)";
            }
            else
            {
                serverResponse.Message = $"No se encontró la Oficina #: {cancelOfficeRequest.Id}";
            }

            response.ServerResponse = serverResponse;
            return response;
        }
    }
}
