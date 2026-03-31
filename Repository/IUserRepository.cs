public interface IUserRepository
{
    Eftelinked<User> LoadUsers();
    void SaveUsers(Eftelinked<User> users);
}