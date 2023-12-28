﻿using cmclean.Application.Common.Results;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Commands.UpdateContact;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;
using cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint.GetContactByIdTests;
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
            Guid TobeUpdatedGuid = Guid.Parse(CreateContactResponseJSONFormat["id"].ToString());
            var updateContactRequest = new UpdateContactRequest(TobeUpdatedGuid, contact.Salutation, contact.FirstName, contact.LastName,
                                                              "", contact.BirthDate, contact.Email, contact.Phonenumber);

            var updateResponse = await client.PutAsJsonAsync("api/contacts/", updateContactRequest);
            var updateContact = await updateResponse.Content.ReadAsStringAsync();
            JsonNode updateMainBodyDeserialized = JsonNode.Parse(updateContact)!;
            ((bool)updateMainBodyDeserialized["success"]).Should().BeTrue();

            var getResponse = await client.GetAsync("api/contacts/" + TobeUpdatedGuid);
            GetContactByIdResponse getContactByIdResponse = await getResponse.Content.ReadFromJsonAsync<GetContactByIdResponse>();
            getContactByIdResponse.Should().BeAssignableTo<GetContactByIdResponse>();
            getContactByIdResponse.DisplayName.Should().Be(contact.Salutation+contact.FirstName+contact.LastName);
        }

        [Fact]
        public async Task TestUpdateContact_UpdateContactWithEmptyEmail_GracefulFailure()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8001/");
            var contact = fixture._contacts[1];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);
            HttpResponseMessage response = await client.PostAsJsonAsync("api/contacts/", createContactRequest);
            var createContact = await response.Content.ReadAsStringAsync();
            JsonNode MainBodyDeserialized = JsonNode.Parse(createContact)!;
            JsonNode CreateContactResponseJSONFormat = MainBodyDeserialized!["data"]!;
            CreateContactResponseJSONFormat["firstName"].ToString().Should().NotBeNullOrEmpty();
            CreateContactResponseJSONFormat["lastName"].ToString().Should().NotBeNullOrEmpty();
            Guid TobeUpdatedGuid = Guid.Parse(CreateContactResponseJSONFormat["id"].ToString());

            var updateContactRequest = new UpdateContactRequest(TobeUpdatedGuid, contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, "", contact.Phonenumber);

           var updateResponse = await client.PutAsJsonAsync("api/contacts/", updateContactRequest);
            var updateContact = await updateResponse.Content.ReadAsStringAsync();
            JsonNode updateMainBodyDeserialized = JsonNode.Parse(updateContact)!;
            ((bool)updateMainBodyDeserialized["success"]).Should().BeFalse();
        }

        [Fact]
        public async Task TestUpdateContact_UpdateContactWithNewName_UpdateContactResponse()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8001/");
            var contact = fixture._contacts[2];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

            HttpResponseMessage response = await client.PostAsJsonAsync("api/contacts/", createContactRequest);
            var createContact = await response.Content.ReadAsStringAsync();
            JsonNode MainBodyDeserialized = JsonNode.Parse(createContact)!;
            JsonNode CreateContactResponseJSONFormat = MainBodyDeserialized!["data"]!;
            CreateContactResponseJSONFormat["firstName"].ToString().Should().NotBeNullOrEmpty();
            CreateContactResponseJSONFormat["lastName"].ToString().Should().NotBeNullOrEmpty();
            Guid TobeUpdatedGuid = Guid.Parse(CreateContactResponseJSONFormat["id"].ToString());

            var updateContactRequest = new UpdateContactRequest(TobeUpdatedGuid, contact.Salutation, "Alexia", contact.LastName,
                                                               contact.Email, contact.BirthDate, contact.Email, contact.Phonenumber);

            var updateResponse = await client.PutAsJsonAsync("api/contacts/", updateContactRequest);
            var updateContact = await updateResponse.Content.ReadAsStringAsync();
            JsonNode updateMainBodyDeserialized = JsonNode.Parse(updateContact)!;
            ((bool)updateMainBodyDeserialized["success"]).Should().BeTrue();

            response = await client.GetAsync("api/contacts/" + TobeUpdatedGuid);
            GetContactByIdResponse getContactByIdResponse = await response.Content.ReadFromJsonAsync<GetContactByIdResponse>();
            getContactByIdResponse.Should().BeAssignableTo<GetContactByIdResponse>();
            getContactByIdResponse.FirstName.Should().Be("Alexia");

        }
    }
}
