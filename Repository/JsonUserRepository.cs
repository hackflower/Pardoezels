using System.Text.Json;

public class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    public JsonUserRepository(string filePath) => _filePath = filePath;
    public Efteldingen<User> LoadUsers()
    {
        if (!File.Exists(_filePath))
        {
            return new Efteldingen<User>();
        }
        string json = File.ReadAllText(_filePath);
        var array = JsonSerializer.Deserialize<User[]>(json) ?? Array.Empty<User>();
        
        Efteldingen<User> users = new Efteldingen<User>();

        foreach (User user in array)
        {
            users.Add(user);
        }

        return users;
    }

    public void SaveUsers(Efteldingen<User> users)
    {
        string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}