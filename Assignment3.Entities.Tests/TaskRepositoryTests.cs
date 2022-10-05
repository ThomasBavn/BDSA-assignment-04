
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly KanbanContext _context;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly TaskRepository _repository;

    public TaskRepositoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();

        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        optionsBuilder.UseSqlite(_connection);
        var context = new KanbanContext(optionsBuilder.Options);
        context.Database.EnsureCreated();





        context.Tasks.AddRange(GetTasks());

        context.SaveChanges();
        _repository = new TaskRepository(context);
        _context = context;
    }

    [Fact]
    public void Create_Success()
    {
        //Assign
        var task = new TaskCreateDTO("new Task", null, "this is a brand new task!", new[] { "ASAP", "Whenever" });

        // Act
        var r1 = _repository.Create(task);

        // Assert
        Assert.Equal((Response.Created, 4), r1);
    }

    [Fact]
    public void Create_Title_Too_Long()
    {

        var tags = GetTags();
        var r2 = _repository.Create(new TaskCreateDTO(
            "dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd",
            null, "man, i cant wait to do this task", new[] { tags[0].Name, tags[1].Name }.ToList()));
        Assert.Equal((Response.BadRequest, 0), r2);
    }

    [Fact]
    public void Update_UpdatesStateOfTask()
    {
        var tags = GetTags();
        //Assign;
        var task = new TaskUpdateDTO
        (
            2,
            "updated title",
            null,
            "Very interesting description",
            new[] { tags[0].Name, tags[1].Name }.ToList(),
            State.Active
        );
        var actual = _repository.Update(task);
        Assert.Equal(Response.Updated, actual);

    }

    [Fact]
    public void Update_ConfirmUpdate()
    {
        //Assign;
        // var task = new TaskUpdateDTO
        // (
        //     2,
        //     "updated title",
        //     null,
        //     "Very interesting description",
        //     new[] { "ASAP", "Whenever" },
        //     State.Active
        // );
        // _repository.Update(task);

        // //confirm update has been made
        // var task2 = _repository.Read(2);

        // Assert.NotNull(task2);
        // Assert.Equal(2, task2.Id);
        // Assert.Equal("updated title", task2.Title);
        // Assert.Equal(State.Active, task2.State);
    }

    [Fact]
    public void Delete_RemovesTaskEntity()
    {
        // var result = _repository.Delete(1);


        // var (_, t1) = _repository.Create(new TaskCreateDTO("cool title", null, "man, i cant wait to do this task", tags));
        // Assert.Single(_repository.ReadAll());
        // _repository.Delete(2);
        // Assert.Empty(_repository.ReadAll());
    }

    [Fact]
    public void ReadAllByTag_ReturnsOne()
    {
        var asap = _repository.ReadAllByTag("ASAP");
        Assert.Single(asap);
    }

    [Fact]
    public void Read_ReturnsSpecifiedID()
    {
        var tags = GetTags();
        var r = _repository.Read(2);

        Assert.Equal(2, r.Id);
        Assert.Equal("Task2", r.Title);
        // Assert.Single(r.Tags);
        Assert.Equal(new[] { tags[2].Name, tags[3].Name }, r.Tags);
    }


    public Tag[] GetTags()
    {

        var tag1 = new Tag { Id = 1, Name = "ASAP" };
        var tag2 = new Tag { Id = 2, Name = "Whenever" };
        var tag3 = new Tag { Id = 3, Name = "Important" };
        var tag4 = new Tag { Id = 4, Name = "Inspiration" };

        return new[] { tag1, tag2, tag3, tag4 };
    }
    public Task[] GetTasks()
    {

        var tags = GetTags();
        var user = GetUser();
        var task1 = new Task();
        task1.Id = 1;
        task1.Title = "Cool title";
        task1.Description = "Very interesting description";
        task1.State = State.Active;
        task1.Tags = new List<Tag>() { tags[0], tags[1] };
        task1.AssignedTo = user;

        var task2 = new Task();
        task2.Id = 2;
        task2.Title = "Task2";
        task2.Description = "Description2";
        task2.State = State.New;
        task2.Tags = new List<Tag>() { tags[2], tags[3] };

        var task3 = new Task();
        task3.Id = 3;
        task3.Title = "Assignment 4";
        task3.Description = "Removed";
        task3.State = State.Removed;
        task3.AssignedTo = user;

        return new[] { task1, task2, task3 };
    }

    public User GetUser()
    {

        var user = new User();
        user.Id = 1;
        user.Name = "User1";
        user.Email = "user1@example.com";

        return user;
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}
