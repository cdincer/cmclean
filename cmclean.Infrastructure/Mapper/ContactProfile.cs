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
            .ForMember(dest => dest.BirthDate, opt =>
            opt.MapFrom(src => src.BirthDate.ToDateTime()))
            .ForMember(dest => dest.CreationTimestamp, opt =>
            opt.MapFrom(src => src.CreationTimestamp.ToDateTime()))
            .ForMember(dest => dest.LastChangeTimeStamp, opt =>
            opt.MapFrom(src => src.LastChangeTimeStamp.ToDateTime()));

        CreateMap<ContactProtoModel, GetContactByFilterResponse>()
           .ForMember(dest => dest.BirthDate, opt =>
               opt.MapFrom(src => src.BirthDate.ToDateTime()));

        CreateMap<ContactProtoModel, GetContactByIdResponse>()
            .ForMember(dest => dest.BirthDate, opt =>
            opt.MapFrom(src => src.BirthDate.ToDateTime()))
            .ForMember(dest => dest.CreationTimestamp, opt =>
            opt.MapFrom(src => src.CreationTimestamp.ToDateTime()))
            .ForMember(dest => dest.LastChangeTimeStamp, opt =>
            opt.MapFrom(src => src.LastChangeTimeStamp.ToDateTime()));

        CreateMap<ContactProtoModel, CreateContactResponse>()
            .ForMember(dest => dest.BirthDate, opt =>
                opt.MapFrom(src => src.BirthDate.ToDateTime()));

        CreateMap<ContactProtoModel, CreateContactRequest>()
            .ForMember(dest => dest.BirthDate, opt =>
                opt.MapFrom(src => src.BirthDate.ToDateTime()));

        CreateMap<GetContactByFilterQuery, GetContactByFilterProtoRequest>()
        .ForMember(dest => dest.BirthDate, opt =>
        opt.MapFrom(src => Timestamp.FromDateTime(src.BirthDate.ToUniversalTime()))).ReverseMap();

        CreateMap<CreateContactResponse, ContactProtoModel>()
            .ForMember(dest => dest.BirthDate, opt =>
                opt.MapFrom(src => Timestamp.FromDateTime(src.BirthDate)));

        CreateMap<UpdateContactProtoResponse, UpdateContactResponse>();
        CreateMap<DeleteContactProtoResponse, DeleteContactResponse>();

        CreateMap<CreateContactRequest, CreateContactProtoRequest>()
            .ForMember(dest => dest.BirthDate, opt =>
                opt.MapFrom(src => Timestamp.FromDateTime(src.BirthDate.ToUniversalTime()))).ReverseMap();

        CreateMap<UpdateContactRequest, UpdateContactProtoRequest>()
            .ForMember(dest => dest.BirthDate, opt =>
                opt.MapFrom(src => Timestamp.FromDateTime(src.BirthDate.ToUniversalTime())))
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.ContactId.ToString())).ReverseMap();

        CreateMap<DeleteContactRequest, DeleteContactProtoRequest>();
    }

}
