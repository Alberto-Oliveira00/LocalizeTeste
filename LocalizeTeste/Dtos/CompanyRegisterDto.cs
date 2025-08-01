using System.ComponentModel.DataAnnotations;

namespace LocalizeTeste.Dtos;

public class CompanyRegisterDto
{
    [Required(ErrorMessage = "O CNPJ é obrigatório.")]
    [StringLength(18, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter entre 14 e 18 caracteres (com ou sem formatação).")]
    
    [RegularExpression(@"^\d{2}\.?\d{3}\.?\d{3}\/?\d{4}\-?\d{2}$|^\d{14}$", ErrorMessage = "Formato de CNPJ inválido.")]
    public string Cnpj { get; set; }
}
