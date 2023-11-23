using Npgsql;

namespace cmclean.MinimalApi.Extensions
{
    public static class InitializeDBExtension
    {
        public static IServiceCollection SetupTableAndSampleRecords(this IServiceCollection services, string ConnectionString)
        {
            /*
            Quotes are added around the tables because PostGreSql doesn't play nice with Entity Framework.
            Ef expects a table with uppercase and 's' on default. Postgresql doesn't like uppercases. I can use a package here
            which is shown on Npgsql side but that's not fun for solving the problem right ?
            */
            try
            {

                using var connection = new NpgsqlConnection
                (ConnectionString);
                connection.Open();

                using var command = new NpgsqlCommand
                {
                    Connection = connection
                };
                #region Table Creation
                command.CommandText = @"DROP TABLE IF EXISTS ""Contacts"" ";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE ""Contacts""(Id uuid, 
                                                                Salutation VARCHAR(5),
                                                                Firstname VARCHAR(24),
                                                                Lastname VARCHAR(24),
                                                                Displayname VARCHAR(50),
                                                                Birthdate timestamp,                                                    
                                                                CreationTimestamp timestamp,
                                                                LastChangeTimestamp timestamp,
                                                                Email VARCHAR(50) UNIQUE,
                                                                Phonenumber VARCHAR(24),
                                                                PRIMARY KEY (Id))";
                command.ExecuteNonQuery();
                Console.WriteLine("Table creation is succesful");
                #endregion
                
                #region Sample Records For Testing
                Guid TrialGuid = Guid.NewGuid();
                command.CommandText =
                @"INSERT INTO ""Contacts"""
                + "(Id, Salutation, Firstname, Lastname, Displayname, "
                + "Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber)"
                + $"VALUES ('{TrialGuid}','Mr', 'Jeffrey' , 'Donovan' ,'','1968-05-11T19:10:25',"
                + $"'{DateTime.Now}','{DateTime.Now}','trialrun1@email.com','02123445566')";
                command.ExecuteNonQuery();
                Console.WriteLine("First test user created");

                TrialGuid = Guid.NewGuid();
                command.CommandText =
                @"INSERT INTO ""Contacts"""
                + "(Id, Salutation, Firstname, Lastname, Displayname, "
                + "Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber)"
                + $"VALUES ('{TrialGuid}','Mr', 'Bruce' , 'Campbell' ,'','1958-06-22T19:10:25',"
                + $"'{DateTime.Now}', '{DateTime.Now}','trialrun2@email.com','02123558899')";
                command.ExecuteNonQuery();
                Console.WriteLine("Second test user created");

                command.CommandText =
               @"INSERT INTO ""Contacts"""
               + "(Id, Salutation, Firstname, Lastname, Displayname, "
               + "Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber)"
               + $"VALUES ('4b2056a9-7ee4-47b1-a64f-15770ceab7aa','Ms', 'Kimberly' , 'Director' ,'KimDirector','1974-11-13T19:10:25',"
               + $"'{DateTime.Now}', '{DateTime.Now}','trialrun3@email.com','02124669900')";
                command.ExecuteNonQuery();
                Console.WriteLine("Third test user created stricly for update user scenario");

                command.CommandText =
                 @"INSERT INTO ""Contacts"""
                 + "(Id, Salutation, Firstname, Lastname, Displayname, "
                 + "Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber)"
                 + $"VALUES ('104142c0-7248-48aa-b230-5798810adf58','Ms', 'Evelyn' , 'Hampshire' ,'EveH','{DateTime.Now.AddDays(12)}',"
                 + $"'{DateTime.Now}', '{DateTime.Now}','trialrun4@email.com','02124669901')";
                command.ExecuteNonQuery();
                Console.WriteLine("Fourth test user created to see for Birthday under the check range scenario");

                command.CommandText =
                @"INSERT INTO ""Contacts"""
                + "(Id, Salutation, Firstname, Lastname, Displayname, "
                + "Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber)"
                + $"VALUES ('15423c8b-6f3d-4848-868a-ff10e2835e60','Mr', 'Matthew' , 'Lillard' ,'MShagl','{DateTime.Now.AddDays(15)}',"
                + $"'{DateTime.Now}', '{DateTime.Now}','trialrun5@email.com','02124669902')";
                command.ExecuteNonQuery();
                Console.WriteLine("Fifth test user created to see for Birthday over the check range scenario");

                #endregion

            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Database connection or table creation failed" + ex.Message);
            }

            return services;
        }
    }
}
