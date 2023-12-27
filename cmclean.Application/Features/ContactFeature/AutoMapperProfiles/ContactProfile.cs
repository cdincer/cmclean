using AutoMapper;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Commands.DeleteContact;
using cmclean.Application.Features.ContactFeature.Commands.UpdateContact;
using cmclean.Application.Features.ContactFeature.Queries.GetAllContacts;
using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;
using cmclean.Domain.Model;

namespace cmclean.Application.Features.ContactFeature.AutoMapperProfiles;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<CreateContactCommand, Contact>();
        CreateMap<CreateContactRequest, CreateContactCommand>().ReverseMap();
        CreateMap<UpdateContactRequest, UpdateContactCommand>().ReverseMap();
        CreateMap<DeleteContactRequest, DeleteContactCommand>().ReverseMap();
        CreateMap<Contact, CreateContactResponse>()
            .ForMember(dest => dest.Data, opt =>opt.MapFrom(src => src));
        CreateMap<CreateContactResponse,Contact >()
           .ForMember(dest => dest, opt => opt.MapFrom(src => src.Data));
        CreateMap<Contact, GetAllContactsResponse>().ReverseMap();
        CreateMap<Contact, GetContactByIdResponse>().ReverseMap();
        CreateMap<Contact, GetContactByFilterResponse>().ReverseMap();
    }
}