using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using SuperGiros.Transfer.Services.gRPC.Protos;
using SuperGiros.Transfer.Application.UseCases.Features.Offices.Commands.CreateOffice;
using SuperGiros.Transfer.Application.UseCases.Features.Offices.Commands.UpdateOffice;
using SuperGiros.Transfer.Application.UseCases.Features.Offices.Querys.GetOffice;
using SuperGiros.Transfer.Application.UseCases.Features.Offices.Querys.GetAllOffice;
using SuperGiros.Transfer.Application.UseCases.Features.Customer.Commands.CreateCustomer;
using SuperGiros.Transfer.Application.UseCases.Features.Customer.Commands.UpdateCustomer;
using SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetCustomer;
using SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetAllCustomer;
using SuperGiros.Transfer.Application.UseCases.Features.Transaction.Commands.CreateTransaction;
using SuperGiros.Transfer.Application.UseCases.Features.Transaction.Commands.UpdateTransaction;
using SuperGiros.Transfer.Application.UseCases.Features.Transaction.Querys.GetTransaction;
using SuperGiros.Transfer.Application.UseCases.Features.Transaction.Querys.GetAllTransaction;
using DomainEnums = SuperGiros.Transfer.Domain.Enums;

namespace SuperGiros.Transfer.Services.gRPC.Commons.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DateTime, Timestamp>()
                .ConvertUsing(x => Timestamp.FromDateTime(DateTime.SpecifyKind(x, DateTimeKind.Utc)));
            CreateMap<Timestamp, DateTime>()
                .ConvertUsing(x => x != null ? x.ToDateTime() : DateTime.UtcNow);

            CreateMap<decimal, double>().ConvertUsing(x => (double)x);
            CreateMap<double, decimal>().ConvertUsing(x => (decimal)x);

            // Conversión explícita de OfficeStatus proto <-> dominio
            CreateMap<OfficeStatus, DomainEnums.OfficeStatus>()
                .ConvertUsing(src => (DomainEnums.OfficeStatus)(int)src);
            CreateMap<DomainEnums.OfficeStatus, OfficeStatus>()
                .ConvertUsing(src => (OfficeStatus)(int)src);

            // Salida: DTO -> Proto Response
            CreateMap<GetAllOfficeResponseDto, OfficeResponse>()
                .ForMember(d => d.Estado, o => o.MapFrom(s => (OfficeStatus)(int)s.Estado));
            CreateMap<GetOfficeResponseDto, OfficeResponse>()
                .ForMember(d => d.Estado, o => o.MapFrom(s => (OfficeStatus)(int)s.Estado));

            CreateMap<GetAllCustomerResponseDto, CustomerResponse>();
            CreateMap<GetCustomerResponseDto, CustomerResponse>();

            CreateMap<GetAllTransactionResponseDto, TransactionResponse>();
            CreateMap<GetTransactionResponseDto, TransactionResponse>();

            // Entrada: Proto Request -> Command
            CreateMap<CreateOfficeRequest, CreateOfficeCommand>()
                .ForMember(d => d.Estado, o => o.MapFrom(s => (DomainEnums.OfficeStatus)(int)s.Estado));
            CreateMap<UpdateOfficeRequest, UpdateOfficeCommand>()
                .ForMember(d => d.Estado, o => o.MapFrom(s => (DomainEnums.OfficeStatus)(int)s.Estado));

            CreateMap<CreateCustomerRequest, CreateCustomerCommand>();
            CreateMap<UpdateCustomerRequest, UpdateCustomerCommand>();

            CreateMap<CreateTransactionRequest, CreateTransactionCommand>();
            CreateMap<UpdateTransactionRequest, UpdateTransactionCommand>();
        }
    }
}
