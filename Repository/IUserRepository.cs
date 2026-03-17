public interface IUserRepository
{
    Efteldingen<User> LoadUsers();
    void SaveUsers(Efteldingen<User> users);
}