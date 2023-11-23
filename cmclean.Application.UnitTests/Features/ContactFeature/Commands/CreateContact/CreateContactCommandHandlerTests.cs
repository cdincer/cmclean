using AutoMapper;
using cmclean.Application.Common.Behaviours;
using cmclean.Application.Common.Exceptions;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Domain.Model;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace cmclean.Application.UnitTests.Features.ContactFeature.Commands.CreateContact
{
    public class CreateContactCommandHandlerTests
    {
        private CreateContactCommandHandler? _createContactCommandHandler;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<CreateContactCommand>> _logger;
        private readonly Mock<IContactWriteRepository> _contactWriteRepository;
        private readonly IServiceCollection _services;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<Contact> _contacts;
        private readonly CreateContactValidator _createContactValidator;



        public CreateContactCommandHandlerTests()
        {
            _contactWriteRepository = new Mock<IContactWriteRepository>();
            _services = new ServiceCollection();
            _services.AddApplicationServices();
            _services.AddLogging();

            _serviceProvider = _services.BuildServiceProvider();
            _mapper = _serviceProvider.GetService<IMapper>()!;

            _createContactValidator = new CreateContactValidator();

            _logger = new Mock<ILogger<CreateContactCommand>>();

            _contacts = new List<Contact>
           {
            new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "Tester1First", LastName = "Tester1Last", 
                DisplayName="Tester1Dis",BirthDate = new DateTime(1990,9,1),Email = "testerfirst@email.com",Phonenumber ="22222"
            },
                new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "Tester2First", LastName = "Tester2Last",
                DisplayName="",BirthDate = new DateTime(1989,9,1),Email = "testerfirst@email.com",Phonenumber ="33333"
            },              
                new()
            {
                Id  = Guid.NewGuid() ,Salutation ="", FirstName = "Tester3First", LastName = "Tester3Last",
                DisplayName="Tester3Dis",BirthDate = new DateTime(1990,9,1),Email = "testerfirst@email.com",Phonenumber ="33333"
            }
           };
        }


        [Fact]
        public async Task TestCreateContact_CreateContactWithValidCommandShouldReturn_NoValidationException()
        {
            var contact = _contacts[0];
            var createContactRequest = new CreateContactRequest(contact.Salutation,contact.FirstName,contact.LastName,
                                                                contact.DisplayName,contact.BirthDate,contact.Email,contact.Phonenumber);
            var command = _mapper.Map<CreateContactCommand>(createContactRequest);
            var requestLogger = new LoggingBehaviour<CreateContactCommand>(_logger.Object);
            await requestLogger.Process(command, new CancellationToken());
            _createContactCommandHandler = new CreateContactCommandHandler(_contactWriteRepository.Object, _mapper);
            var validationBehaviour = new ValidationBehaviour<CreateContactCommand, CreateContactResponse>(new[] { _createContactValidator });
            var requestHandlerDelegate = new RequestHandlerDelegate<CreateContactResponse>(() => _createContactCommandHandler.Handle(command, CancellationToken.None));
            var result = await validationBehaviour.Handle(command,requestHandlerDelegate, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.ToString().Should().NotBeEmpty();
            result.Salutation.Should().NotBeEmpty();
            result.FirstName.Should().NotBeEmpty();
            result.LastName.Should().NotBeEmpty();
            result.Email.Should().NotBeEmpty();
            result.BirthDate.Should().Be(contact.BirthDate);
            result.Should().BeAssignableTo<CreateContactResponse>();
        }

        [Fact]
        public async Task TestCreateContact_CreateContactDisplayNameReplacement_NoValidationException()
        {
            var contact = _contacts[1];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);
            var command = _mapper.Map<CreateContactCommand>(createContactRequest);
            var requestLogger = new LoggingBehaviour<CreateContactCommand>(_logger.Object);
            await requestLogger.Process(command, new CancellationToken());
            _createContactCommandHandler = new CreateContactCommandHandler(_contactWriteRepository.Object, _mapper);
            var validationBehaviour = new ValidationBehaviour<CreateContactCommand, CreateContactResponse>(new[] { _createContactValidator });
            var requestHandlerDelegate = new RequestHandlerDelegate<CreateContactResponse>(() => _createContactCommandHandler.Handle(command, CancellationToken.None));
            var result = await validationBehaviour.Handle(command, requestHandlerDelegate, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.ToString().Should().NotBeEmpty();
            result.Salutation.Should().NotBeEmpty();
            result.FirstName.Should().NotBeEmpty();
            result.LastName.Should().NotBeEmpty();
            result.DisplayName.Should().Be(result.Salutation + result.FirstName + result.LastName);
            result.Email.Should().NotBeEmpty();
            result.BirthDate.Should().Be(contact.BirthDate);
            result.Should().BeAssignableTo<CreateContactResponse>();
        }

        /// <summary>
        /// Testing validation behaviour for Salutation, it shouldn't be shorter than 2 characters.
        /// </summary>
        /// <returns>ValidationException</returns>
        [Fact]
        public async Task TestCreateContact_CreateContactWithInvalidCommandShouldReturn_ValidationException()
        {
            var contact = _contacts[2];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);
            var command = _mapper.Map<CreateContactCommand>(createContactRequest);
            _createContactCommandHandler = new CreateContactCommandHandler(_contactWriteRepository.Object, _mapper);
            var validationBehaviour = new ValidationBehaviour<CreateContactCommand, CreateContactResponse>(new[] { _createContactValidator });
            
            Task<CreateContactResponse> requestHandlerDelegate() => _createContactCommandHandler.Handle(command, CancellationToken.None);
            requestHandlerDelegate().Should().NotBeNull();

            var requestHandlerDelegateResult = new RequestHandlerDelegate<CreateContactResponse>(requestHandlerDelegate);
            requestHandlerDelegateResult.Should().NotBeNull();

            Func<Task> act = async () => await validationBehaviour.Handle(command, requestHandlerDelegateResult, CancellationToken.None);
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
