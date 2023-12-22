using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;
using cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint.GetContactByFilterTests;
using cmclean.Domain.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactWritesEndpoint
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
        public async Task TestCreateContact_ValidContactWithoutDisplayName_NoException()
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
            createContact.DisplayName.Should().Be(contact.Salutation+contact.FirstName+ contact.LastName);
        }


        //[Fact]
        //public async Task TestCreateContact_NoEmail_NoException()
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:8001/");
        //    var contact = fixture._contacts[2];
        //    var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
        //                                                        contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

        //    HttpResponseMessage response = await client.PostAsJsonAsync("api/contacts/", createContactRequest);
        //    CreateContactResponse createContact = await response.Content.ReadFromJsonAsync<CreateContactResponse>();       
        //}
    }
}
