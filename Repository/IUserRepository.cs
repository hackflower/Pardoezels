public interface IUserRepository
{
    List<User> LoadUsers();
    void SaveUsers(List<User> users);
}