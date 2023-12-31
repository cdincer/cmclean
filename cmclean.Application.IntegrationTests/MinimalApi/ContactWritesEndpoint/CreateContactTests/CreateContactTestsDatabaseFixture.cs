﻿using cmclean.Domain.Model;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactWritesEndpoint.CreateContactTests
{
    public class CreateContactTestsDatabaseFixture
    {
        public readonly List<Contact> _contacts;

        public CreateContactTestsDatabaseFixture()
        {
            _contacts = new List<Contact>
           {
                    new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "Danny", LastName = "Boyle",
                DisplayName="TheDirector",BirthDate = new DateTime(1956,10,20),Email = "thedirector@email.com",Phonenumber ="33333"
            },
                new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "Alex", LastName = "Garland",
                DisplayName="",BirthDate = new DateTime(1970,5,26),Email = "thewriter@email.com",Phonenumber ="33333"
            },
            new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "Cillian", LastName = "Murphy",
                DisplayName="TheActor",BirthDate = new DateTime(1976,5,25),Email = "",Phonenumber ="22222"
            },
               new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mrs", FirstName = "Naomie", LastName = "Harris",
                DisplayName="SelenaK",BirthDate = new DateTime(1976,9,6),Email = "naomie@harris.com",Phonenumber ="22222"
            },
             new()
            {
                Id  = Guid.NewGuid() ,Salutation ="Mr", FirstName = "Noah", LastName = "Huntley",
                DisplayName="Noah1234",BirthDate = new DateTime(1974,9,7),Email = "noah@huntley.com",Phonenumber ="22222"
            }

           };
        }
    }
}
