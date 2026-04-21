using System.Text.Json;

public class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    private readonly IMyCollection<User> _users;

    public JsonUserRepository(string filePath, IMyCollection<User> users)
    {
        _filePath = filePath;
        _users = users;
    }

    public IMyCollection<User> LoadUsers()
    {
        if (!File.Exists(_filePath))
        {
            return _users;
        }
        string json = File.ReadAllText(_filePath);
        User[] array = JsonSerializer.Deserialize<User[]>(json) ?? Array.Empty<User>();
        IMyCollection<User> users = _users.FromArray(array);

        return users;
    }

    public void SaveUsers(IMyCollection<User> users)
    {
        string json = JsonSerializer.Serialize(users.ToArray(), new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}