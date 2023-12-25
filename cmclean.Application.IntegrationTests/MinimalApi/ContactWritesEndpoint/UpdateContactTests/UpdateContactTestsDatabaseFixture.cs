using cmclean.Application.IntegrationTests.MinimalApi.ContactWritesEndpoint.CreateContactTests;
using cmclean.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactWritesEndpoint.UpdateContactTests
{
    public class UpdateContactTestsDatabaseFixture
    {
        public readonly List<Contact> _contacts;

        public UpdateContactTestsDatabaseFixture()
        {
            _contacts = new List<Contact>
           {
                    new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "Dan", LastName = "Levy",
                DisplayName="DavidRose",BirthDate = new DateTime(1983,08,09),Email = "davidrose@email.com",Phonenumber ="33333"
            },
                new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "Eugene", LastName = "Levy",
                DisplayName="JohnnyRose",BirthDate = new DateTime(1946,12,17),Email = "johnnyrose@email.com",Phonenumber ="33333"
            },
            new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mrs", FirstName = "Annie", LastName = "Murphy",
                DisplayName="AlexisRose",BirthDate = new DateTime(1976,5,25),Email = "alexisrose@email.com",Phonenumber ="22222"
            }

           };
        }
    }
}
