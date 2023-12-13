using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Commands;

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
            await FullContainer();
        }

        public async Task DisposeAsync()
        {
            await RemoveDockerContainer();
        }


        public async Task FullContainer()
        {
            DirectoryInfo StartingPoint = new DirectoryInfo(Directory.GetCurrentDirectory());
            DirectoryInfo MainFolderPath = StartingPoint.Parent.Parent.Parent.Parent;
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
                _docker.Host.RemoveContainer(containerElement.Id,true,true,null);
            }

        }
    }
}
