using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmclean.Application.Features.ContactFeature.Queries.GetContactByFilter
{
    public class GetContactByFilterQuery : IRequest<List<GetContactByFilterResponse>>
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string DisplayName { get; init; }

        public DateTime BirthDate { get; init; }
    }
}
