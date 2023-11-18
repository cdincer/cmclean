using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Domain.Model;
using Dapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.Persistence.Repositories.Contacts;

public class ContactReadRepository : IContactReadRepository
{
    private readonly IConfiguration _configuration;
    public ContactReadRepository(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<List<Contact?>> GetAll()
    {
        var constring = _configuration["ConnectionStrings:Default"];
        using var connection = new NpgsqlConnection
          (constring);

        var Contacts = await connection.QueryAsync<Contact>(@"SELECT Id, Firstname, Lastname, Displayname, Birthdate FROM  ""Contacts""");

        return (List<Contact?>)Contacts;
    }

    public async Task<Contact?> GetByIdAsync(Guid id)
    {
        var constring = _configuration["ConnectionStrings:Default"];
        using var connection = new NpgsqlConnection
          (constring);

        var Contact = await connection.QueryFirstAsync<Contact>(@"SELECT Id, Firstname, Lastname, Displayname, Birthdate FROM  ""Contacts"" where Id = @Id", new { Id = id });

        return Contact;
    }

    public async Task<List<Contact?>> GetAsync(GetContactByFilterQuery getContactByFilterQuery)
    {
        var constring = _configuration["ConnectionStrings:Default"];
        using var connection = new NpgsqlConnection
          (constring);


        foreach (var prop in getContactByFilterQuery.GetType().GetProperties())
        {
            Console.WriteLine(prop.Name + prop.GetValue(getContactByFilterQuery, null));
        }



        var Contacts = await connection.QueryAsync<Contact?>(@"SELECT Id, Firstname, Lastname, Displayname, Birthdate FROM  ""Contacts"" where 
        (Firstname = @Firstname OR  @Firstname IS NULL) AND
        (Lastname = @Lastname OR  @Lastname IS NULL) AND
        (Displayname = @Displayname OR  @Displayname IS NULL) AND
        (Birthdate = @Birthdate OR  @Birthdate IS NULL)
        ", new { Firstname = getContactByFilterQuery.FirstName,
                 Lastname = getContactByFilterQuery.LastName,
                 Displayname = getContactByFilterQuery.DisplayName,
                 Birthdate = getContactByFilterQuery.DateOfBirth});

        return (List<Contact?>)Contacts;
    }
}
