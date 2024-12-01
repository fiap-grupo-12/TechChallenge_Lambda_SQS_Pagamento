using Amazon.DynamoDBv2.DataModel;
using FIAP.TechChallenge.Lambda.SQS.Pagamento.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.TechChallenge.Lambda.SQS.Pagamento.Infra.Data.Repositories
{
    public class PagamentoRepository
    {
        private readonly IDynamoDBContext _context;

        public PagamentoRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<FIAP.TechChallenge.Lambda.SQS.Pagamento.Domain.Entities.Pagamento> Post(FIAP.TechChallenge.Lambda.SQS.Pagamento.Domain.Entities.Pagamento pagamento)
        {
            try
            {
                await _context.SaveAsync(pagamento);

                return pagamento;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar pagamento. {ex.Message}", ex);
            }
        }
    }
}
