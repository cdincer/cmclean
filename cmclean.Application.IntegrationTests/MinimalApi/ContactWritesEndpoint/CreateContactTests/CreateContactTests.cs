using cmclean.Application.Common.Results;
using cmclean.Application.Features.ContactFeature.Commands.CreateContact;
using FluentAssertions;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactWritesEndpoint.CreateContactTests
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
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            var contact = fixture._contacts[0];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

            HttpResponseMessage response = await client.PostAsJsonAsync(ContactEndpointConstants.ContactEndpoint, createContactRequest);
            var createContact = await response.Content.ReadAsStringAsync();
            JsonNode MainBodyDeserialized = JsonNode.Parse(createContact)!;
            JsonNode CreateContactJNode = MainBodyDeserialized![ContactEndpointConstants.DataNode]!;
            CreateContactJNode[ContactEndpointConstants.FirstNameNode].ToString().Should().BeEquivalentTo("Danny");
            CreateContactJNode[ContactEndpointConstants.LastNameNode].ToString().Should().BeEquivalentTo("Boyle");
        }
        [Fact]
        public async Task TestCreateContact_ValidContactWithoutDisplayName_ReturnMergedDisplayName()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            var contact = fixture._contacts[1];
            var createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

            HttpResponseMessage response = await client.PostAsJsonAsync(ContactEndpointConstants.ContactEndpoint, createContactRequest);
            var createContact = await response.Content.ReadAsStringAsync();
            JsonNode MainBodyDeserialized = JsonNode.Parse(createContact)!;
            JsonNode CreateContactJNode = MainBodyDeserialized![ContactEndpointConstants.DataNode]!;
            CreateContactJNode[ContactEndpointConstants.FirstNameNode].ToString().Should().BeEquivalentTo("Alex");
            CreateContactJNode[ContactEndpointConstants.LastNameNode].ToString().Should().BeEquivalentTo("Garland");
            CreateContactJNode[ContactEndpointConstants.DisplayNameNode].ToString().Should().Be(contact.Salutation + contact.FirstName + contact.LastName);
        }

        public static TheoryData<string, string, string, string, DateTime, string, string> MissingFieldCases =
        new()
        {
            { "", "Test","User1", "NoSalutation", new DateTime(2018, 12, 31), "nosalutation@email.com", "12341234" },
            { "Mr", "","User2", "NoFirstName", new DateTime(2018, 12, 31), "nofirstname@email.com", "12341234" },
            { "Mr", "Test","", "NoLastName", new DateTime(2018, 12, 31), "nolastname@email.com", "12341234" },
            { "Mr", "Test","User4", "NoEmail", new DateTime(2018, 12, 31), "", "12341234" }
        };
        [Theory, MemberData(nameof(MissingFieldCases))]
        public async Task TestCreateContact_MissingFields_ValidationErrorMessages(string Salutation, string FirstName, string LastName, string DisplayName,
                                                                DateTime BirthDate, string Email, string Phonenumber)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            var createContactRequest = new CreateContactRequest(Salutation, FirstName, LastName,
                                                                DisplayName, BirthDate, Email, Phonenumber);

            HttpResponseMessage response = await client.PostAsJsonAsync(ContactEndpointConstants.ContactEndpoint, createContactRequest);
            var createContact = await response.Content.ReadAsStringAsync();
            JsonNode MainBodyDeserialized = JsonNode.Parse(createContact)!;
            JsonNode SuccessJNode = MainBodyDeserialized![ContactEndpointConstants.SuccessNode]!;
            JsonNode MessageJNode = MainBodyDeserialized![ContactEndpointConstants.MessageNode]!;
            ((bool)SuccessJNode).Should().BeFalse();
            MessageJNode.ToString().Should().NotBeNullOrEmpty();
            MessageJNode.ToString().Length.Should().BeGreaterThan(7);//Detailed validation error check, succesfull responses have only "success" in them for messages.
        }

        [Fact]
        public async Task TestCreateContact_InsertDuplicateContact_ReturnValidationError()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            var contact = fixture._contacts[3];
            CreateContactRequest createContactRequest = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

            HttpResponseMessage OrgResponse = await client.PostAsJsonAsync(ContactEndpointConstants.ContactEndpoint, createContactRequest);
            var OrgCreateContact = await OrgResponse.Content.ReadAsStringAsync();
            JsonNode OrgBodyDeserialized = JsonNode.Parse(OrgCreateContact)!;
            JsonNode CreateContactJNode = OrgBodyDeserialized![ContactEndpointConstants.DataNode]!;

            CreateContactJNode[ContactEndpointConstants.FirstNameNode].ToString().Should().BeEquivalentTo("Naomie");
            CreateContactJNode[ContactEndpointConstants.LastNameNode].ToString().Should().BeEquivalentTo("Harris");
            CreateContactJNode[ContactEndpointConstants.DisplayNameNode].ToString().Should().Be("SelenaK");

            HttpResponseMessage dresponse = await client.PostAsJsonAsync(ContactEndpointConstants.ContactEndpoint, createContactRequest);
            var duplicateContact = await dresponse.Content.ReadAsStringAsync();
            JsonNode DupReqDeserialized = JsonNode.Parse(duplicateContact)!;
            JsonNode SuccessJNode = DupReqDeserialized![ContactEndpointConstants.SuccessNode]!;
            JsonNode MessageJNode = DupReqDeserialized![ContactEndpointConstants.MessageNode]!;
            ((bool)SuccessJNode).Should().BeFalse();
            MessageJNode.ToString().Should().NotBeNullOrEmpty();
            MessageJNode.ToString().Length.Should().BeGreaterThan(7);
        }

        [Fact]
        public async Task TestCreateContact_InsertDuplicateEmailWithDifferentFields_ReturnValidationError()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ContactEndpointConstants.BaseEndpoint);
            var contact = fixture._contacts[4];
            CreateContactRequest UniqueContactReq = new CreateContactRequest(contact.Salutation, contact.FirstName, contact.LastName,
                                                                contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

            HttpResponseMessage response = await client.PostAsJsonAsync(ContactEndpointConstants.ContactEndpoint, UniqueContactReq);
            var UniqueContactResponse = await response.Content.ReadAsStringAsync();
            JsonNode MainBodyDeserialized = JsonNode.Parse(UniqueContactResponse)!;
            JsonNode CreateContactJNode = MainBodyDeserialized![ContactEndpointConstants.DataNode]!;
            CreateContactJNode[ContactEndpointConstants.FirstNameNode].ToString().Should().BeEquivalentTo("Noah");
            CreateContactJNode[ContactEndpointConstants.LastNameNode].ToString().Should().BeEquivalentTo("Huntley");
            CreateContactJNode[ContactEndpointConstants.DisplayNameNode].ToString().Should().Be("Noah1234");

            CreateContactRequest DifferentContactSameEmailRequest = new CreateContactRequest("Te", "stFirst", "Name",
                                                               contact.DisplayName, contact.BirthDate, contact.Email, contact.Phonenumber);

            HttpResponseMessage dcseResponse = await client.PostAsJsonAsync(ContactEndpointConstants.ContactEndpoint, DifferentContactSameEmailRequest);
            var dcseContact = await dcseResponse.Content.ReadAsStringAsync();
            JsonNode DupDeserialized = JsonNode.Parse(dcseContact)!;
            JsonNode SuccessJNode = DupDeserialized![ContactEndpointConstants.SuccessNode]!;
            JsonNode MessageJNode = DupDeserialized![ContactEndpointConstants.MessageNode]!;
            ((bool)SuccessJNode).Should().BeFalse();
            MessageJNode.ToString().Should().NotBeNullOrEmpty();
            MessageJNode.ToString().Length.Should().BeGreaterThan(7);
        }
    }
}
