using System.Text.Json;

public class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    public JsonUserRepository(string filePath) => _filePath = filePath;

    public List<User> LoadUsers()
    {
        if (!File.Exists(_filePath))
        {
            return new List<User>();
        }
        string json = File.ReadAllText(_filePath);
        var array = JsonSerializer.Deserialize<User[]>(json) ?? Array.Empty<User>();
        return array.ToList();
    }

    public void SaveUsers(List<User> users)
    {
        string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}