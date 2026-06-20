using AutoMapper;
using Grpc.Core;
using MediatR;
using SuperGiros.Transfer.Services.gRPC.Protos;
using SuperGiros.Transfer.Application.UseCases.Features.Transaction.Commands.CreateTransaction;
using SuperGiros.Transfer.Application.UseCases.Features.Transaction.Commands.CancelTransaction;
using SuperGiros.Transfer.Application.UseCases.Features.Transaction.Commands.UpdateTransaction;
using SuperGiros.Transfer.Application.UseCases.Features.Transaction.Querys.GetTransaction;
using SuperGiros.Transfer.Application.UseCases.Features.Transaction.Querys.GetAllTransaction;

namespace SuperGiros.Transfer.Services.gRPC.Services
{
    public class TransactionService : Transactions.TransactionsBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public TransactionService(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public override async Task<GetAllTransactionResponse> GetAllTransaction(GetAllTransactionRequest request, ServerCallContext context)
        {
            var transactionDtos = await _mediator.Send(new GetAllTransactionQuery());
            var response = new GetAllTransactionResponse();
            var serverResponse = new ServerResponse();

            if (transactionDtos != null && transactionDtos.Any())
            {
                serverResponse.IsSuccess = true;
                serverResponse.Message = "Consulta Exitosa";
                response.Data.AddRange(_mapper.Map<List<TransactionResponse>>(transactionDtos));
            }
            else
            {
                serverResponse.Message = "No se encontraron transacciones";
            }

            response.ServerResponse = serverResponse;
            return response;
        }

        public override async Task<GetTransactionResponse> GetTransaction(GetTransactionRequest request, ServerCallContext context)
        {
            var transactionDto = await _mediator.Send(new GetTransactionQuery() { Id = request.Id });
            var response = new GetTransactionResponse();
            var serverResponse = new ServerResponse();

            if (transactionDto is null)
            {
                serverResponse.Message = $"No se encontró la Transacción #: {request.Id}";
                response.ServerResponse = serverResponse;
                return response;
            }

            response.Data = _mapper.Map<TransactionResponse>(transactionDto);
            serverResponse.IsSuccess = true;
            serverResponse.Message = "Consulta Exitosa";
            response.ServerResponse = serverResponse;
            return response;
        }

        public override async Task<CreateTransactionResponse> CreateTransaction(CreateTransactionRequest request, ServerCallContext context)
        {
            var createTransactionCommand = _mapper.Map<CreateTransactionCommand>(request);
            var estado = await _mediator.Send(createTransactionCommand);

            var response = new CreateTransactionResponse();
            var serverResponse = new ServerResponse();

            if (estado)
            {
                serverResponse.IsSuccess = true;
                serverResponse.Message = "Transacción registrada exitosamente";
            }
            else
            {
                serverResponse.Message = "Error al crear la Transacción";
            }

            response.ServerResponse = serverResponse;
            return response;
        }

        public override async Task<UpdateTransactionResponse> UpdateTransaction(UpdateTransactionRequest request, ServerCallContext context)
        {
            var command = _mapper.Map<UpdateTransactionCommand>(request);
            var estado = await _mediator.Send(command);

            var response = new UpdateTransactionResponse();
            var serverResponse = new ServerResponse();

            if (estado)
            {
                var transactionDto = await _mediator.Send(new GetTransactionQuery() { Id = request.Id });
                if (transactionDto != null)
                {
                    response.Data = _mapper.Map<TransactionResponse>(transactionDto);
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
                serverResponse.Message = $"No se encontró la Transacción #: {request.Id}";
            }

            response.ServerResponse = serverResponse;
            return response;
        }

        public override async Task<CancelTransactionResponse> CancelTransaction(CancelTransactionRequest request, ServerCallContext context)
        {
            var estado = await _mediator.Send(new CancelTransactionCommand() { Id = request.Id });

            var response = new CancelTransactionResponse();
            var serverResponse = new ServerResponse();

            if (estado)
            {
                serverResponse.IsSuccess = true;
                serverResponse.Message = "Transacción eliminada correctamente";
            }
            else
            {
                serverResponse.Message = $"No se encontró la Transacción #: {request.Id}";
            }

            response.ServerResponse = serverResponse;
            return response;
        }
    }
}
