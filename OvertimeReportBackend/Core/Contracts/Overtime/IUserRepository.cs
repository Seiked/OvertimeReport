using Core.Models.Overtime;

namespace Core.Contracts.Overtime
{
    public interface IUserRepository
    {
        public Task<List<UserT>> GetAllUser();
    }
}