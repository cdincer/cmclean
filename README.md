# cmclean
Contact Manager made in Clean Architecture.
Made with Visual Studio 2022.
Use Docker-compose to start everything up through CLI
Or you can use the debug option in Visual Studio to start it up and look through it.


# Query Testing
  {
    "firstName": "",
    "lastName": "Dicer",
    "displayName":"",
    "birthDate": "0001-01-01T00:00:00Z",
    "email": "",
    "phonenumber": ""
  }

 {
    "firstName": "Kimberly",
    "lastName": "",
    "displayName":"",
    "birthDate": "0001-01-01T00:00:00Z",
    "email": "",
    "phonenumber": ""
  }

   {
    "firstName": "",
    "lastName": "",
    "displayName":"",
    "birthDate": "0001-01-01T00:00:00Z",
    "email": "trialrun3@email.com",
    "phonenumber": ""
  }

# Random testing scenarios

//Third user set at the start up with a set guid for update/get user functions testing if you choose to do so //it's guid is :4b2056a9-7ee4-47b1-a64f-15770ceab7aa

//Test Scenario for Birthday
{ "salutation": "Mrs", "firstname": "TestFirstName1", "lastname": "TestLastName1", "email": "working@email.com", "displayname": "TestingDisplayName1", "birthdate": "1999-09-22T10:47:24.285Z", "phoneNumber": "" }

//Test Scenario for DisplayName
{ "salutation": "Mr", "firstname": "TestFirstName2", "lastname": "TestLastName2", "email": "working2@email.com", "displayname": "", "birthdate": "1998-09-12T10:47:24.285Z", "phoneNumber": "02124669900" }