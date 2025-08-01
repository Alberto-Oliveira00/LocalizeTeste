using LocalizeTeste.Dtos;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LocalizeTeste.Services;

public class ReceitaWsService
{
    private readonly HttpClient _httpClient;

    public ReceitaWsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://www.receitaws.com.br/");
    }

    public async Task<ReceitaWsResponseDto> GetCnpjDataAsync(string cnpj)
    {
        string cleanCnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");

        var response = await _httpClient.GetAsync($"v1/cnpj/{cleanCnpj}");

        if(response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ReceitaWsResponseDto>(content);

            if(result != null && result.Status == "ERROR")
            {
                throw new InvalidOperationException($"Erro da ReceitaWS: {result.Message}");
            }
            return result;

        }
        else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException($"CNPJ {cnpj} não encontrado na ReceitaWS.");
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            throw new HttpRequestException("Limite de requisições da ReceitaWS excedido. Tente novamente mais tarde.");
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Erro ao consultar ReceitaWS: {response.StatusCode} - {errorContent}");
        }
    }
}