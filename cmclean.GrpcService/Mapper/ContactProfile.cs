using AutoMapper;
using cmclean.Application.Common.Results;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Queries.GetAllContacts;
using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;
using cmclean.GrpcService.Protos;
using Google.Protobuf.WellKnownTypes;

namespace cmclean.GrpcService.Mapper
{
    public class ContactProfile : Profile
    {

        public ContactProfile()
        {
            CreateMap<GetAllContactsResponse, ContactProtoModel>()
            .ForMember(dest => dest.BirthDate, opt =>
            opt.MapFrom(src => Timestamp.FromDateTime(src.BirthDate.ToUniversalTime())))
            .ForMember(dest => dest.CreationTimestamp, opt =>
            opt.MapFrom(src => Timestamp.FromDateTime(src.CreationTimestamp.ToUniversalTime())))
            .ForMember(dest => dest.LastChangeTimeStamp, opt =>
            opt.MapFrom(src => Timestamp.FromDateTime(src.LastChangeTimeStamp.ToUniversalTime())));

            CreateMap<GetContactByIdResponse, ContactProtoModel>()
            .ForMember(dest => dest.BirthDate, opt =>
            opt.MapFrom(src => Timestamp.FromDateTime(src.BirthDate.ToUniversalTime())))
            .ForMember(dest => dest.CreationTimestamp, opt =>
            opt.MapFrom(src => Timestamp.FromDateTime(src.CreationTimestamp.ToUniversalTime())))
            .ForMember(dest => dest.LastChangeTimeStamp, opt =>
            opt.MapFrom(src => Timestamp.FromDateTime(src.LastChangeTimeStamp.ToUniversalTime())));


            CreateMap<IDataResult<CreateContactResponse>, CreateContactProtoResponse>().
            ForPath(dest => dest.Data.Id, opt => opt.MapFrom(src => src.Data.Id)).
            ForPath(dest => dest.Data.Salutation, opt => opt.MapFrom(src => src.Data.Salutation)).
            ForPath(dest => dest.Data.FirstName, opt => opt.MapFrom(src => src.Data.FirstName)).
            ForPath(dest => dest.Data.LastName, opt => opt.MapFrom(src => src.Data.LastName)).
            ForPath(dest => dest.Data.DisplayName, opt => opt.MapFrom(src => src.Data.DisplayName)).
            ForPath(dest => dest.Data.BirthDate, opt => opt.MapFrom(src => Timestamp.FromDateTime(src.Data.BirthDate.ToUniversalTime()))).
            ForPath(dest => dest.Data.CreationTimestamp, opt => opt.MapFrom(src => Timestamp.FromDateTime(src.Data.CreationTimestamp.ToUniversalTime()))).
            ForPath(dest => dest.Data.LastChangeTimeStamp, opt => opt.MapFrom(src => Timestamp.FromDateTime(src.Data.LastChangeTimeStamp.ToUniversalTime()))).
            ForPath(dest => dest.Data.NotifyHasBirthdaySoon, opt => opt.MapFrom(src => src.Data.NotifyHasBirthdaySoon)).
            ForPath(dest => dest.Data.Email, opt => opt.MapFrom(src => src.Data.Email)).
            ForPath(dest => dest.Data.Phonenumber, opt => opt.MapFrom(src => src.Data.Phonenumber)).
            ForMember(dest => dest.Message, opt =>opt.MapFrom(src => src.Message)).
            ForMember(dest => dest.Success, opt =>opt.MapFrom(src => src.Success));

            CreateMap<GetContactByFilterProtoRequest, GetContactByFilterQuery>().
            ForMember(dest => dest.BirthDate, opt =>
            opt.MapFrom(src => src.BirthDate.ToDateTime()));

            CreateMap<GetContactByFilterResponse, ContactProtoModel>()
            .ForMember(dest => dest.BirthDate, opt =>
            opt.MapFrom(src => Timestamp.FromDateTime(src.BirthDate.ToUniversalTime())))
            .ForMember(dest => dest.CreationTimestamp, opt =>
            opt.MapFrom(src => Timestamp.FromDateTime(src.CreationTimestamp.ToUniversalTime())))
            .ForMember(dest => dest.LastChangeTimeStamp, opt =>
            opt.MapFrom(src => Timestamp.FromDateTime(src.LastChangeTimeStamp.ToUniversalTime())));
        }
    }
}
