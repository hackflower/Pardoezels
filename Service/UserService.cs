using System.Text.RegularExpressions;

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

    public User? GetUserByEmail(string email)
    {
        var result = _users.FindBy(email, (user, key) => user.Email == key ? 0 : -1);

        return result.HasValue ? result.Value : null;
    }
    
    public User? GetUserById(int id)
    {
        var result = _users.FindBy(id, (user, key) => user.Id == key ? 0 : -1);

        return result.HasValue ? result.Value : null;
    }

    public bool ValidateUser(string email, string password)
    {
        var user = GetUserByEmail(email);

        if (user == null)
            return false;

        return user.Password == password;
    }

    public bool UserExists(string email)
    {
        var user = GetUserByEmail(email);
        return user != null;
    }

    public bool EmailValid(string email)
    {
        var emailPattern = "^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    public bool UserValid(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            return false;

        if (!EmailValid(user.Email))
            return false;

        if (UserExists(user.Email))
            return false;

        return true;
    }

    public Eftelinked<User> GetAllUsers()
    {
        return _users;
    }
}