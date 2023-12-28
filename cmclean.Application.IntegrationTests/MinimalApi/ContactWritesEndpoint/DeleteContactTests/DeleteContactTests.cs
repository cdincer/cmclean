using cmclean.Application.Common.Results;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Commands.DeleteContact;
using cmclean.Application.Features.ContactFeature.Commands.UpdateContact;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;
using FluentAssertions;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

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
            var createContact = await response.Content.ReadAsStringAsync();
            JsonNode MainBodyDeserialized = JsonNode.Parse(createContact)!;
            JsonNode CreateContactResponseJSONFormat = MainBodyDeserialized!["data"]!;
            CreateContactResponseJSONFormat["firstName"].ToString().Should().NotBeNullOrEmpty();
            CreateContactResponseJSONFormat["lastName"].ToString().Should().NotBeNullOrEmpty();

            Guid ToDelete = Guid.Parse(CreateContactResponseJSONFormat["id"].ToString());
            HttpResponseMessage deletedResponse = await client.DeleteAsync("api/contacts/"+ToDelete.ToString());
            var deleteContact = await deletedResponse.Content.ReadAsStringAsync();
            JsonNode deleteMainBodyDeserialized = JsonNode.Parse(deleteContact)!;
            ((bool)deleteMainBodyDeserialized["success"]).Should().BeTrue();
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
            var createContact = await response.Content.ReadAsStringAsync();
            JsonNode MainBodyDeserialized = JsonNode.Parse(createContact)!;
            JsonNode CreateContactResponseJSONFormat = MainBodyDeserialized!["data"]!;
            CreateContactResponseJSONFormat["firstName"].ToString().Should().NotBeNullOrEmpty();
            CreateContactResponseJSONFormat["lastName"].ToString().Should().NotBeNullOrEmpty();
            Guid ToDelete = Guid.Parse(CreateContactResponseJSONFormat["id"].ToString());
            HttpResponseMessage deletedResponse = await client.DeleteAsync("api/contacts/" + ToDelete);

            var deleteContact = await deletedResponse.Content.ReadAsStringAsync();
            JsonNode deleteMainBodyDeserialized = JsonNode.Parse(deleteContact)!;
            ((bool)deleteMainBodyDeserialized["success"]).Should().BeTrue();

        }
    }
}
