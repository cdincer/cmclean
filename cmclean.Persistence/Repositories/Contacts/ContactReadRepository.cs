﻿using cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter;
using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Domain.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

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

        var Contacts = await connection.QueryAsync<Contact>(@"SELECT Id, Salutation, Firstname, Lastname, Displayname, Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber FROM  ""Contacts""");

        return (List<Contact?>)Contacts;
    }

    public async Task<Contact?> GetByIdAsync(Guid id)
    {
        var constring = _configuration["ConnectionStrings:Default"];
        using var connection = new NpgsqlConnection
          (constring);

        var Contact = await connection.QueryFirstAsync<Contact>(@"SELECT Id, Salutation, Firstname, Lastname, Displayname, Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber FROM  ""Contacts"" where Id = @Id", new { Id = id });

        return Contact;
    }

    public async Task<List<Contact?>> GetAsync(GetContactByFilterQuery getContactByFilterQuery)
    {
        var constring = _configuration["ConnectionStrings:Default"];
        using var connection = new NpgsqlConnection
          (constring);


        foreach (var prop in getContactByFilterQuery.GetType().GetProperties())
        {
            string CurrentValue = prop.GetValue(getContactByFilterQuery, null).ToString();
            var property = typeof(GetContactByFilterQuery).GetProperty(prop.Name);
           
            if (string.IsNullOrWhiteSpace(CurrentValue))
            {
                property.SetValue(getContactByFilterQuery, null, null);
            }
            else if(CurrentValue == "01/01/0001 00:00:00")
            {
                property.SetValue(getContactByFilterQuery, DateTime.MinValue, null);
            }
        }


        var Contacts = await connection.QueryAsync<Contact?>
        (@"SELECT Id, Salutation, Firstname, Lastname, Displayname, Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber FROM ""Contacts"" where 
        (Firstname = @Firstname OR  @Firstname IS NULL) AND
        (Lastname = @Lastname OR  @Lastname IS NULL) AND
        (Displayname = @Displayname OR  @Displayname IS NULL) AND
        (Birthdate = @Birthdate OR  '1111-11-01' > @Birthdate) AND
        (Email = @Email OR  @Email IS NULL) AND
        (Phonenumber = @Phonenumber OR  @Phonenumber IS NULL)
        ", new { Firstname = getContactByFilterQuery.FirstName,
                 Lastname = getContactByFilterQuery.LastName,
                 Displayname = getContactByFilterQuery.DisplayName,
                 Birthdate = getContactByFilterQuery.BirthDate,
                 Email = getContactByFilterQuery.Email,
                 Phonenumber = getContactByFilterQuery.Phonenumber
              });

        return (List<Contact?>)Contacts;
    }
}
