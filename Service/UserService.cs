public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private Eftelinked<User> _users;

    public UserService(IUserRepository repository)
    {
        _repository = repository;

        _users = (Eftelinked<User>)_repository.LoadUsers();
    }

    public void AddUser(User user)
    {
        user.Id = _users.Count > 0 ? _users.Max((a, b) => a.Id.CompareTo(b.Id)).Id + 1 : 1;

        _users.Add(user);
        _repository.SaveUsers(_users);
    }

    public User GetUserByEmail(string email)
    {
        Efteldingen<User> users = (Efteldingen<User>)_users.Filter(u => u.Email == email);
        return users[0];
    }

    public bool ValidateUser(string email, string password)
    {
        var user = GetUserByEmail(email);

        if (user == null)
            return false;

        return user.Password == password;
    }
}