﻿using AutoMapper;
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
        CreateMap<Contact, CreateContactResponse>().ReverseMap();
        CreateMap<Contact, GetAllContactsResponse>().ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate)).ReverseMap();
        CreateMap<Contact, GetContactByIdResponse>().ReverseMap();
        CreateMap<Contact, GetContactByFilterResponse>().ReverseMap();
    }
}