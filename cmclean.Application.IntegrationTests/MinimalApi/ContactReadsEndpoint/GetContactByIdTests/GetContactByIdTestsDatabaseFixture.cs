using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint.GetContactByIdTests
{
    public class GetContactByIdTestsDatabaseFixture
    {
        public GetContactByIdTestsDatabaseFixture()
        {
            string ConnectionString = ContactEndpointConstants.IntegrationTestDBConnection;
            using var connection = new NpgsqlConnection
            (ConnectionString);
            connection.Open();

            using var command = new NpgsqlCommand
            {
                Connection = connection
            };
            Guid TrialGuid = Guid.NewGuid();
            command.CommandText =
             @"INSERT INTO ""Contacts"""
             + "(Id, Salutation, Firstname, Lastname, Displayname, "
             + "Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber)"
             + $"VALUES ('9a40ab48-cac4-44e6-a50c-c4fa3470591b','Mr', 'Jeremy' , 'Sisto' ,'GeorgeAltman','{DateTime.Now.AddDays(12).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}',"
             + $"'{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}', '{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}','jeremysisto@email.com','02124669901')";
            command.ExecuteNonQuery();
            TrialGuid = Guid.NewGuid();
            command.CommandText =
            @"INSERT INTO ""Contacts"""
            + "(Id, Salutation, Firstname, Lastname, Displayname, "
            + "Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber)"
            + $"VALUES ('49900c0c-8119-4fd4-98cd-f0e643231528','Mrs', 'Jane' , 'Levy' ,'AsleyWilliams','1989-12-29T19:10:25',"
            + $"'{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}', '{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}','janelevy@email.com','02124669902')";
            command.ExecuteNonQuery();
        }
    }
}
