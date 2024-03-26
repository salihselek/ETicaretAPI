using MediatR;

namespace ETicaretAPI.Application.Features.Queries.AuthorizationEndpoint.GetRolesByEndpoint
{
    public class GetRolesByEndpointQueryRequest : IRequest<GetRolesByEndpointQueryResponse>
    {
        public string Code { get; set; }
        public string Menu { get; set; }
    }
}