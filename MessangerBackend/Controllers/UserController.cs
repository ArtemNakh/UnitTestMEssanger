using AutoMapper;
using MessangerBackend.Core.Models;
using MessangerBackend.DTOs;
using MessangerBackend.Requests;
using MessangerBackend.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MessangerBackend.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : Controller
{
    private readonly MessangerContext _context;
    private readonly IMapper _mapper;

    public UserController(MessangerContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDTO>> AddUser(CreateUserRequest request)
    {
        var userDb = _mapper.Map<User>(request);
        userDb.CreatedAt = userDb.LastSeenOnline = DateTime.UtcNow;
        
        _context.Users.Add(userDb);
        await _context.SaveChangesAsync();
        
        return Created("user", _mapper.Map<UserDTO>(userDb));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<UserDTO>>(users));
    }
}