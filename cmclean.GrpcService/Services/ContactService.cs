using AutoMapper;
using cmclean.Application.Common.Exceptions;
using cmclean.GrpcService.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Newtonsoft.Json;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Commands.DeleteContact;
using cmclean.Application.Features.ContactFeature.Commands.UpdateContact;
using cmclean.Application.Features.ContactFeature.Queries.GetAllContacts;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;

namespace cmclean.GrpcService.Services;


public class ContactService : ContactProtoService.ContactProtoServiceBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ContactService(IMediator mediator, IMapper mapper, ILogger<ContactService> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task<GetAllContactsProtoResponse> GetContacts(Empty request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetAllContactsQuery());
        var Contacts = _mapper.Map<List<ContactProtoModel>>(result);
        return new GetAllContactsProtoResponse
        {
            Contacts = { Contacts }
        };
    }

    public override async Task<GetContactByIdProtoResponse> GetContactById(GetContactByIdProtoRequest request, ServerCallContext context)
    {
        try
        {
            var result = await _mediator.Send(new GetContactByIdQuery(Guid.Parse(request.ContactId)));

            var Contact = _mapper.Map<ContactProtoModel>(result);
            return new GetContactByIdProtoResponse
            {
                Contact = Contact
            };
        }
        catch (NotFoundException ex)
        {
            var metadata = new Metadata
            {
                {"exception-type", "NotFoundException"},
                {"original-exception", JsonConvert.SerializeObject(ex)}
            };
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message, ex), metadata);
        }
        catch (Exception ex)
        {
            var metadata = new Metadata
            {
                {"exception-type", "Exception"}
            };
            throw new RpcException(new Status(StatusCode.Internal, ex.Message, ex), metadata);
        }
    }

    public override async Task<CreateContactProtoResponse> CreateContact(CreateContactProtoRequest request, ServerCallContext context)
    {
        try
        {
            var command = new CreateContactCommand(request.FirstName, request.LastName,
                request.DateOfBirth.ToDateTime(), request.DisplayName);
            var result = await _mediator.Send(command);
            var Contact = _mapper.Map<ContactProtoModel>(result);
            return new CreateContactProtoResponse { Contact = Contact };
        }
        catch (ValidationException ex)
        {
            var metadata = new Metadata
            {
                {"exception-type", "ValidationException"},
                {"original-exception", JsonConvert.SerializeObject(ex)}
            };
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message, ex), metadata);
        }
        catch (Exception ex)
        {
            var metadata = new Metadata
            {
                {"exception-type", "Exception"}
            };
            throw new RpcException(new Status(StatusCode.Internal, ex.Message, ex), metadata);
        }
    }

    public override async Task<UpdateContactProtoResponse> UpdateContact(UpdateContactProtoRequest request, ServerCallContext context)
    {
        try
        {
            var command = new UpdateContactCommand(Guid.Parse(request.Id), request.FirstName, request.LastName, request.DateOfBirth.ToDateTime());
            await _mediator.Send(command);
            return new UpdateContactProtoResponse { Status = true };
        }
        catch (NotFoundException ex)
        {
            var metadata = new Metadata
            {
                {"exception-type", "NotFoundException"},
                {"original-exception", JsonConvert.SerializeObject(ex)}
            };
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message, ex), metadata);
        }
        catch (ValidationException ex)
        {
            var metadata = new Metadata
            {
                {"exception-type", "ValidationException"},
                {"original-exception", JsonConvert.SerializeObject(ex)}
            };
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message, ex), metadata);
        }
        catch (Exception ex)
        {
            var metadata = new Metadata
            {
                {"exception-type", "Exception"}
            };
            throw new RpcException(new Status(StatusCode.Internal, ex.Message, ex), metadata);
        }
    }

    public override async Task<DeleteContactProtoResponse> DeleteContact(DeleteContactProtoRequest request, ServerCallContext context)
    {
        try
        {
            var command = new DeleteContactCommand(Guid.Parse(request.Id));
            await _mediator.Send(command);
            return new DeleteContactProtoResponse
            {
                Status = true
            };
        }
        catch (NotFoundException ex)
        {
            var metadata = new Metadata
            {
                {"exception-type", "NotFoundException"},
                {"original-exception", JsonConvert.SerializeObject(ex)}
            };
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message, ex), metadata);
        }
        catch (Exception ex)
        {
            var metadata = new Metadata
            {
                {"exception-type", "Exception"}
            };
            throw new RpcException(new Status(StatusCode.Internal, ex.Message, ex), metadata);
        }
    }
}