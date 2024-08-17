using MessangerBackend.Core.Interfaces;
using MessangerBackend.Core.Models;
using MessangerBackend.Core.Services;
using MessangerBackend.Tests.Fakes;

namespace MessangerBackend.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task UserService_Login_CorrectInput()
    {
        // AAA Assign, Act, Assert
        var userService = CreateUserService();
        var expectedUser = new User()
        {
            Nickname = CorrectNickname,
            Password = CorrectPassword,
        };

        await userService.Register(CorrectNickname, CorrectPassword);
        var user = await userService.Login(CorrectNickname, CorrectPassword);
        
        Assert.Equal(expectedUser, user, new UserComparer());
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public async Task UserService_Login_ThrowsExceptionWhenEmptyField(string data)
    {
        // Assign
        var service = CreateUserService();
        
        // Act
        var exceptionNicknameHandler = async () =>
        {
            await service.Login(data, CorrectPassword);
        };
        var exceptionPasswordHandler = async () =>
        {
            await service.Login(CorrectNickname, data);
        };
        
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(exceptionNicknameHandler);
        await Assert.ThrowsAsync<ArgumentNullException>(exceptionPasswordHandler);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("1")]
    [InlineData("@")]
    public async Task UserService_Register_ThrowsExceptionWhenIncorrectNickname(string nickname)
    {
        var service = CreateUserService();
        
        var exceptionHandler = async () =>
        {
            await service.Register(nickname, CorrectPassword);
        };

        await Assert.ThrowsAsync<ArgumentException>(exceptionHandler);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("1")]
    [InlineData("9999")]
    public async Task UserService_Register_ThrowsExceptionWhenIncorrectPassword(string password)
    {
        var service = CreateUserService();
        
        var exceptionHandler = async () =>
        {
            await service.Register(CorrectNickname, password);
        };

        await Assert.ThrowsAsync<ArgumentException>(exceptionHandler);
    }

    private const string CorrectNickname = "TestUser";

    private const string CorrectPassword = "rRT56TGV!_gTr";

    private IUserService CreateUserService()
    {
        /*var options = new DbContextOptionsBuilder<MessangerContext>()
            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MessangerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
            .Options;
        var context = new MessangerContext(options);
        var repository = new Repository(context);*/
        return new UserService(new FakeUserRepository());
    }

    



    //new
    [Fact]
    public async Task UserService_Login_ThrowsExceptionWhenUserNotFound()
    {
        var service = CreateUserService();

        var exceptionHandler = async () =>
        {
            await service.Login("NonExistingUser", CorrectPassword);
        };

        await Assert.ThrowsAsync<InvalidOperationException>(exceptionHandler);
    }



    [Fact]
    public async Task UserService_Register_SuccessfullyRegistersUser()
    {
        var service = CreateUserService();
        var nickname = "NewUser";
        var password = "NewPassword!23";

        var registeredUser = await service.Register(nickname, password);
        var loginUser = await service.Login(nickname, password);

        Assert.Equal(registeredUser, loginUser, new UserComparer());
    }


    [Fact]
    public async Task UserService_Register_ThrowsExceptionWhenUserAlreadyExists()
    {
        var service = CreateUserService();

        await service.Register(CorrectNickname, CorrectPassword);

        var exceptionHandler = async () =>
        {
            await service.Register(CorrectNickname, CorrectPassword);
        };

        await Assert.ThrowsAsync<InvalidOperationException>(exceptionHandler);
    }


}

class UserComparer : IEqualityComparer<User>
{
    public bool Equals(User x, User y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Nickname == y.Nickname && x.Password == y.Password;
    }

    public int GetHashCode(User obj)
    {
        return HashCode.Combine(obj.Nickname, obj.Password);
    }
}