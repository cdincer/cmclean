using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Domain.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace cmclean.Persistence.Repositories.Contacts
{
    public class ContactWriteRepository : GenericWriteRepository<Contact>, IContactWriteRepository
    {
        private readonly IConfiguration _configuration;
        public ContactWriteRepository(CmcleanDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public override async Task<Contact> AddAsync(Contact entity)
        {

            var tester1 = _configuration["ConnectionStrings:Default"];
            using var connection = new NpgsqlConnection
              (tester1);

            var affected =
                await connection.ExecuteAsync
                    (@"INSERT INTO ""Contacts"" (Id, Firstname, Lastname, 
                    Displayname, Birthdate)
                    VALUES (@Id, @Firstname , @Lastname , 
                    @Displayname, @Birthdate)",
                            new
                            {
                                id = entity.Id,
                                Firstname = entity.FirstName,
                                Lastname = entity.LastName,
                                Displayname = entity.DisplayName,
                                Birthdate = entity.BirthDate,

                            });

            if (affected == 0)
                throw new InvalidOperationException();

            return entity;
        }
    }
}
