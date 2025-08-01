using LocalizeTeste.Dtos;
using LocalizeTeste.Models;
using LocalizeTeste.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalizeTeste.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var newUser = await _userService.RegisterUserAsync(userRegisterDto);
            return CreatedAtAction(nameof(Register), new { id = newUser.Id },
                                                     new { newUser.Id, newUser.Name, newUser.Email });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao registrar usuário: {ex.Message}");
            return StatusCode(500, new { message = "Ocorreu um erro interno ao registrar o usuário." });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto LoginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = await _userService.AuthenticateUserAync(LoginDto);

            if(user == null)
                return Unauthorized(new { message = "E-mail ou senha inválidos." });

            return Ok(new { message = "Login bem-sucedido!", userId = user.Id, userName = user.Name });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao fazer login: {ex.Message}");
            return StatusCode(500, new { message = "Ocorreu um erro interno ao tentar fazer o login." });
        }
    }
}
