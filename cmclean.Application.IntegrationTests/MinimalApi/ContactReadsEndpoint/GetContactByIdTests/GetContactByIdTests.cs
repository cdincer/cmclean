using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using cmclean.Application.Features.ContactFeature.Queries.GetContactById;
using cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint.GetContactByFilterTests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint.GetContactByFilterTests.GetContactByFilterTests;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint.GetContactByIdTests
{
    [Collection("Read Collection")]
    public class GetContactByIdTests : IClassFixture<GetContactByIdTestsDatabaseFixture>
    {
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
    }


}
