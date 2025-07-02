using Core.Models.Identity;

namespace Core.Contracts.Identity
{
    public interface IUserService
    {
        public string UserId { get; }
        public string SolId { get; }
        public string Name { get; }
    }
}