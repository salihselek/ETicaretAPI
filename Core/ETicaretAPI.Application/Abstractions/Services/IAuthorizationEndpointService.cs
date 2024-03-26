namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IAuthorizationEndpointService
    {
        public Task AssignRoleEndpointAsync(string[] roles, string menu, string code, Type type);
        public Task<List<string>> GetRolesByEndpointAsync(string code,string menu);
    }
}