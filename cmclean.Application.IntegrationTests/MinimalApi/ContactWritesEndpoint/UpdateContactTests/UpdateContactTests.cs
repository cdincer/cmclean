using cmclean.Application.Common.Results;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Commands.UpdateContact;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;
using cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint.GetContactByIdTests;
using cmclean.Domain.Model;
using FluentAssertions;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactWritesEndpoint.UpdateContactTests
{
    [Collection("ContactsEndpoint Collection")]

    public class UpdateContactTests : IClassFixture<UpdateContactTestsDatabaseFixture>
    {


        UpdateContactTestsDatabaseFixture fixture;

        public UpdateContactTests(UpdateContactTestsDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }


        [Fact]
        public async Task TestUpdateContact_UpdateContactWithDisplayNameEmpty_DisplayNameConcanated()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            UpdateContactRequest Sample = await TestUpdateContact_SampleRecordInsert_Utility(0);
            Guid TobeUpdatedGuid = Sample.Id;
            var updateContactRequest = Sample;
            updateContactRequest.DisplayName = "";

            var updateResponse = await client.PutAsJsonAsync(ContactEndpointConstants.ContactEndpoint, updateContactRequest);
            var updateContact = await updateResponse.Content.ReadAsStringAsync();

            JsonNode updateMainBodyDeserialized = JsonNode.Parse(updateContact)!;
            ((bool)updateMainBodyDeserialized[ContactEndpointConstants.SuccessNode]).Should().BeTrue();

            var getResponse = await client.GetAsync(ContactEndpointConstants.ContactEndpoint + TobeUpdatedGuid);
            GetContactByIdResponse getContactByIdResponse = await getResponse.Content.ReadFromJsonAsync<GetContactByIdResponse>();
            getContactByIdResponse.Should().BeAssignableTo<GetContactByIdResponse>();
            getContactByIdResponse.DisplayName.Should().Be(updateContactRequest.Salutation+ updateContactRequest.FirstName+ updateContactRequest.LastName);
        }

        [Fact]
        public async Task TestUpdateContact_UpdateContactWithEmptyEmail_GracefulFailure()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            UpdateContactRequest Sample = await TestUpdateContact_SampleRecordInsert_Utility(1);
            Guid TobeUpdatedGuid = Sample.Id;
            var updateContactRequest = Sample;
            updateContactRequest.Email = "";

            var updateResponse = await client.PutAsJsonAsync(ContactEndpointConstants.ContactEndpoint, updateContactRequest);
            var updateContact = await updateResponse.Content.ReadAsStringAsync();
            JsonNode updateMainBodyDeserialized = JsonNode.Parse(updateContact)!;
            ((bool)updateMainBodyDeserialized[ContactEndpointConstants.SuccessNode]).Should().BeFalse();
        }

        [Fact]
        public async Task TestUpdateContact_UpdateContactWithNewName_UpdateContactResponse()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            UpdateContactRequest Sample = await TestUpdateContact_SampleRecordInsert_Utility(2);
            Guid TobeUpdatedGuid = Sample.Id;
            var updateContactRequest = Sample;
            updateContactRequest.FirstName = "Alexia";

            var updateResponse = await client.PutAsJsonAsync(ContactEndpointConstants.ContactEndpoint, updateContactRequest);
            var updateContact = await updateResponse.Content.ReadAsStringAsync();
            JsonNode updateMainBodyDeserialized = JsonNode.Parse(updateContact)!;
            ((bool)updateMainBodyDeserialized[ContactEndpointConstants.SuccessNode]).Should().BeTrue();

            HttpResponseMessage response = await client.GetAsync(ContactEndpointConstants.ContactEndpoint + TobeUpdatedGuid);
            GetContactByIdResponse getContactByIdResponse = await response.Content.ReadFromJsonAsync<GetContactByIdResponse>();
            getContactByIdResponse.Should().BeAssignableTo<GetContactByIdResponse>();
            getContactByIdResponse.FirstName.Should().Be("Alexia");

        }
    
        public async Task<UpdateContactRequest> TestUpdateContact_SampleRecordInsert_Utility(int IndexForSample)
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
            Guid TobeUpdatedGuid = Guid.Parse(CreateContactResponseJNode[ContactEndpointConstants.IdNode].ToString());

            var updateContactRequest = new UpdateContactRequest(TobeUpdatedGuid,contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

            return updateContactRequest;
        }
    }
}
