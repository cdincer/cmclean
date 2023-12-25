using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using FluentAssertions;
using System.Net.Http.Json;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactWritesEndpoint.CreateContactTests
{
    [Collection("ContactsEndpoint Collection")]

    public class CreateContactTests : IClassFixture<CreateContactTestsDatabaseFixture>
    {
        CreateContactTestsDatabaseFixture fixture;

        public CreateContactTests(CreateContactTestsDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task TestCreateContact_ValidContactWithNoMissingFields_NoException()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8001/");
            var contact = fixture._contacts[0];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

            HttpResponseMessage response = await client.PostAsJsonAsync("api/contacts/", createContactRequest);
            CreateContactResponse createContact = await response.Content.ReadFromJsonAsync<CreateContactResponse>();

            createContact.Should().BeAssignableTo<CreateContactResponse>();
            createContact.FirstName.Should().BeEquivalentTo("Danny");
            createContact.LastName.Should().BeEquivalentTo("Boyle");
        }
        [Fact]
        public async Task TestCreateContact_ValidContactWithoutDisplayName_ReturnMergedDisplayName()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8001/");
            var contact = fixture._contacts[1];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

            HttpResponseMessage response = await client.PostAsJsonAsync("api/contacts/", createContactRequest);
            CreateContactResponse createContact = await response.Content.ReadFromJsonAsync<CreateContactResponse>();

            createContact.Should().BeAssignableTo<CreateContactResponse>();
            createContact.FirstName.Should().BeEquivalentTo("Alex");
            createContact.LastName.Should().BeEquivalentTo("Garland");
            createContact.DisplayName.Should().Be(contact.Salutation + contact.FirstName + contact.LastName);
        }

        public static TheoryData<string, string, string, string, DateTime, string, string> MissingFieldCases =
        new()
        {
            { "", "Test","User1", "NoSalutation", new DateTime(2018, 12, 31), "nosalutation@email.com", "12341234" },
            { "Mr", "","User2", "NoFirstName", new DateTime(2018, 12, 31), "nofirstname@email.com", "12341234" },
            { "Mr", "Test","", "NoLastName", new DateTime(2018, 12, 31), "nolastname@email.com", "12341234" },
            { "Mr", "Test","User4", "NoEmail", new DateTime(2018, 12, 31), "", "12341234" }
        };
        [Theory, MemberData(nameof(MissingFieldCases))]
        public async Task TestCreateContact_MissingFields_NoExceptionGracefulFailure(string Salutation, string FirstName, string LastName, string DisplayName,
                                                                DateTime BirthDate, string Email, string Phonenumber)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8001/");
            var createContactRequest = new CreateContactRequest(Salutation, FirstName, LastName,
                                                                DisplayName, BirthDate, Email, Phonenumber);

            HttpResponseMessage response = await client.PostAsJsonAsync("api/contacts/", createContactRequest);
            CreateContactResponse createContact = await response.Content.ReadFromJsonAsync<CreateContactResponse>();
            createContact.Should().BeAssignableTo<CreateContactResponse>();
            createContact.FirstName.Should().BeEquivalentTo(null);
            createContact.LastName.Should().BeEquivalentTo(null);
        }
    }
}
