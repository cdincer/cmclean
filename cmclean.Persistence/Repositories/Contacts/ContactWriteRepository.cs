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
                    (@"INSERT INTO ""Contacts"" (Id, Salutation, Firstname, Lastname, 
                    Displayname, Birthdate,CreationTimestamp,
                     LastChangeTimeStamp,Email,Phonenumber)
                    VALUES (@Id, @Salutation, @Firstname , @Lastname , 
                    @Displayname, @Birthdate, @CreationTimestamp,
                    @LastChangeTimeStamp,@Email,@Phonenumber)",
                            new
                            {
                                id = entity.Id,
                                Salutation = entity.Salutation,
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

        public async Task<bool> Remove(Contact entity)
        {
            var constring = _configuration["ConnectionStrings:Default"];
            using var connection = new NpgsqlConnection
              (constring);

            var affected =
                await connection.ExecuteAsync
                    (@"DELETE FROM ""Contacts"" WHERE Id = @Id",
                new { Id = entity.Id });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> Update(Contact entity)
        {
            var constring = _configuration["ConnectionStrings:Default"];
            using var connection = new NpgsqlConnection
              (constring);

            var affected =
               await connection.ExecuteAsync
                   (@"UPDATE ""Contacts"" SET Salutation = @Salutation, Firstname =  @Firstname, 
                    Lastname = @Lastname , Displayname = @Displayname, Birthdate = @Birthdate, 
                    LastChangeTimestamp = @LastChangeTimestamp, Email = @Email,Phonenumber = @Phonenumber
                    WHERE Id = @Id",
                           new
                           {
                               id = entity.Id,
                               Salutation = entity.Salutation,
                               Firstname = entity.FirstName,
                               Lastname = entity.LastName,
                               Displayname = entity.DisplayName,
                               Birthdate = entity.BirthDate,
                               LastChangeTimeStamp = entity.LastChangeTimeStamp,
                               Email = entity.Email,
                               Phonenumber = entity.Phonenumber
                           });

            if (affected == 0)
                return false;

            return true;
        }
    }
}
