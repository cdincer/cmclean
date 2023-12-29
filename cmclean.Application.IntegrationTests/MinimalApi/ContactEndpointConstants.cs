using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.Application.IntegrationTests.MinimalApi
{
    public static class ContactEndpointConstants
    {
        public const string BaseEndpoint = "http://localhost:8001/";
        public const string ContactEndpoint = "api/contacts/";
        public const string DataNode = "data";
        public const string IdNode = "id";
        public const string FirstNameNode ="firstName";
        public const string LastNameNode ="lastName";
        public const string DisplayNameNode = "displayName";
        public const string SuccessNode = "success";
        public const string MessageNode = "message";
    }
}
