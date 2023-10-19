namespace Project_management.objects;

public class Task
{
    public int Id { get; set; } 
    public string Number { get; set; }
    public string Title { get; set; }
    public int Duration { get; set; }
    public Task Parent { get; set; }
    public Project Project = null;

    public Task(string number, string title, int duration, Task parent)
    {
        this.Number = number;
        this.Title = title;
        this.Duration = duration;
        this.Parent = parent;
    }
}