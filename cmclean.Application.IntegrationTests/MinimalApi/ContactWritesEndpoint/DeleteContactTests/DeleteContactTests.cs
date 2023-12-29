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
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            Guid ToDelete = await TestDeleteContact_SampleRecordInsert_Utility(0);

            HttpResponseMessage deletedResponse = await client.DeleteAsync(ContactEndpointConstants.ContactEndpoint+ToDelete.ToString());
            var deleteContact = await deletedResponse.Content.ReadAsStringAsync();
            JsonNode deleteMainBodyDeserialized = JsonNode.Parse(deleteContact)!;
            ((bool)deleteMainBodyDeserialized[ContactEndpointConstants.SuccessNode]).Should().BeTrue();
        }


        [Fact]
        public async Task TestDeleteContact_DeleteExistingContact_FailedResponse()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            Guid ToDelete = await TestDeleteContact_SampleRecordInsert_Utility(1);

            HttpResponseMessage deletedResponse = await client.DeleteAsync(ContactEndpointConstants.ContactEndpoint + ToDelete);
            HttpResponseMessage TryExistingDeletionResponse = await client.DeleteAsync(ContactEndpointConstants.ContactEndpoint + ToDelete);

            var deleteContact = await TryExistingDeletionResponse.Content.ReadAsStringAsync();
            JsonNode deleteMainBodyDeserialized = JsonNode.Parse(deleteContact)!;
            ((bool)deleteMainBodyDeserialized[ContactEndpointConstants.SuccessNode]).Should().BeFalse();

        }

        public async Task<Guid> TestDeleteContact_SampleRecordInsert_Utility(int IndexForSample)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            var contact = fixture._contacts[IndexForSample];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

            HttpResponseMessage response = await client.PostAsJsonAsync(ContactEndpointConstants.ContactEndpoint, createContactRequest);
            var createContact = await response.Content.ReadAsStringAsync();
            JsonNode MainBodyDeserialized = JsonNode.Parse(createContact)!;
            JsonNode CreateContactResponseJNode = MainBodyDeserialized![ContactEndpointConstants.DataNode]!;

            CreateContactResponseJNode[ContactEndpointConstants.FirstNameNode].ToString().Should().Be(contact.FirstName);
            CreateContactResponseJNode[ContactEndpointConstants.LastNameNode].ToString().Should().Be(contact.LastName);
            Guid TobeDeletedGuid = Guid.Parse(CreateContactResponseJNode[ContactEndpointConstants.IdNode].ToString());

            return TobeDeletedGuid;
        }
    }
}
