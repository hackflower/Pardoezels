using System.Text.Json;

public class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    public JsonUserRepository(string filePath) => _filePath = filePath;
    public Eftelinked<User> LoadUsers()
    {
        if (!File.Exists(_filePath))
        {
            return new Eftelinked<User>();
        }
        string json = File.ReadAllText(_filePath);
        Eftelinked<User> users = JsonSerializer.Deserialize<Eftelinked<User>>(json) ?? new Eftelinked<User>();

        return users;
    }

    public void SaveUsers(Eftelinked<User> users)
    {
        string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}