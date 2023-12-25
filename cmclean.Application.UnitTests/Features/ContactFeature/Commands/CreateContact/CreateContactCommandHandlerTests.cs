using AutoMapper;
using cmclean.Application.Common.Behaviours;
using cmclean.Application.Common.Exceptions;
using cmclean.Application.Common.Results;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Domain.Model;
using FluentAssertions;
using FluentValidation;
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
        private readonly Mock<IContactReadRepository> _contactReadRepository;

        private readonly IServiceCollection _services;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<Contact> _contacts;
        CreateContactValidator _createContactValidator;



        public CreateContactCommandHandlerTests()
        {
            _contactWriteRepository = new Mock<IContactWriteRepository>();
            _contactReadRepository = new Mock<IContactReadRepository>();
            _services = new ServiceCollection();
            _services.AddApplicationServices();
            _services.AddLogging();
            _serviceProvider = _services.BuildServiceProvider();
            _mapper = _serviceProvider.GetService<IMapper>()!;
            _logger = new Mock<ILogger<CreateContactCommand>>();

            _contacts = new List<Contact>
           {
            new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "Tester1First", LastName = "Tester1Last",
                DisplayName="Tester1Dis",BirthDate = new DateTime(1990,9,1),Email = "testerfirst1@email.com",Phonenumber ="22222"
            },
                new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "Tester2First", LastName = "Tester2Last",
                DisplayName="",BirthDate = new DateTime(1989,9,1),Email = "testerfirs2t@email.com",Phonenumber ="33333"
            },
                new()
            {
                Id  = Guid.NewGuid() ,Salutation ="", FirstName = "Tester3First", LastName = "Tester3Last",
                DisplayName="Tester3Dis",BirthDate = new DateTime(1990,9,1),Email = "testerfirs3t@email.com",Phonenumber ="33333"
            }
           };
        }


        [Fact]
        public async Task TestCreateContact_CreateContactWithValidCommandShouldReturn_NoValidationException()
        {
            var contact = _contacts[0];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);
            var query = new GetContactByFilterQuery()
            {
                FirstName = "",
                LastName = "",
                DisplayName = "",
                BirthDate = DateTime.MinValue,
                Email = contact.Email,
                Phonenumber = ""
            };

            var command = _mapper.Map<CreateContactCommand>(createContactRequest);
            _contactReadRepository.Setup(x => x.GetAsync(query)).Returns((Task<List<Contact?>>)null);
            _createContactValidator = new CreateContactValidator(_contactReadRepository.Object);
            var validationResult = await _createContactValidator.ValidateAsync(command);
            validationResult.Errors.Should().HaveCount(0);
            _createContactCommandHandler = new CreateContactCommandHandler(_contactWriteRepository.Object, _mapper);

            var result = await _createContactCommandHandler.Handle(command, CancellationToken.None);
            result.Id.Should().NotBeEmpty();
            result.Should().BeAssignableTo<CreateContactResponse>();
        }

        [Fact]
        public async Task TestCreateContact_CreateContactDisplayNameReplacement_NoValidationException()
        {
            var contact = _contacts[1];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                    contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);
            var query = new GetContactByFilterQuery()
            {
                FirstName = "",
                LastName = "",
                DisplayName = "",
                BirthDate = DateTime.MinValue,
                Email = contact.Email,
                Phonenumber = ""
            };

            var command = _mapper.Map<CreateContactCommand>(createContactRequest);
            _contactReadRepository.Setup(x => x.GetAsync(query)).Returns((Task<List<Contact?>>)null);

            _createContactValidator = new CreateContactValidator(_contactReadRepository.Object);
            var validationResult = await _createContactValidator.ValidateAsync(command);
            validationResult.Errors.Should().HaveCount(0);
            _createContactCommandHandler = new CreateContactCommandHandler(_contactWriteRepository.Object, _mapper);

            var result = await _createContactCommandHandler.Handle(command, CancellationToken.None);
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
        /// Testing validation behaviour for Salutation, it shouldn't be shorter than 2 characters and not empty.
        /// </summary>
        /// <returns>ValidationException</returns>
        [Fact]
        public async Task TestCreateContact_CreateContactWithInvalidCommandShouldReturn_ValidationErrors()
        {
            var contact = _contacts[2];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);
            var command = _mapper.Map<CreateContactCommand>(createContactRequest);
            _createContactCommandHandler = new CreateContactCommandHandler(_contactWriteRepository.Object, _mapper);

            var query = new GetContactByFilterQuery()
            {
                FirstName = "",
                LastName = "",
                DisplayName = "",
                BirthDate = DateTime.MinValue,
                Email = contact.Email,
                Phonenumber = ""
            };
            var emptyList = new List<Contact?>();
            _contactReadRepository.Setup(x => x.GetAsync(query)).ReturnsAsync(emptyList);

            _createContactValidator = new CreateContactValidator(_contactReadRepository.Object);
            var validationResult = await _createContactValidator.ValidateAsync(command);
            validationResult.Errors.Should().HaveCount(2);
        }

        [Fact]
        public async Task TestCreateContact_CreateContactWithDuplicateEmailShouldReturn_ValidationErrors()
        {
            var contact = _contacts[0];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);
            List<Contact?> nonUniqList = new List<Contact?>
            {
                contact
            };
            var query = new GetContactByFilterQuery()
            {
                FirstName = "",
                LastName = "",
                DisplayName = "",
                BirthDate = DateTime.MinValue,
                Email = contact.Email,
                Phonenumber = ""
            };
            //Because of extension method in validator, this mocking can be only done with IsAny
            //More Info:https://stackoverflow.com/questions/60712223/how-do-i-mock-an-abstractvalidator-that-is-using-rule-sets
            _contactReadRepository.Setup(x => x.GetAsync(It.IsAny<GetContactByFilterQuery>())).ReturnsAsync(nonUniqList);
            _createContactCommandHandler = new CreateContactCommandHandler(_contactWriteRepository.Object, _mapper);
            _createContactValidator = new CreateContactValidator(_contactReadRepository.Object);

            var command = _mapper.Map<CreateContactCommand>(createContactRequest);
            var validationResult = await _createContactValidator.ValidateAsync(command);
            var result = await _createContactCommandHandler.Handle(command, CancellationToken.None);

            validationResult.Errors.Count.Should().Be(1);
        }
    }
}
