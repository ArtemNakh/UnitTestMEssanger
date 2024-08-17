using MessangerBackend.Core.Interfaces;
using MessangerBackend.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MessangerBackend.Core.Services;

public class UserService : IUserService
{
    private readonly IRepository _repository;

    public UserService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> Login(string nickname, string password)
    {
        if (string.IsNullOrWhiteSpace(nickname) || string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException();
        }

        return _repository.GetAll<User>()
            .SingleOrDefault(x => x.Nickname == nickname && x.Password == password)
            ?? throw new InvalidOperationException("Invalid login .");
      
    }

    public async Task<User> Register(string nickname, string password)
    {
        if (string.IsNullOrWhiteSpace(nickname) || string.IsNullOrWhiteSpace(password) || nickname.Length < 3 || password.Length < 6)
        {
            throw new ArgumentException();
        }

        var existingUser = _repository.GetAll<User>()
            .FirstOrDefault(x => x.Nickname == nickname);

        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this nickname already exists.");
        }

        var newUser = new User
        {
            Nickname = nickname,
            Password = password
        };

        _repository.Add(newUser);

        return newUser;
    }


    public Task<User> GetUserById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetUsers(int page, int size)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> SearchUsers(string nickname)
    {
        throw new NotImplementedException();
    }
    
    // якщо нікнейм коректний 
    // якщо нікнейм пустий 
    // якщо немає такого нікнейму
    // якщо нікнейм є частиною чиєгось нікнейму (User { Nickname = "TestDevUser" }, nickname = "Dev")
    // різні регістри (User { Nickname = "User" }, nickname = "user")




}