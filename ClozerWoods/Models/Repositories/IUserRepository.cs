using ClozerWoods.Models.Entities;

namespace ClozerWoods.Models.Repositories;
public interface IUserRepository {
    IEnumerable<User> Users { get; }
    User GetUser(string email, byte[] password);
}
