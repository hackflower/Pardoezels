public class TaskItem
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public bool Completed { get; set; }

    public override string ToString()
    {
        return $"{Id}.\nTask Description: {Description}\nCompleted: {Completed}\n";
    }
}
