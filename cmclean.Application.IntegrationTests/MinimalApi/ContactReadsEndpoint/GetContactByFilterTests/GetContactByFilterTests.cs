using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using FluentAssertions;
using System.Net.Http.Json;


namespace cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint.GetContactByFilterTests
{
    [Collection("ContactsEndpoint Collection")]
    public class GetContactByFilterTests : IClassFixture<GetContactByFilterTestsDatabaseFixture>
    {
        GetContactByFilterTestsDatabaseFixture fixture;
        public static string FilterEndpoint = "api/contacts/filter";
        public GetContactByFilterTests(GetContactByFilterTestsDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task TestGetContactByFilter_GetOneRecordWithThatFirstName_NoValidationExcepiton()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            GetContactByFilterTestQuery getContactByFilterTestQuery = new();
            getContactByFilterTestQuery.FirstName = "Evelyn";
            getContactByFilterTestQuery.LastName = "";
            getContactByFilterTestQuery.DisplayName = "";
            getContactByFilterTestQuery.Email = "";
            getContactByFilterTestQuery.Phonenumber = "";
            getContactByFilterTestQuery.BirthDate = DateTime.MinValue;

            HttpResponseMessage response = await client.PostAsJsonAsync(FilterEndpoint, getContactByFilterTestQuery);

            List<GetContactByFilterResponse?> contactList = await response.Content.ReadFromJsonAsync<List<GetContactByFilterResponse?>>();
            contactList.Should().BeAssignableTo<List<GetContactByFilterResponse?>>();
            contactList.Count.Should().Be(1);
        }

        [Fact]
        public async Task TestGetContactByFilter_GetMultipleRecordWithThatFirstName_NoValidationExcepiton()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            GetContactByFilterTestQuery getContactByFilterTestQuery = new();
            getContactByFilterTestQuery.FirstName = "Jon";
            getContactByFilterTestQuery.LastName = "";
            getContactByFilterTestQuery.DisplayName = "";
            getContactByFilterTestQuery.Email = "";
            getContactByFilterTestQuery.Phonenumber = "";
            getContactByFilterTestQuery.BirthDate = DateTime.MinValue;

            HttpResponseMessage response = await client.PostAsJsonAsync(FilterEndpoint, getContactByFilterTestQuery);

            List<GetContactByFilterResponse?> contactList = await response.Content.ReadFromJsonAsync<List<GetContactByFilterResponse?>>();
            contactList.Should().BeAssignableTo<List<GetContactByFilterResponse?>>();
            contactList.Count.Should().Be(2);
        }

        [Fact]
        public async Task TestGetContactByFilter_GetMultipleRecordWithThatLastName_NoValidationExcepiton()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            GetContactByFilterTestQuery getContactByFilterTestQuery = new();
            getContactByFilterTestQuery.FirstName = "";
            getContactByFilterTestQuery.LastName = "Hamm";
            getContactByFilterTestQuery.DisplayName = "";
            getContactByFilterTestQuery.Email = "";
            getContactByFilterTestQuery.Phonenumber = "";
            getContactByFilterTestQuery.BirthDate = DateTime.MinValue;

            HttpResponseMessage response = await client.PostAsJsonAsync(FilterEndpoint, getContactByFilterTestQuery);

            List<GetContactByFilterResponse?> contactList = await response.Content.ReadFromJsonAsync<List<GetContactByFilterResponse?>>();
            contactList.Should().BeAssignableTo<List<GetContactByFilterResponse?>>();
            contactList.Count.Should().Be(2);
        }



        [Fact]
        public async Task TestGetContactByFilter_GetSingleContactWithFirstNameAndLastName_NoValidationExcepiton()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            GetContactByFilterTestQuery getContactByFilterTestQuery = new();
            getContactByFilterTestQuery.FirstName = "Jon";
            getContactByFilterTestQuery.LastName = "Hamm";
            getContactByFilterTestQuery.DisplayName = "";
            getContactByFilterTestQuery.Email = "";
            getContactByFilterTestQuery.Phonenumber = "";
            getContactByFilterTestQuery.BirthDate = DateTime.MinValue;

            HttpResponseMessage response = await client.PostAsJsonAsync(FilterEndpoint, getContactByFilterTestQuery);

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
