using Docker.DotNet.Models;
using Docker.DotNet;
using Npgsql;
using Xunit;

namespace cmclean.Application.IntegrationTests
{
    public class ContactsReadFixture : IAsyncLifetime
    {

        const string ContainerName = "IntegrationTestDB";

        public ContactsReadFixture()
        {
            
        }

        public async Task InitializeAsync()
        {
            await SetUpDockerContainer();
            await SetupDatabaseAndSampleRecords();
        }

        public async Task DisposeAsync()
        {
            await RemoveDockerContainer();
        }

        public async Task<int> SetUpDockerContainer()
        {


            using (var conf = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"))) // localhost
            using (var client = conf.CreateClient())
            {

                const string ImageName = "postgres";
                const string ImageTag = "latest";

                var containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
                var container = containers.FirstOrDefault(c => c.Names.Contains("/" + ContainerName));
                if (container == null)
                {
                    // Download image
                    await client.Images.CreateImageAsync(new ImagesCreateParameters() { FromImage = ImageName, Tag = ImageTag }, new AuthConfig(), new Progress<JSONMessage>());

                    // Create the container
                    var config = new Config()
                    {
                        Hostname = "localhost"
                    };

                    // Configure the ports to expose
                    var hostConfig = new HostConfig()
                    {
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                        {
                         { "5432/tcp", new List<PortBinding>
                            {
                             new PortBinding { HostIP = "127.0.0.1", HostPort = "5432" }
                             }
                         }
                        }
                    };
                    HealthConfig PostGresHealthConfig = new();
                    PostGresHealthConfig.Test = new List<string> { "CMD-SHELL", "pg_isready", "-d", "db_prod" };
                    PostGresHealthConfig.Interval = TimeSpan.FromSeconds(30);
                    PostGresHealthConfig.Timeout = TimeSpan.FromSeconds(60);
                    PostGresHealthConfig.Retries = 5;
                    PostGresHealthConfig.StartPeriod = 300000000000;
                    // Create the container

                    var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters(config)
                    {
                        Image = ImageName + ":" + ImageTag,
                        Name = ContainerName,
                        Tty = false,
                        HostConfig = hostConfig,
                        Env = new List<string> { "POSTGRES_USER=admin", "POSTGRES_PASSWORD=admin1234", "POSTGRES_DB=Contactmanagerdb" },
                        Healthcheck = PostGresHealthConfig
                    });

                    // Get the container object
                    containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
                    container = containers.First(c => c.ID == response.ID);

                    if (container.State != "running")
                    {
                        var started = await client.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
                        if (!started)
                        {
                            Console.WriteLine("Not started");
                        }
                    }
                }
                return 1;
            }
        }

        public async Task<int> SetupDatabaseAndSampleRecords()
        {
            try
            {
                string ConnectionString = "Server=127.0.0.1;Port=5432;Database=Contactmanagerdb;User Id=admin;Password=admin1234;Timeout=50";
                await using var connection = new NpgsqlConnection
                (ConnectionString);
                connection.Open();

                await using var command = new NpgsqlCommand
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
                command.CommandText = @"SELECT Id, Salutation, Firstname, Lastname, Displayname, Birthdate, CreationTimestamp, LastChangeTimestamp, Email, Phonenumber FROM  ""Contacts""";
                #endregion

                return 1;
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Database connection or table creation failed" + ex.Message);
                return 0;
            }

        }

        public async static Task RemoveDockerContainer()
        {
            using (var conf = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"))) // localhost
            using (var client = conf.CreateClient())
            {
                var containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
                var container = containers.FirstOrDefault(c => c.Names.Contains("/" + ContainerName));
                await client.Containers.StopContainerAsync(container.ID, new ContainerStopParameters
                {
                    WaitBeforeKillSeconds = 3
                });
                 client.Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters()).Wait();
            }
        }

  
    }
}
