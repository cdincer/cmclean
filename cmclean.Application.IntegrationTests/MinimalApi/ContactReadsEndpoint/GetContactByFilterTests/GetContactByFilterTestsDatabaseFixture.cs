using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint.GetContactByFilterTests
{
    public class GetContactByFilterTestsDatabaseFixture : IDisposable
    {
        public GetContactByFilterTestsDatabaseFixture()
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
             + $"VALUES ('{TrialGuid}','Mr', 'Jon' , 'Hamm' ,'JDog','{DateTime.Now.AddDays(12).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}',"
             + $"'{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}', '{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}','jonhamm@email.com','02124669901')";
            command.ExecuteNonQuery();
            TrialGuid = Guid.NewGuid();
            command.CommandText =
            @"INSERT INTO ""Contacts"""
            + "(Id, Salutation, Firstname, Lastname, Displayname, "
            + "Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber)"
            + $"VALUES ('{TrialGuid}','Mrs', 'Jane' , 'Hamm' ,'J2Dog','{DateTime.Now.AddDays(15).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}',"
            + $"'{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}', '{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}','janehamm@email.com','02124669902')";
            command.ExecuteNonQuery();

            TrialGuid = Guid.NewGuid();
            command.CommandText =
            @"INSERT INTO ""Contacts"""
            + "(Id, Salutation, Firstname, Lastname, Displayname, "
            + "Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber)"
            + $"VALUES ('{TrialGuid}','Mr', 'Jon' , 'Bovi' ,'JBow','{DateTime.Now.AddDays(8).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}',"
            + $"'{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}', '{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}','jonbovi@email.com','02124669902')";
            command.ExecuteNonQuery();

        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

    }
}
