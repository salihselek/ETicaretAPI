using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Role.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommandRequest, UpdateRoleCommandRespone>
    {
        readonly IRoleService _roleService;

        public UpdateRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<UpdateRoleCommandRespone> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _roleService.UpdateRoleAsync(request.Id, request.Name);
            return new()
            {
                Succeeded = result
            };
        }
    }
}
