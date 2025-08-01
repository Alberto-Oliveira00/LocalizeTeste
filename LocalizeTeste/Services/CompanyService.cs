using LocalizeTeste.Data;
using LocalizeTeste.Dtos;
using LocalizeTeste.Models;
using Microsoft.EntityFrameworkCore;

namespace LocalizeTeste.Services;

public class CompanyService
{
    private readonly AppDbContext _context;
    private readonly ReceitaWsService _receitaWsService;

    public CompanyService(AppDbContext context, ReceitaWsService receitaWsService)
    {
        _context = context;
        _receitaWsService = receitaWsService;
    }

    public async Task<Company> RegisterCompanyAsync(string cnpj, int userId)
    {
        var companyExists = await _context.Companies.FirstOrDefaultAsync(c => c.Cnpj == cnpj);
        if (companyExists != null)
            throw new InvalidOperationException($"Empresa com {cnpj} já cadastrada.");

        ReceitaWsResponseDto cnpjData;
        try
        {
            cnpjData = await _receitaWsService.GetCnpjDataAsync(cnpj);
        }
        catch (HttpRequestException ex)
        {
            throw new ApplicationException($"Falha ao consultar CNPJ na ReceitaWS: {ex.Message}", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new ApplicationException($"Dados inválidos da ReceitaWS para CNPJ {cnpj}: {ex.Message}", ex);
        }
        catch (KeyNotFoundException ex)
        {
            throw new ApplicationException($"CNPJ {cnpj} não encontrado ou inválido.", ex);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Erro inesperado ao consultar ReceitaWS para CNPJ {cnpj}: {ex.Message}", ex);
        }

        var company = new Company
        {
            NomeEmpresarial = cnpjData.NomeEmpresarial,
            NomeFantasia = cnpjData.NomeFantasia,
            Cnpj = cnpjData.Cnpj,
            Situacao = cnpjData.Situacao,
            Abertura = cnpjData.Abertura,
            Tipo = cnpjData.Tipo,
            NaturezaJuridica = cnpjData.NaturezaJuridica,
            AtividadePrincipal = cnpjData.AtividadePrincipal != null && cnpjData.AtividadePrincipal.Any()
                                     ? cnpjData.AtividadePrincipal.First().Text
                                     : "Não informada",
            Logradouro = cnpjData.Logradouro,
            Numero = cnpjData.Numero,
            Complemento = cnpjData.Complemento,
            Bairro = cnpjData.Bairro,
            Municipio = cnpjData.Municipio,
            Uf = cnpjData.Uf,
            Cep = cnpjData.Cep,
            UserId = userId
        };

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        return company;
    }

    public async Task<IEnumerable<Company>> GetCompaniesByUserIdAsync (int userId)
    {
        return await _context.Companies
                             .Where(c => c.UserId == userId)
                             .ToListAsync();
    }
}
