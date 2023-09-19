# cmclean
Contact Manager made in Clean Architecture.
Made with Visual Studio Code.
Steps for setting it up.</br>
1-Use cmcleanWorkspace.code-workspace to open up the whole project in a single workspace.</br>
2-Use docker-compose.yml to start up the required PostgresqlDB.</br>
3-As you start cmclean.API, commands in InitializeDB.cs which is in Helpers folder</br>
will replace any tables existed insert sample records for you to look around with.</br>
4-Additional scenarios for testing stuff is below.</br>

# Random testing scenarios

//Third user is at the start up with a set guid for update/get user functions testing if you choose to do so //it's guid is :4b2056a9-7ee4-47b1-a64f-15770ceab7aa

//Test Scenario for Birthday
{ "salutation": "Mrs", "firstname": "TestFirstName1", "lastname": "TestLastName1", "email": "working@email.com", "displayname": "TestingDisplayName1", "birthdate": "1999-09-22T10:47:24.285Z", "phoneNumber": "" }

//Test Scenario for DisplayName
{ "salutation": "Mr", "firstname": "TestFirstName2", "lastname": "TestLastName2", "email": "working2@email.com", "displayname": "", "birthdate": "1998-09-12T10:47:24.285Z", "phoneNumber": "02124669900" }