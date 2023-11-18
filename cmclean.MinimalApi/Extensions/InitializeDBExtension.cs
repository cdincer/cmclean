﻿using Npgsql;

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
                                                                Firstname VARCHAR(24) NOT NULL,
                                                                Lastname VARCHAR(24) NOT NULL,
                                                                Displayname VARCHAR(24),                                                           
                                                                Birthdate timestamp,                                                                                                       
                                                                PRIMARY KEY (Id))";
                command.ExecuteNonQuery();
                Console.WriteLine("Table creation is succesful");
                #endregion
                /*
                #region Triggers
                //Time adjusted in 2 triggers for my time zone.
                command.CommandText = @"CREATE OR REPLACE FUNCTION change_creationtimestamp() RETURNS trigger AS $change_creationtimestamp$
                BEGIN
                    NEW.creationtimestamp := NOW() + interval '3 hours';
                    NEW.lastchangetimestamp := NOW() + interval '3 hours';
                    RETURN NEW;
                END;
                $change_creationtimestamp$ LANGUAGE plpgsql;";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE OR REPLACE TRIGGER change_creationtimestamp BEFORE INSERT ON ""Contacts""
                FOR EACH ROW EXECUTE FUNCTION change_creationtimestamp();";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE OR REPLACE FUNCTION change_lastchangetimestamp() RETURNS trigger AS $change_lastchangetimestamp$
                 BEGIN
                -- Remember who changed the payroll when
                NEW.lastchangetimestamp := NOW() + interval '3 hours';
                RETURN NEW;
                END;
                $change_lastchangetimestamp$ LANGUAGE plpgsql; ";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE OR REPLACE TRIGGER change_lastchangetimestamp BEFORE UPDATE ON ""Contacts""
                                        FOR EACH ROW EXECUTE FUNCTION change_lastchangetimestamp();";
                command.ExecuteNonQuery();
                Console.WriteLine("Trigger creation succesful");
                #endregion
                #region Sample Records For Testing
                Guid TrialGuid = Guid.NewGuid();
                command.CommandText =
                @"INSERT INTO ""Contacts"""
                + "(Id, Salutation, Firstname, Lastname, Displayname, "
                + "Birthdate, CreationTimestamp, LastChangeTimestamp, NotifyHasBirthdaySoon, Email, Phonenumber)"
                + $"VALUES ('{TrialGuid}','Mr', 'Can' , 'Dincer' ,'','2016-09-12T19:10:25',"
                + $"'{DateTime.Now}','{DateTime.Now}', true,'trialrun1@email.com','02123445566')";
                command.ExecuteNonQuery();
                Console.WriteLine("First test user created");

                TrialGuid = Guid.NewGuid();
                command.CommandText =
                @"INSERT INTO ""Contacts"""
                + "(Id, Salutation, Firstname, Lastname, Displayname, "
                + "Birthdate, CreationTimestamp, LastChangeTimestamp, NotifyHasBirthdaySoon, Email, Phonenumber)"
                + $"VALUES ('{TrialGuid}','Mr', 'Cem' , 'Dicer' ,'','2016-09-16T19:10:25',"
                + $"'{DateTime.Now}', '{DateTime.Now}', true,'trialrun2@email.com','02123558899')";
                command.ExecuteNonQuery();
                Console.WriteLine("Second test user created");

                command.CommandText =
               @"INSERT INTO ""Contacts"""
               + "(Id, Salutation, Firstname, Lastname, Displayname, "
               + "Birthdate, CreationTimestamp, LastChangeTimestamp, NotifyHasBirthdaySoon, Email, Phonenumber)"
               + $"VALUES ('4b2056a9-7ee4-47b1-a64f-15770ceab7aa','Ms', 'Kimberly' , 'Director' ,'KimDirector','1974-11-13T19:10:25',"
               + $"'{DateTime.Now}', '{DateTime.Now}', true,'trialrun3@email.com','02124669900')";
                command.ExecuteNonQuery();
                Console.WriteLine("Third test user created / stricly for update user scenario");
                #endregion
              */
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Database connection or table creation failed" + ex.Message);
            }

            return services;
        }
    }
}