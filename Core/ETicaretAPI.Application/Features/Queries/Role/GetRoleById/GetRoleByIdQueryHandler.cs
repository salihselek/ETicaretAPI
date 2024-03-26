using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Role.GetRoleById
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQueryRequest, GetRoleByIdQueryResppnse>
    {
        readonly IRoleService _roleService;

        public GetRoleByIdQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetRoleByIdQueryResppnse> Handle(GetRoleByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _roleService.GetByIdAsync(request.Id);
            return new()
            {
                Id = result.id,
                Name = result.name
            };
        }
    }
}