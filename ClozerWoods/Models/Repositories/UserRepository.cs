using ClozerWoods.Models.Entities;

namespace ClozerWoods.Models.Repositories;
public class UserRepository : IUserRepository {
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) {
        _context = context;
    }

    public IEnumerable<User> Users => _context.Users;

    public User GetUser(string email, byte[] password) {
        User? user = null;
        if(Users != null) {
            user = Users.FirstOrDefault(x => x.Email == email && x.Password.SequenceEqual(password));
        }
        if(user == null) {
            throw new UserNotFoundException();
        }

        return user;
    }
}

public class UserNotFoundException : Exception { }
