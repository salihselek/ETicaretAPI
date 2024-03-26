using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Role.UpdateRole
{
    public class UpdateRoleCommandRequest : IRequest<UpdateRoleCommandRespone>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
