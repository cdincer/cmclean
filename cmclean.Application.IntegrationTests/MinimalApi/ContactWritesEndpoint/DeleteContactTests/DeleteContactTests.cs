using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Commands.UpdateContact;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;
using FluentAssertions;
using System.Net.Http.Json;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactWritesEndpoint.DeleteContactTests
{
    [Collection("ContactsEndpoint Collection")]
    public class DeleteContactTests : IClassFixture<DeleteContactTestsDatabaseFixture>
    {

        DeleteContactTestsDatabaseFixture fixture;
        public DeleteContactTests(DeleteContactTestsDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }


        [Fact]
        public async Task TestDeleteContact_DeleteExistingContact_SuccesfulResponse()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8001/");
            var contact = fixture._contacts[0];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

            HttpResponseMessage response = await client.PostAsJsonAsync("api/contacts/", createContactRequest);
            CreateContactResponse createContact = await response.Content.ReadFromJsonAsync<CreateContactResponse>();
            createContact.Should().BeAssignableTo<CreateContactResponse>();
            createContact.FirstName.Should().NotBeNull();
            createContact.LastName.Should().NotBeNull();
            Guid ToDelete = createContact.Id;
            var deleteResponse = await client.DeleteAsync("api/contacts/"+ToDelete.ToString());
            deleteResponse.IsSuccessStatusCode.Should().BeTrue();
        }


        [Fact]
        public async Task TestDeleteContact_DeleteExistingContact_FailedResponse()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8001/");
            var contact = fixture._contacts[0];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

            HttpResponseMessage response = await client.PostAsJsonAsync("api/contacts/", createContactRequest);
            CreateContactResponse createContact = await response.Content.ReadFromJsonAsync<CreateContactResponse>();
            createContact.Should().BeAssignableTo<CreateContactResponse>();
            createContact.FirstName.Should().NotBeNull();
            createContact.LastName.Should().NotBeNull();
            Guid NonExistingGuid = Guid.NewGuid();
            var deleteResponse = await client.DeleteAsync("api/contacts/" + NonExistingGuid.ToString());
            deleteResponse.IsSuccessStatusCode.Should().BeFalse();
        }
    }
}
