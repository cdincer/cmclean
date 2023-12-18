using cmclean.Domain.Model;
using FluentAssertions;

using System.Net.Http.Json;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint
{
    [Collection("Read Collection")]
    public class GetAllContactsTests
    {

        private readonly ContactsReadFixture _fixture;
        public GetAllContactsTests(ContactsReadFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAll()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8001/");
            HttpResponseMessage response = await client.GetAsync(
            "/api/contacts");
            List<Contact?> contactList = await response.Content.ReadFromJsonAsync<List<Contact?>>();
            contactList.Should().BeAssignableTo<List<Contact?>>();
            contactList.Count.Should().Be(5);
        }

    }
}