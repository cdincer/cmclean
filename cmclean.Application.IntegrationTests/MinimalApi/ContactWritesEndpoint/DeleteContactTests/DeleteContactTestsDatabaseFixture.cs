using cmclean.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactWritesEndpoint.DeleteContactTests
{
    public class DeleteContactTestsDatabaseFixture
    {
        public readonly List<Contact> _contacts;

        public DeleteContactTestsDatabaseFixture() 
        {
            _contacts = new List<Contact>
           {
                    new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "Deleted", LastName = "User1",
                DisplayName="DeletedUser1",BirthDate = new DateTime(1983,08,09),Email = "DeletedUser1@email.com",Phonenumber ="33333"
            },
                new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "Deleted", LastName = "User2",
                DisplayName="DeletedUser2",BirthDate = new DateTime(1946,12,17),Email = "DeletedUser2@email.com",Phonenumber ="33333"
            },
            new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mrs", FirstName = "Deleted", LastName = "User3",
                DisplayName="DeletedUser3",BirthDate = new DateTime(1976,5,25),Email = "DeletedUser3@email.com",Phonenumber ="22222"
            }
           };
        }
    }
    }

