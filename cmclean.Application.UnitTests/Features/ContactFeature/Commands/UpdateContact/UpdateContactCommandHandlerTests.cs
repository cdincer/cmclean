using AutoMapper;
using cmclean.Application.Common.Behaviours;
using cmclean.Application.Common.Exceptions;
using cmclean.Application.Features.ContactFeature.Commands.UpdateContact;
using cmclean.Application.Features.ContactFeature.Commands.UpdateContact;
using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Domain.Model;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.Application.UnitTests.Features.ContactFeature.Commands.UpdateContact
{
    public class UpdateContactCommandHandlerTests
    {
        private UpdateContactCommandHandler? _updateContactCommandHandler;
        private readonly IMapper _mapper;
        private readonly Mock<IContactWriteRepository> _contactWriteRepository;
        private readonly Mock<IContactReadRepository> _contactReadRepository;
        private readonly IServiceCollection _services;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<Contact> _contacts;
        private readonly UpdateContactValidator _UpdateContactValidator;


        public UpdateContactCommandHandlerTests()
        {
            _contactWriteRepository = new Mock<IContactWriteRepository>();
            _contactReadRepository = new Mock<IContactReadRepository>();

            _services = new ServiceCollection();
            _services.AddApplicationServices();

            _serviceProvider = _services.BuildServiceProvider();
            _mapper = _serviceProvider.GetService<IMapper>()!;

            _UpdateContactValidator = new UpdateContactValidator();


            _contacts = new List<Contact>
           {
            new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "Tester1First", LastName = "Tester1Last",
                DisplayName="Tester1Dis",BirthDate = new DateTime(1990,9,1),Email = "testerfirst@email.com",Phonenumber ="22222"
            },
                new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "__", LastName = "Tester3Last",
                DisplayName="Tester3Dis",BirthDate = new DateTime(1990,9,1),Email = "testerfirst@email.com",Phonenumber ="33333"
            }
           };
        }


        [Fact]
        public async Task TestUpdateContact_UpdateContactWithValidCommandShouldReturn_NoValidationException()
        {
            var contact = _contacts[0];
            _contactReadRepository.Setup(_ => _.GetByIdAsync(contact.Id)).ReturnsAsync(contact);
            _contactWriteRepository.Setup(_ => _.Update(contact)).ReturnsAsync(true);

            var UpdateContactRequest = new UpdateContactRequest(contact.Id, contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

            var command = _mapper.Map<UpdateContactCommand>(UpdateContactRequest);

            _updateContactCommandHandler = new UpdateContactCommandHandler(_contactWriteRepository.Object, _contactReadRepository.Object);
            
            var validationBehaviour = new ValidationBehaviour<UpdateContactCommand, UpdateContactResponse>(new[] { _UpdateContactValidator });
            var requestHandlerDelegate = new RequestHandlerDelegate<UpdateContactResponse>(() => _updateContactCommandHandler.Handle(command, CancellationToken.None));
            
            var result = await validationBehaviour.Handle(command, requestHandlerDelegate, CancellationToken.None);

            result.Should().NotBeNull();
            result.Status.Should().BeTrue();
            result.Should().BeAssignableTo<UpdateContactResponse>();
        }


        /// <summary>
        /// Testing validation behaviour for FirstName, it shouldn't be shorter than 2 characters.
        /// </summary>
        /// <returns>ValidationException</returns>
        [Fact]
        public void  TestUpdateContact_UpdateContactWithInvalidCommandShouldReturn_ValidationException()
        {
            var contact = _contacts[1];
            _contactReadRepository.Setup(_ => _.GetByIdAsync(contact.Id)).ReturnsAsync(contact);
            _contactWriteRepository.Setup(_ => _.Update(contact)).ReturnsAsync(true);

            var UpdateContactRequest = new UpdateContactRequest(contact.Id, contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);
            var command = _mapper.Map<UpdateContactCommand>(UpdateContactRequest);

            _updateContactCommandHandler = new UpdateContactCommandHandler(_contactWriteRepository.Object, _contactReadRepository.Object);

            var validationBehaviour = new ValidationBehaviour<UpdateContactCommand, UpdateContactResponse>(new[] { _UpdateContactValidator });
            Task<UpdateContactResponse> requestHandlerDelegate() => _updateContactCommandHandler.Handle(command, CancellationToken.None);

            requestHandlerDelegate().Should().NotBeNull();
            var requestHandlerDelegateResult = new RequestHandlerDelegate<UpdateContactResponse>(requestHandlerDelegate);

            Assert.ThrowsAnyAsync<ValidationException>(() => validationBehaviour.Handle(command, requestHandlerDelegateResult, CancellationToken.None));
        }

    }
}
