using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Domain.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace cmclean.Persistence.Repositories.Contacts
{
    public class ContactWriteRepository :IContactWriteRepository
    {
        private readonly IConfiguration _configuration;
        public ContactWriteRepository( IConfiguration configuration) 
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public  async Task<Contact> AddAsync(Contact entity)
        {

            var constring = _configuration["ConnectionStrings:Default"];
            using var connection = new NpgsqlConnection
              (constring);

            var affected =
                await connection.ExecuteAsync
                    (@"INSERT INTO ""Contacts"" (Id, Firstname, Lastname, 
                    Displayname, Birthdate,CreationTimestamp,
                     LastChangeTimeStamp,Email,Phonenumber)
                    VALUES (@Id, @Firstname , @Lastname , 
                    @Displayname, @Birthdate, @CreationTimestamp,
                    @LastChangeTimeStamp,@Email,@Phonenumber)",
                            new
                            {
                                id = entity.Id,
                                Firstname = entity.FirstName,
                                Lastname = entity.LastName,
                                Displayname = entity.DisplayName,
                                Birthdate = entity.BirthDate,
                                CreationTimestamp = entity.CreationTimestamp,
                                LastChangeTimeStamp = entity.LastChangeTimeStamp,
                                Email = entity.Email,
                                Phonenumber = entity.Phonenumber
                            });

            if (affected == 0)
                throw new InvalidOperationException();

            return entity;
        }

        public bool Remove(Contact entity)
        {
            throw new NotImplementedException();
        }

        public Contact Update(Contact entity)
        {
            throw new NotImplementedException();
        }
    }
}
