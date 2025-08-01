using Newtonsoft.Json;

namespace LocalizeTeste.Dtos;

public class ReceitaWsAtividadeDto
{
    [JsonProperty("code")]
    public string Code { get; set; }
    [JsonProperty("text")]
    public string Text { get; set; }
}
