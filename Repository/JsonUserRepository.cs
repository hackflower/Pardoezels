using System.Text.Json;

public class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    public JsonUserRepository(string filePath) => _filePath = filePath;
    public IMyCollection<User> LoadUsers()
    {
        if (!File.Exists(_filePath))
        {
            return new Efteldingen<User>();
        }
        string json = File.ReadAllText(_filePath);
        IMyCollection<User> users = JsonSerializer.Deserialize<IMyCollection<User>>(json) ?? new Efteldingen<User>();

        return users;
    }

    public void SaveUsers(IMyCollection<User> users)
    {
        string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}