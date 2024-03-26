using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.AuthorizationEndpoint.GetRolesByEndpoint
{
    public class GetRolesByEndpointQueryHandler : IRequestHandler<GetRolesByEndpointQueryRequest, GetRolesByEndpointQueryResponse>
    {
        readonly IAuthorizationEndpointService _authorizationEndpointService;

        public GetRolesByEndpointQueryHandler(IAuthorizationEndpointService authorizationEndpointService)
        {
            _authorizationEndpointService = authorizationEndpointService;
        }

        public async Task<GetRolesByEndpointQueryResponse> Handle(GetRolesByEndpointQueryRequest request, CancellationToken cancellationToken)
        {
            var datas = await _authorizationEndpointService.GetRolesByEndpointAsync(request.Code, request.Menu);
            return new() { Roles = datas };
        }
    }
}