public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private Eftelinked<User> _users;

    public UserService(IUserRepository repository)
    {
        _repository = repository;

        _users = _repository.LoadUsers();
    }

    public void AddUser(User user)
    {
        user.Id = _users.Count > 0 ? _users.Max((a, b) => a.Id.CompareTo(b.Id)).Id + 1 : 1;

        _users.Add(user);
        _repository.SaveUsers(_users);
    }

    public User GetUserByEmail(string email)
    {
        Eftelinked<User> users = (Eftelinked<User>)_users.Filter(u => u.Email == email);
        return users.head!.Data;
    }

    public bool ValidateUser(string email, string password)
    {
        var user = GetUserByEmail(email);

        if (user == null)
            return false;

        return user.Password == password;
    }
}