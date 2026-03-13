public interface IUserService
{
    void AddUser(User user);
    User? GetUserByEmail(string email);
    bool ValidateUser(string email, string password);
    string? GetLoggedInUser(string loggedInEmail);
}