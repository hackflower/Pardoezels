using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;

public class TaskItem
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    [JsonIgnore]
    public required Efteldingen<int> AssignedUsersIds { get; set; } = [];

    [JsonIgnore]
    public Efteldingen<int> DependenciesIds { get; set; } = [];

    [JsonPropertyName("Dependencies")]
    public int[] DependenciesArray
    {
        get => DependenciesIds.ToArray();
    }

    [JsonPropertyName("AssignedUsers")]
    public int[] AssignedUsersArray
    {
        get => AssignedUsersIds.ToArray();
    }

    public enum Progress
    {
        [Description("Not Started")]
        NotStarted = 0,
        [Description("In Progress")]
        InProgress = 1,
        [Description("Completed")]
        Completed = 2
    }

    public Progress Status { get; set; } = Progress.NotStarted;

    public DateTime CreatedAt { get; set; }

    public enum Importance
    {
        [Description("Low")]
        Low = 0,
        [Description("Normal")]
        Normal = 1,
        [Description("High")]
        High = 2
    }

    public Importance Priority { get; set; }

    public override string ToString()
    {
        return $"ID: {Id} | Name: {Name} | Description: {Description} | Status: {Status.GetDescription()} | Priority: {Priority}";
    }
}

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        DescriptionAttribute? attr = field?.GetCustomAttribute<DescriptionAttribute>();
        return attr != null ? attr.Description : value.ToString();
    }
}