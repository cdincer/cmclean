using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Commands;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace cmclean.Application.IntegrationTests.MinimalApi
{
    public class ContactsEndpointsFixture : IAsyncLifetime
    {
        public ContactsEndpointsFixture()
        {

        }

        public async Task InitializeAsync()
        {
            await FullContainer();
            await DelayExecution();
        }

        public async Task DisposeAsync()
        {
            await RemoveDockerContainer();
        }

        public async Task FullContainer()
        {
            DirectoryInfo StartingPoint = new DirectoryInfo(Directory.GetCurrentDirectory());
            DirectoryInfo MainFolderPath = StartingPoint.Parent.Parent.Parent.Parent;//Local folder on developers machine,adjust accordingly
            var mainyml = Path.Combine(MainFolderPath.ToString(), (TemplateString)"docker-compose/docker-compose.yml");
            var overrideyml = Path.Combine(MainFolderPath.ToString(), (TemplateString)"docker-compose/docker-compose.override.yml");

            var container =
                  new Builder().UseContainer().UseCompose().FromFile(overrideyml).FromFile(mainyml).Build().Start();
        }

        public async static Task RemoveDockerContainer()
        {
            var hosts = new Hosts().Discover();
            var _docker = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default");
            var response = _docker.Host.InspectContainers();
            ;
            foreach (var containerElement in response.Data)
            {
                _docker.Host.RemoveContainer(containerElement.Id, true, true, null);
            }

        }
        //Giving some time for our database to be setup
        public static async Task DelayExecution()
        {

            string ConnectionString = ContactEndpointConstants.IntegrationTestDBConnection;

            var connectionEstablised = false;
            var start = DateTime.UtcNow;
            int maxWaitTimeSeconds = 20;
            while (!connectionEstablised && start.AddSeconds(maxWaitTimeSeconds) > DateTime.UtcNow)
            {
                try
                {
                    string ConnectionStringT = ConnectionString;
                    await using var connectionT = new NpgsqlConnection
                    (ConnectionStringT);
                    await connectionT.OpenAsync();
                    connectionEstablised = true;
                }
                catch
                {
                    // If opening the npgSQL connection fails, database is not ready yet
                    await Task.Delay(500);
                }
            }
        }
    


    //Same as MinimalAPI project's InitializeDBExtension. In a real life project it's supposed to be here, it's not redundant.
    //Because of this project's purpose we focus on the MinimalAPI's execution and turn this off for the time being.
    public static async Task SetupTableAndSampleRecords()
    {

        string ConnectionString = ContactEndpointConstants.IntegrationTestDBConnection;


        try
        {
            var connectionEstablised = false;
            var start = DateTime.UtcNow;
            int maxWaitTimeSeconds = 20;
            while (!connectionEstablised && start.AddSeconds(maxWaitTimeSeconds) > DateTime.UtcNow)
            {
                try
                {
                    string ConnectionStringT = ConnectionString;
                    await using var connectionT = new NpgsqlConnection
                    (ConnectionStringT);
                    await connectionT.OpenAsync();
                    connectionEstablised = true;
                }
                catch
                {
                    // If opening the npgSQL connection fails, SQL Server is not ready yet
                    await Task.Delay(500);
                }
            }


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
            + $"'{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}','{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}','trialrun1@email.com','02123445566')";
            command.ExecuteNonQuery();
            Console.WriteLine("First test user created");

            TrialGuid = Guid.NewGuid();
            command.CommandText =
            @"INSERT INTO ""Contacts"""
            + "(Id, Salutation, Firstname, Lastname, Displayname, "
            + "Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber)"
            + $"VALUES ('{TrialGuid}','Mr', 'Bruce' , 'Campbell' ,'','1958-06-22T19:10:25',"
            + $"'{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}', '{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}','trialrun2@email.com','02123558899')";
            command.ExecuteNonQuery();
            Console.WriteLine("Second test user created");

            command.CommandText =
           @"INSERT INTO ""Contacts"""
           + "(Id, Salutation, Firstname, Lastname, Displayname, "
           + "Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber)"
           + $"VALUES ('4b2056a9-7ee4-47b1-a64f-15770ceab7aa','Ms', 'Kimberly' , 'Director' ,'KimDirector','1974-11-13T19:10:25',"
           + $"'{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}', '{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}','trialrun3@email.com','02124669900')";
            command.ExecuteNonQuery();
            Console.WriteLine("Third test user created stricly for update user scenario");

            command.CommandText =
             @"INSERT INTO ""Contacts"""
             + "(Id, Salutation, Firstname, Lastname, Displayname, "
             + "Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber)"
             + $"VALUES ('104142c0-7248-48aa-b230-5798810adf58','Ms', 'Evelyn' , 'Hampshire' ,'EveH','{DateTime.Now.AddDays(12).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}',"
             + $"'{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}', '{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}','trialrun4@email.com','02124669901')";
            command.ExecuteNonQuery();
            Console.WriteLine("Fourth test user created to see for Birthday under the check range scenario");

            command.CommandText =
            @"INSERT INTO ""Contacts"""
            + "(Id, Salutation, Firstname, Lastname, Displayname, "
            + "Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber)"
            + $"VALUES ('15423c8b-6f3d-4848-868a-ff10e2835e60','Mr', 'Matthew' , 'Lillard' ,'MShagl','{DateTime.Now.AddDays(15).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}',"
            + $"'{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}', '{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")}','trialrun5@email.com','02124669902')";
            command.ExecuteNonQuery();
            Console.WriteLine("Fifth test user created to see for Birthday over the check range scenario");

            #endregion

        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine("Database connection or table creation failed" + ex.Message);
        }

    }

}
}