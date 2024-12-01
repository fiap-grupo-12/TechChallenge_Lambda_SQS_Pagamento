using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FIAP.TechChallenge.Lambda.SQS.Pagamento.Domain.Entities.MercadoPago
{
    public class MercadoPagoToken
    {
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }
    }
}
