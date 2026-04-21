public interface IUserService
{
    void AddUser(User user);
    User? GetUserByEmail(string email);
    User? GetUserById(int id);
    bool ValidateUser(string email, string password);
    bool UserExists(string email);
    bool EmailValid(string email);
    bool UserValid(User user);
    IMyCollection<User> GetAllUsers();
}