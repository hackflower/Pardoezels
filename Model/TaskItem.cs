public class TaskItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public required string Description { get; set; }
    public bool Completed { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Priority { get; set; }
    public override string ToString()
    {
        return $"{Id}. {Name} \n{Description} \n{(Completed ? "Completed" : "Not Completed")}\n";
    }
}
