using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.TechChallenge.Lambda.SQS.Pagamento.Domain.Entities.MercadoPago
{
    public class MercadoPagoQrCodeResponse
    {
        public string InStoreOrderId { get; set; }
        public string QrData { get; set; }
    }
}
