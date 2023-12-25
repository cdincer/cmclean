using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;
using FluentAssertions;
using System.Net.Http.Json;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint.GetContactByIdTests
{
    [Collection("ContactsEndpoint Collection")]
    public class GetContactByIdTests : IClassFixture<GetContactByIdTestsDatabaseFixture>
    {
        private readonly int UserBirthDateCheck = 14;

        GetContactByIdTestsDatabaseFixture fixture;
        public GetContactByIdTests(GetContactByIdTestsDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task TestGetContactById_GetSingleRecordWithId_NoValidationExcepiton()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8001/");

            HttpResponseMessage response = await client.GetAsync("api/contacts/49900c0c-8119-4fd4-98cd-f0e643231528");
            GetContactByIdResponse contactById = await response.Content.ReadFromJsonAsync<GetContactByIdResponse>();

            contactById.Should().BeAssignableTo<GetContactByIdResponse>();
            contactById.FirstName.Should().BeEquivalentTo("Jane");
            contactById.LastName.Should().BeEquivalentTo("Levy");
        }

          public static TheoryData<string, string, string, string, DateTime, string, string> BirthdayCalculationCases =
          new()
          {
                    { "Mr", "Test","User1", "User1Surname", new DateTime(2000, 01, 01), "behindmonths@email.com", "12341234" },
                    { "Mr", "Test","User2", "User2Surname", new DateTime(2000, DateTime.Now.Month, DateTime.Now.AddDays(7).Day), "7daysbirthday@email.com", "12341234" },
                    { "Mr", "Test","User3", "User3Surname", new DateTime(2000, DateTime.Now.Month, DateTime.Now.AddDays(12).Day), "12daysbirthday@email.com", "12341234" },
                    { "Mr", "Test","User4", "User4Surname", new DateTime(2000, DateTime.Now.Month, DateTime.Now.AddDays(14).Day), "14daysbirthday@email.com", "12341234" },
                    { "Mr", "Test","User4", "User4Surname", new DateTime(2000, DateTime.Now.Month, DateTime.Now.AddDays(15).Day), "afterlimitbirthday@email.com", "12341234" }
          };
        [Theory, MemberData(nameof(BirthdayCalculationCases))]
        public async Task TestCreateContact_BirthdateCalculation_ValidResponses(string Salutation, string FirstName, string LastName, string DisplayName,
                                                                DateTime BirthDate, string Email, string Phonenumber)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8001/");
            var createContactRequest = new CreateContactRequest(Salutation, FirstName, LastName,
                                                                DisplayName, BirthDate, Email, Phonenumber);

            HttpResponseMessage response = await client.PostAsJsonAsync("api/contacts/", createContactRequest);
            CreateContactResponse createContact = await response.Content.ReadFromJsonAsync<CreateContactResponse>();
            createContact.Should().BeAssignableTo<CreateContactResponse>();
            
            response = await client.GetAsync("api/contacts/"+ createContact.Id);
            GetContactByIdResponse getContactByIdResponse = await response.Content.ReadFromJsonAsync<GetContactByIdResponse>();

            int requiredYearsForCalculation = DateTime.Now.Year - BirthDate.Year;
            BirthDate.AddYears(requiredYearsForCalculation);

            if (BirthDate >= DateTime.Now && BirthDate <= DateTime.Now.AddDays(UserBirthDateCheck))
            {
                getContactByIdResponse.NotifyHasBirthdaySoon.Should().BeTrue();
            }
            else
            {
                getContactByIdResponse.NotifyHasBirthdaySoon.Should().BeFalse();
            }
        }
    }
}
