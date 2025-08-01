using LocalizeTeste.Dtos;
using LocalizeTeste.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LocalizeTeste.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CompaniesController : ControllerBase
{
    private readonly CompanyService _companyService;

    public CompaniesController(CompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterCompany([FromBody] CompanyRegisterDto companyRegisterDto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized(new { message = "ID do usuário não encontrado ou inválido no token." });
        }

        try
        {
            var newCompany = await _companyService.RegisterCompanyAsync(companyRegisterDto.Cnpj, userId);

            var responseDto = new CompanyResponseDto
            {
                Id = newCompany.Id,
                NomeEmpresarial = newCompany.NomeEmpresarial,
                NomeFantasia = newCompany.NomeFantasia,
                Cnpj = newCompany.Cnpj,
                Situacao = newCompany.Situacao,
                Abertura = newCompany.Abertura,
                Tipo = newCompany.Tipo,
                NaturezaJuridica = newCompany.NaturezaJuridica,
                AtividadePrincipal = newCompany.AtividadePrincipal,
                Logradouro = newCompany.Logradouro,
                Numero = newCompany.Numero,
                Complemento = newCompany.Complemento,
                Bairro = newCompany.Bairro,
                Municipio = newCompany.Municipio,
                Uf = newCompany.Uf,
                Cep = newCompany.Cep,
                UserId = newCompany.UserId
            };
            return CreatedAtAction(nameof(RegisterCompany), new { id = newCompany.Id }, responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao cadastrar empresa: {ex.Message}");
            return StatusCode(500, new { message = "Ocorreu um erro interno ao cadastrar a empresa." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetCompanies([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized(new { message = "ID do usuário não encontrado ou inválido no token." });
        }

        try
        {
            var paginationCompanies = await _companyService.GetCompaniesByUserIdAsync(userId, pageNumber, pageSize);

            var responseDtos = paginationCompanies.Items.Select(c => new CompanyResponseDto
            {
                Id = c.Id,
                NomeEmpresarial = c.NomeEmpresarial,
                NomeFantasia = c.NomeFantasia,
                Cnpj = c.Cnpj,
                Situacao = c.Situacao,
                Abertura = c.Abertura,
                Tipo = c.Tipo,
                NaturezaJuridica = c.NaturezaJuridica,
                AtividadePrincipal = c.AtividadePrincipal,
                Logradouro = c.Logradouro,
                Numero = c.Numero,
                Complemento = c.Complemento,
                Bairro = c.Bairro,
                Municipio = c.Municipio,
                Uf = c.Uf,
                Cep = c.Cep,
                UserId = c.UserId
            }).ToList();

            return Ok(new PaginationResponseDto<CompanyResponseDto>
            {
                TotalCount = paginationCompanies.TotalCount,
                PageNumber = paginationCompanies.PageNumber,
                PageSize = paginationCompanies.PageSize,
                Items = responseDtos
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao listar empresas: {ex.Message}");
            return StatusCode(500, new { message = "Ocorreu um erro interno ao obter as empresas." });
        }
    }
}
