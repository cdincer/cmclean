using cmclean.Domain.Model;
using FluentAssertions;

using System.Net.Http.Json;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint.GetAllContactsTests
{
    [Collection("ContactsEndpoint Collection")]
    public class GetAllContactsTests
    {
        private readonly int UserBirthDateCheck = 14;

        public GetAllContactsTests()
        {
        }

        [Fact]
        public async Task TestGetAllContacts_CheckValidity_ResultsMoreThanTwo()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            HttpResponseMessage response = await client.GetAsync(
            "/api/contacts");
            List<Contact?> contactList = await response.Content.ReadFromJsonAsync<List<Contact?>>();

            contactList.Should().BeAssignableTo<List<Contact?>>();
            contactList.Count.Should().BeGreaterThan(2);
        }


        [Fact]
        public async Task TestGetAllContacts_CheckBirthdate_AllValid()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            HttpResponseMessage response = await client.GetAsync(
            "/api/contacts");
            List<Contact?> contactList = await response.Content.ReadFromJsonAsync<List<Contact?>>();

            foreach (Contact element in contactList)
            {
                int requiredYearsForCalculation = DateTime.Now.Year - element.BirthDate.Year;
                element.BirthDate.AddYears(requiredYearsForCalculation);

                if (element.BirthDate >= DateTime.Now && element.BirthDate <= DateTime.Now.AddDays(UserBirthDateCheck))
                {
                    element.NotifyHasBirthdaySoon.Should().BeTrue();
                }
                else
                {
                    element.NotifyHasBirthdaySoon.Should().BeFalse();
                }
            }
            contactList.Should().BeAssignableTo<List<Contact?>>();
            contactList.Count.Should().BeGreaterThan(2);
        }
    }
}