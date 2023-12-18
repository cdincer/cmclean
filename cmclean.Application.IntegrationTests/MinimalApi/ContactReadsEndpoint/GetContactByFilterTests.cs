using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using FluentAssertions;
using Npgsql;
using System.Net.Http.Json;


namespace cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint
{
    [Collection("Read Collection")]
    public class GetContactByFilterTests
    {
        private readonly ContactsReadFixture _fixture;
        public GetContactByFilterTests(ContactsReadFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetByFilterName()
        {
        

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8001/");
            GetContactByFilterTestQuery getContactByFilterTestQuery = new();
            getContactByFilterTestQuery.FirstName = "Evelyn";
            getContactByFilterTestQuery.LastName = "";
            getContactByFilterTestQuery.DisplayName = "";
            getContactByFilterTestQuery.Email = "";
            getContactByFilterTestQuery.Phonenumber = "";
            getContactByFilterTestQuery.BirthDate = DateTime.MinValue;

            HttpResponseMessage response = await client.PostAsJsonAsync("api/contacts/filter", getContactByFilterTestQuery);

            List<GetContactByFilterResponse?> contactList = await response.Content.ReadFromJsonAsync<List<GetContactByFilterResponse?>>();
            contactList.Should().BeAssignableTo<List<GetContactByFilterResponse?>>();
            contactList.Count.Should().Be(1);
        }



        [Fact]
        public async Task GetByFilterNameAndSurname()
        {


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8001/");
            GetContactByFilterTestQuery getContactByFilterTestQuery = new();
            getContactByFilterTestQuery.FirstName = "Jon";
            getContactByFilterTestQuery.LastName = "Hamm";
            getContactByFilterTestQuery.DisplayName = "";
            getContactByFilterTestQuery.Email = "";
            getContactByFilterTestQuery.Phonenumber = "";
            getContactByFilterTestQuery.BirthDate = DateTime.MinValue;

            HttpResponseMessage response = await client.PostAsJsonAsync("api/contacts/filter", getContactByFilterTestQuery);

            List<GetContactByFilterResponse?> contactList = await response.Content.ReadFromJsonAsync<List<GetContactByFilterResponse?>>();
            contactList.Should().BeAssignableTo<List<GetContactByFilterResponse?>>();
            contactList.Count.Should().Be(1);
        }

        public class GetContactByFilterTestQuery
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string DisplayName { get; set; }
            public DateTime BirthDate { get; set; }
            public string Email { get; set; }
            public string Phonenumber { get; set; }
        }

    }
}
