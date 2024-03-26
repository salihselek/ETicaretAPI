using ETicaretAPI.Application.DTOs.Role;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IRoleService
    {
        Task<(object, int)> GetAllRoles(int page, int size);
        Task<(string id, string name)> GetByIdAsync(string id);
        Task<bool> CreateRoleAsync(string name);
        Task<bool> DeleteRoleAsync(string id);
        Task<bool> UpdateRoleAsync(string id, string name);
    }
}