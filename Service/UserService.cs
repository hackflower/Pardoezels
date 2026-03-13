public class UserService : IUserService
{
    private List<User> _users;
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;

        _users = _repository.LoadUsers() ?? new List<User>();
    }

    public void AddUser(User user)
    {
        user.Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;

        _users.Add(user);
        _repository.SaveUsers(_users);
    }

    public User? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public bool ValidateUser(string email, string password)
    {
        var user = GetUserByEmail(email);

        if (user == null)
            return false;

        return user.Password == password;
    }

    public string? GetLoggedInUser(string loggedInEmail)
    {
        return _users.FirstOrDefault(u => u.Email == loggedInEmail)?.Username;
    }
}