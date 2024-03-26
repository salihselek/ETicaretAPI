using MediatR;

namespace ETicaretAPI.Application.Features.Queries.AppUser.GetRolesByUser
{
    public class GetRolesByUserQueryRequest : IRequest<GetRolesByUserQueryResponse>
    {
        public string UserId { get; set; }
    }
}