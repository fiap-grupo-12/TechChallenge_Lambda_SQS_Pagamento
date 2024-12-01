using Amazon.DynamoDBv2.DataModel;
using FIAP.TechChallenge.Lambda.SQS.Pagamento.Domain.Entities.Enum;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.Lambda.SQS.Pagamento.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    [DynamoDBTable("PagamentoTable")]
    public class Pagamento
    {
        [DynamoDBHashKey("id")]
        public Guid Id { get; set; }

        [DynamoDBProperty("valorTotal")]
        public double ValorTotal { get; set; }

        [DynamoDBProperty("statusPagamento")]
        public StatusPagamento StatusPagamento { get; set; }

        [DynamoDBProperty("qrCode")]
        public string QrCode { get; set; }

        [DynamoDBProperty("dataCriacao")]
        public DateTime DataCriacao { get; set; }
    }
}
