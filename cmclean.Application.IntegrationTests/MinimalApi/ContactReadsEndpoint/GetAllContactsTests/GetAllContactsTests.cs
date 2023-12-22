using cmclean.Domain.Model;
using FluentAssertions;

using System.Net.Http.Json;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint.GetAllContactsTests
{
    [Collection("ContactsEndpoint Collection")]
    public class GetAllContactsTests
    {

        public GetAllContactsTests()
        {
        }

        [Fact]
        public async Task TestGetAllContacts_CheckValidity_ResultsMoreThanTwo()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8001/");
            HttpResponseMessage response = await client.GetAsync(
            "/api/contacts");
            List<Contact?> contactList = await response.Content.ReadFromJsonAsync<List<Contact?>>();
            contactList.Should().BeAssignableTo<List<Contact?>>();
            contactList.Count.Should().BeGreaterThan(2);
        }
    }
}