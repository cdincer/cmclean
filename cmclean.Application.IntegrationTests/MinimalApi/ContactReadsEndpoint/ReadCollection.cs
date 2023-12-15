using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.Application.IntegrationTests.MinimalApi.ContactReadsEndpoint
{
    [CollectionDefinition("Read Collection")]
    public class ReadCollection : ICollectionFixture<ContactsReadFixture>
    {
    }
}
