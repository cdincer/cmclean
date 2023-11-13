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

public class ContactReadRepository : GenericReadRepository<Contact>, IContactReadRepository
{
    private readonly IConfiguration _configuration;
    public ContactReadRepository(CmcleanDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public override async Task<List<Contact>> GetAll(bool asNoTracking = false)
    {

        var tester1 = _configuration["ConnectionStrings:Default"];
        using var connection = new NpgsqlConnection
          (tester1);

        var Contacts = await connection.QueryAsync<Contact>(@"SELECT Id, Firstname, Lastname, Displayname, Birthdate FROM  ""Contacts""");

    

        return (List<Contact>)Contacts;
    }
}
