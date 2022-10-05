namespace Assignment3.Entities;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public ICollection<Task> Tasks { get; set; } = null!;
}
