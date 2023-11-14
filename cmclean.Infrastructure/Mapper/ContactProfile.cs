using AutoMapper;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Commands.DeleteContact;
using cmclean.Application.Features.ContactFeature.Commands.UpdateContact;
using cmclean.Application.Features.ContactFeature.Queries.GetAllContacts;
using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;
using cmclean.Infrastructure.Protos;
using Google.Protobuf.WellKnownTypes;


namespace cmclean.Infrastructure.Mapper;


public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<ContactProtoModel, GetAllContactsResponse>()
            .ForMember(dest => dest.DateOfBirth, opt =>
                opt.MapFrom(src => src.DateOfBirth.ToDateTime()));

        CreateMap<ContactProtoModel, GetContactByFilterResponse>()
           .ForMember(dest => dest.DateOfBirth, opt =>
               opt.MapFrom(src => src.DateOfBirth.ToDateTime()));

        CreateMap<ContactProtoModel, GetContactByIdResponse>()
            .ForMember(dest => dest.DateOfBirth, opt =>
                opt.MapFrom(src => src.DateOfBirth.ToDateTime()));

        CreateMap<ContactProtoModel, CreateContactResponse>()
            .ForMember(dest => dest.DateOfBirth, opt =>
                opt.MapFrom(src => src.DateOfBirth.ToDateTime()));

        CreateMap<ContactProtoModel, CreateContactRequest>()
            .ForMember(dest => dest.DateOfBirth, opt =>
                opt.MapFrom(src => src.DateOfBirth.ToDateTime()));
        

               CreateMap<GetContactByFilterQuery, GetContactByFilterProtoRequest>()
            .ForMember(dest => dest.DateOfBirth, opt =>
                opt.MapFrom(src => Timestamp.FromDateTime(src.DateOfBirth.ToUniversalTime()))).ReverseMap();

        CreateMap<CreateContactResponse, ContactProtoModel>()
            .ForMember(dest => dest.DateOfBirth, opt =>
                opt.MapFrom(src => Timestamp.FromDateTime(src.DateOfBirth)));

        CreateMap<UpdateContactProtoResponse, UpdateContactResponse>();
        CreateMap<DeleteContactProtoResponse, DeleteContactResponse>();

        CreateMap<CreateContactRequest, CreateContactProtoRequest>()
            .ForMember(dest => dest.DateOfBirth, opt =>
                opt.MapFrom(src => Timestamp.FromDateTime(src.DateOfBirth.ToUniversalTime()))).ReverseMap();

        CreateMap<UpdateContactRequest, UpdateContactProtoRequest>()
            .ForMember(dest => dest.DateOfBirth, opt =>
                opt.MapFrom(src => Timestamp.FromDateTime(src.DateOfBirth.ToUniversalTime())))
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.ContactId.ToString())).ReverseMap();

        CreateMap<DeleteContactRequest, DeleteContactProtoRequest>();
    }

}
