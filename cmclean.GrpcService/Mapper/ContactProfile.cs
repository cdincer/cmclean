using AutoMapper;
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
             .ForMember(dest => dest.DateOfBirth, opt =>
                 opt.MapFrom(src => Timestamp.FromDateTime(src.DateOfBirth.ToUniversalTime())));

            CreateMap<GetContactByIdResponse, ContactProtoModel>()
                .ForMember(dest => dest.DateOfBirth, opt =>
                    opt.MapFrom(src => Timestamp.FromDateTime(src.DateOfBirth.ToUniversalTime())));

            CreateMap<CreateContactResponse, ContactProtoModel>()
                .ForMember(dest => dest.DateOfBirth, opt =>
                    opt.MapFrom(src => Timestamp.FromDateTime(src.DateOfBirth.ToUniversalTime())));


            CreateMap<GetContactByFilterQuery, GetContactByFilterProtoRequest>()
         .ForMember(dest => dest.DateOfBirth, opt =>
              opt.MapFrom(src => Timestamp.FromDateTime(src.DateOfBirth.Value.ToUniversalTime()))).ReverseMap();

            CreateMap<GetContactByFilterResponse, ContactProtoModel>()
               .ForMember(dest => dest.DateOfBirth, opt =>
              opt.MapFrom(src => Timestamp.FromDateTime(src.DateOfBirth.ToUniversalTime())));


            // CreateMap<GetContactByFilterProtoRequest, GetContactByFilterQuery>()
            //.ForMember(dest => dest.DateOfBirth, opt =>
            //    opt.MapFrom(src => src.DateOfBirth.ToDateTime()));



        }
    }
}
