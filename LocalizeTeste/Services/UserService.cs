using LocalizeTeste.Data;
using LocalizeTeste.Dtos;
using LocalizeTeste.Models;
using Microsoft.EntityFrameworkCore;

namespace LocalizeTeste.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task <User> RegisterUserAsync(UserRegisterDTO userRegisterDto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userRegisterDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("E-mail já cadastrado.");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password);

        var user = new User
        {
            Name = userRegisterDto.Name,
            Email = userRegisterDto.Email,
            PasswordHash = passwordHash
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> AuthenticateUserAync(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if(user == null)
            return null;

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

        if(!isPasswordValid)
            return null;

        return user;
    }
}
