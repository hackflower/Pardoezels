using System;
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
    public string? Priority { get; set; }

    public override string ToString()
    {
        return $"{Id}. {Name}\nDescription: {Description}\nStatus: {Status.GetDescription()}\nPriority: {Priority}";
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