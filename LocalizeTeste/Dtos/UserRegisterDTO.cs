using System.ComponentModel.DataAnnotations;

namespace LocalizeTeste.Dtos;

public class UserRegisterDTO
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    [StringLength(150, ErrorMessage = "O e-mail não pode exceder 150 caracteres.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    [StringLength(100, ErrorMessage = "A senha não pode exceder 100 caracteres.")]
    public string Password { get; set; }
}
