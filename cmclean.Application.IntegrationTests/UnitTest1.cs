using Docker.DotNet;
using Docker.DotNet.Models;

namespace cmclean.Application.IntegrationTests
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            using (var conf = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"))) // localhost
            using (var client = conf.CreateClient())
            {

                const string ContainerName = "GitLabTests";
                const string ImageName = "gitlab/gitlab-ee";
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
                         { "80/tcp", new List<PortBinding> 
                            { 
                             new PortBinding { HostIP = "127.0.0.1", HostPort = "8080" }
                             } 
                         } 
                        }
                    };

                    // Create the container
                    var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters(config)
                    {
                        Image = ImageName + ":" + ImageTag,
                        Name = ContainerName,
                        Tty = false,
                        HostConfig = hostConfig,
                    });

                    // Get the container object
                    containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
                    container = containers.First(c => c.ID == response.ID);

                    if (container.State != "running")
                    {
                        var started = await client.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
                        if (!started)
                        {
                            Assert.Fail("Cannot start the docker container");
                        }
                    }

                    using (var httpClient = new HttpClient())
                    {
                        while (true)
                        {
                            try
                            {
                                using (var response2 = await httpClient.GetAsync("http://localhost:8080"))
                                {
                                    if (response2.IsSuccessStatusCode)
                                        break;
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Couldn't reach gitlab page for health check.");
                            }
                        }
                    }
                }

            }
        }

        [Fact]
        public async void Test2()
        {
            using (var conf = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"))) // localhost
            using (var client = conf.CreateClient())
            {

                const string ContainerName = "IntegrationTestDB";
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

                    // Create the container
                    var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters(config)
                    {
                        Image = ImageName + ":" + ImageTag,
                        Name = ContainerName,
                        Tty = false,
                        HostConfig = hostConfig,
                        Env = new List<string> { "POSTGRES_USER = admin", "POSTGRES_PASSWORD=admin1234", "POSTGRES_DB=Contactmanagerdb" }
                    });                         
                                                
                    // Get the container object
                    containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
                    container = containers.First(c => c.ID == response.ID);

                    if (container.State != "running")
                    {
                        var started = await client.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
                        if (!started)
                        {
                            Assert.Fail("Cannot start the docker container");
                        }
                    }

                    //using (var httpClient = new HttpClient())
                    //{
                    //    while (true)
                    //    {
                    //        try
                    //        {
                    //            using (var response2 = await httpClient.GetAsync("http://localhost:5432"))
                    //            {
                    //                if (response2.IsSuccessStatusCode)
                    //                    break;
                    //            }
                    //        }
                    //        catch
                    //        {
                    //            Console.WriteLine("Couldn't reach gitlab page for health check.");
                    //        }
                    //    }
                    //}
                }

            }
        }


    }
}