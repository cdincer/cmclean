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
using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using static cmclean.Application.Common.Error.Response.ValidationErrorResponse;
using System.Text;

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

    public override async Task<GetContactByFilterProtoResponse> GetContactByFilter(GetContactByFilterProtoRequest request, ServerCallContext context)
    {
        try
        {
            var filter = _mapper.Map<GetContactByFilterQuery>(request);
            var result = await _mediator.Send(filter);

            var Contacts = _mapper.Map<List<ContactProtoModel>>(result);
            return new GetContactByFilterProtoResponse
            {
                Contacts = { Contacts }
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
            var command = new CreateContactCommand(request.Salutation, request.FirstName, request.LastName, request.DisplayName,
                                                     request.BirthDate.ToDateTime(), request.Email, request.Phonenumber);
            var result = await _mediator.Send(command);
            var Contact = _mapper.Map<CreateContactProtoResponse>(result);
            return Contact;
        }
        catch (ValidationException ex)
        {
            var metadata = new Metadata
            {
                {"exception-type", "ValidationException"},
                {"original-exception", JsonConvert.SerializeObject(ex)}
            };
            string DetailedErrors = ValidationErrorBuilder(ex.ValidationErrorResponse.Errors);
            
            throw new RpcException(new Status(StatusCode.InvalidArgument, DetailedErrors.ToString(), ex), metadata);
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
            var command = new UpdateContactCommand(Guid.Parse(request.Id), request.Salutation, request.FirstName,
                                                   request.LastName, request.DisplayName, request.BirthDate.ToDateTime(),
                                                   request.Email, request.Phonenumber);
            await _mediator.Send(command);
            return new UpdateContactProtoResponse { Status = true, Message = "Update succesfully completed" };
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
            string DetailedErrors = ValidationErrorBuilder(ex.ValidationErrorResponse.Errors);

            throw new RpcException(new Status(StatusCode.InvalidArgument, DetailedErrors, ex), metadata);
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
                Status = true,
                Message = "Delete succesfully completed"
            };
        }
        catch (ValidationException ex)
        {
            var metadata = new Metadata
            {
                {"exception-type", "ValidationException"},
                {"original-exception", JsonConvert.SerializeObject(ex)}
            };
            string DetailedErrors = ValidationErrorBuilder(ex.ValidationErrorResponse.Errors);

            throw new RpcException(new Status(StatusCode.InvalidArgument, DetailedErrors, ex), metadata);
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

    public string ValidationErrorBuilder(List<ValidationError> validationErrors)
    {
        StringBuilder DetailedErrors = new();
        foreach (ValidationError error in validationErrors)
        {
            DetailedErrors.AppendLine(error.ErrorMessage);
        }
        return DetailedErrors.ToString();
    }
}