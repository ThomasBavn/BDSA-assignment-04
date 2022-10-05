
using Microsoft.Data.Sqlite;

namespace Assignment3.Entities.Tests;

public class UserRepositoryTests
{

    private readonly SqliteConnection _connection;
    private readonly KanbanContext _context;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly UserRepository _repository;
    public UserRepositoryTests(ITestOutputHelper outputHelper)
    {
        _testOutputHelper = outputHelper;

        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();

        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        optionsBuilder.UseSqlite(_connection);
        var context = new KanbanContext(optionsBuilder.Options);
        context.Database.EnsureCreated();


        //tasks
        var task1 = new Task();
        task1.Id = 1;
        task1.Title = "Task1";
        task1.Description = "Description1";
        task1.State = State.New;

        var task2 = new Task();
        task2.Id = 2;
        task2.Title = "Task2";
        task2.Description = "Description2";
        task2.State = State.Active;

        //users
        var user1 = new User();
        user1.Id = 1;
        user1.Name = "User1";
        user1.Email = "user1@example.com";
        user1.Tasks = new List<Task>() { task1, task2 };

        task1.AssignedTo = user1;
        task2.AssignedTo = user1;

        var hasTasks = new User();
        hasTasks.Id = 2;
        hasTasks.Name = "User2";
        hasTasks.Email = "user2@example.com";
        hasTasks.Tasks = new List<Task>() { task1, task2 };

        var user3 = new User();
        user3.Id = 3;
        user3.Name = "user3";
        user3.Email = "user3@example.com";

        context.Users.AddRange(user1, hasTasks, user3);

        context.SaveChanges();
        _context = context;
        _repository = new UserRepository(_context);

    }

    [Fact]
    public void Create_Success()
    {
        //Arrange
        var user = new UserCreateDTO(
            "newUser",
            "brandNew@example.com"
            );

        //Act
        var (response, index) = _repository.Create(user);

        //Assert
        Assert.Equal(Response.Created, response);
        Assert.Equal(4, index);
    }

    [Fact]
    public void Create_User_Already_Exists()
    {
        //Arrange
        var user = new UserCreateDTO(
            "User2",
            "user2@example.com"
            );

        //Act
        var response = _repository.Create(user);

        //Assert
        Assert.Equal((Response.Conflict, 0), response);
    }

    [Fact]
    public void Delete_User_With_Tasks()
    {
        var response = _repository.Delete(1);
        Assert.Equal(Response.Deleted, response);
    }

    [Fact]
    public void Delete_User_Without_Tasks()
    {
        var response = _repository.Delete(1);
        Assert.Equal(Response.Deleted, response);
    }

    [Fact]
    public void Delete_User_With_Tasks_Force()
    {
        var response = _repository.Delete(2, true);
        Assert.Equal(Response.Deleted, response);
    }
    [Fact]
    public void Delete_User_Doesnt_Exist()
    {
        var response = _repository.Delete(200);
        Assert.Equal(Response.NotFound, response);
    }

    [Fact]
    public void Find_User_Exists()
    {
        var response = _repository.Find(1);
        Assert.Equal(new UserDTO(1, "User1", "user1@example.com"), response);
    }

    [Fact]
    public void Find_User_Doesnt_Exist()
    {
        var response = _repository.Find(200);
        Assert.Null(response);
    }

    [Fact]
    public void Read_All_Users()
    {
        var response = _repository.ReadAll();
        Assert.Equal(_context.Users.Count(), response.Count);
    }








}
