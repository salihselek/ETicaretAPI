using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.AppUser.GetRolesByUser
{
    public class GetRolesByUserQueryHandler : IRequestHandler<GetRolesByUserQueryRequest, GetRolesByUserQueryResponse>
    {
        readonly IUserService _userService;

        public GetRolesByUserQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetRolesByUserQueryResponse> Handle(GetRolesByUserQueryRequest request, CancellationToken cancellationToken)
        {
            var userRoles = await _userService.GetRolesByUser(request.UserId);
            return new()
            {
                UserRoles = userRoles
            };
        }
    }
}