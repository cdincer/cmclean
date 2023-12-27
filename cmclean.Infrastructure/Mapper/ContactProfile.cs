using AutoMapper;
using cmclean.Application.Common.Results;
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
            opt.MapFrom(src => src.BirthDate.ToDateTime()))
            .ForMember(dest => dest.CreationTimestamp, opt =>
            opt.MapFrom(src => src.CreationTimestamp.ToDateTime()))
            .ForMember(dest => dest.LastChangeTimeStamp, opt =>
            opt.MapFrom(src => src.LastChangeTimeStamp.ToDateTime()));

        CreateMap<ContactProtoModel, GetContactByIdResponse>()
            .ForMember(dest => dest.BirthDate, opt =>
            opt.MapFrom(src => src.BirthDate.ToDateTime()))
            .ForMember(dest => dest.CreationTimestamp, opt =>
            opt.MapFrom(src => src.CreationTimestamp.ToDateTime()))
            .ForMember(dest => dest.LastChangeTimeStamp, opt =>
            opt.MapFrom(src => src.LastChangeTimeStamp.ToDateTime()));

        CreateMap<ContactProtoModel, CreateContactRequest>()
            .ForMember(dest => dest.BirthDate, opt =>
                opt.MapFrom(src => src.BirthDate.ToDateTime()));

        CreateMap<GetContactByFilterQuery, GetContactByFilterProtoRequest>()
        .ForMember(dest => dest.BirthDate, opt =>
        opt.MapFrom(src => Timestamp.FromDateTime(src.BirthDate.ToUniversalTime()))).ReverseMap();

        CreateMap<CreateContactProtoResponse, CreateContactResponse > ().
                        ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.Data.Id)).
                        ForPath(dest => dest.Salutation, opt => opt.MapFrom(src => src.Data.Salutation)).
                        ForPath(dest => dest.FirstName, opt => opt.MapFrom(src => src.Data.FirstName)).
                        ForPath(dest => dest.LastName, opt => opt.MapFrom(src => src.Data.LastName)).
                        ForPath(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Data.DisplayName)).
                        ForPath(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Data.BirthDate.ToDateTime())).
                        ForPath(dest => dest.CreationTimestamp, opt => opt.MapFrom(src => src.Data.CreationTimestamp.ToDateTime())).
                        ForPath(dest => dest.LastChangeTimeStamp, opt => opt.MapFrom(src => src.Data.LastChangeTimeStamp.ToDateTime())).
                        ForPath(dest => dest.NotifyHasBirthdaySoon, opt => opt.MapFrom(src => src.Data.NotifyHasBirthdaySoon)).
                        ForPath(dest => dest.Email, opt => opt.MapFrom(src => src.Data.Email)).
                        ForPath(dest => dest.Phonenumber, opt => opt.MapFrom(src => src.Data.Phonenumber));

        CreateMap<UpdateContactProtoResponse, UpdateContactResponse>();
        CreateMap<DeleteContactProtoResponse, DeleteContactResponse>();

        CreateMap<CreateContactRequest, CreateContactProtoRequest>()
            .ForMember(dest => dest.BirthDate, opt =>
                opt.MapFrom(src => Timestamp.FromDateTime(src.BirthDate.ToUniversalTime()))).ReverseMap();

        CreateMap<UpdateContactRequest, UpdateContactProtoRequest>()
            .ForMember(dest => dest.BirthDate, opt =>
                opt.MapFrom(src => Timestamp.FromDateTime(src.BirthDate.ToUniversalTime())));

        CreateMap<DeleteContactRequest, DeleteContactProtoRequest>();
    }

}
