using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.Lambda.SQS.Pagamento
{
    public class BodyRequest
    {
        [JsonProperty("idPedido")]
        public Guid IdPedido { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }
    }
}
