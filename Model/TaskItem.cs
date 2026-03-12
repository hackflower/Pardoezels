using System.ComponentModel;
using System.Reflection;

public class TaskItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public required string Description { get; set; }

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