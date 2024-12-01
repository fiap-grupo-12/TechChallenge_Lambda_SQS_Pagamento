using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using FIAP.TechChallenge.Lambda.SQS.Pagamento.Domain.Entities.Enum;
using FIAP.TechChallenge.Lambda.SQS.Pagamento.Infra.Data.Repositories;
using Newtonsoft.Json;
using Amazon.SQS;
using FIAP.TechChallenge.Lambda.SQS.Pagamento.Infra.Data.Repositories.MercadoPago;
using Amazon;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Amazon.SQS.Model;



// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace FIAP.TechChallenge.Lambda.SQS.Pagamento;

public class Function
{
    private readonly IDynamoDBContext _context;
    private readonly IAmazonSQS amazonSQS;
    private readonly string _url;
    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Function()
    {
        IAmazonDynamoDB amazonDynamo = new AmazonDynamoDBClient(RegionEndpoint.USEast1);
        _context = new DynamoDBContext(amazonDynamo);
        amazonSQS = new AmazonSQSClient(RegionEndpoint.USEast1);
        _url = Environment.GetEnvironmentVariable("url_sqs_solicita_pagamento");
    }


    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    /// to respond to SQS messages.
    /// </summary>
    /// <param name="evnt">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        foreach(var message in evnt.Records)
        {
            await ProcessMessageAsync(message, context);
        }
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        BodyRequest request = JsonConvert.DeserializeObject<BodyRequest>(message.Body);

        var repository = new PagamentoRepository(_context);
        var mercadoPagoRepository = new MercadoPagoRepository();


        var entity = new FIAP.TechChallenge.Lambda.SQS.Pagamento.Domain.Entities.Pagamento
        {
            Id = request.IdPedido,
            ValorTotal = request.Valor,
            QrCode = await mercadoPagoRepository.GetQrCode(request.IdPedido.ToString(), request.Valor),
            StatusPagamento = StatusPagamento.Pendente,
            DataCriacao = DateTime.Now
        };
        var pagamento = repository.Post(entity);

        await amazonSQS.DeleteMessageAsync(new DeleteMessageRequest() { QueueUrl = _url, ReceiptHandle = message.ReceiptHandle });

        // TODO: Do interesting work based on the new message
        await Task.CompletedTask;
    }
}