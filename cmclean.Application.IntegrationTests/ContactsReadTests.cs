using cmclean.Domain.Model;
using Docker.DotNet;
using Docker.DotNet.Models;
using Npgsql;
using Dapper;
using System.Data;
using FluentAssertions;
using cmclean.Application.Features.ContactFeature.Queries.GetAllContacts;
using AutoMapper;
using cmclean.Application.Interfaces.Repositories.Contacts;
using Xunit;

namespace cmclean.Application.IntegrationTests
{
    public class ContactsReadTests : IClassFixture<ContactsReadFixture>
    {
        const string ContainerName = "IntegrationTestDB";

        private readonly ContactsReadFixture _fixture;
        public ContactsReadTests(ContactsReadFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAll()
        {
            var ConnectionString2 = "Server=127.0.0.1;Port=5432;Database=Contactmanagerdb;User Id=admin;Password=admin1234;Timeout=50;";
            await using var connection2 = new NpgsqlConnection(ConnectionString2);
            var contacts = (List<Contact?>) await connection2.QueryAsync<Contact>(@"SELECT Id, Salutation, Firstname, Lastname, Displayname, 
                                                                                    Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber FROM  ""Contacts""");

            contacts.Should().BeAssignableTo<List<Contact?>>();
            contacts.Count.Should().Be(5);
        }
    }
}