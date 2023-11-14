using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Domain.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public Task<Contact?> GetAsync(string filter)
    {
        throw new NotImplementedException();
    }
}
