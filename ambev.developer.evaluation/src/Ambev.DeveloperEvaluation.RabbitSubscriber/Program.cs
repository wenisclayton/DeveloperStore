using Ambev.DeveloperEvaluation.RabbitSubscriber;
using Ambev.DeveloperEvaluation.RabbitSubscriber.IntegrationsEvents;
using EasyNetQ;
using EasyNetQ.DI;


using var bus = RabbitHutch.CreateBus("host=ambev.developerevaluation.rabbitmq;username=developer;password=ev@luAt10n;virtualHost=/", s =>
{
    s.EnableSystemTextJson();
    s.Register<ITypeNameSerializer>(_ => new CustomTypeNameSerializer());
});

// Inscreve-se para receber mensagens do tipo MyMessage
// "meu_subscriber" é o identificador da assinatura, pode ser qualquer string única
bus.PubSub.Subscribe<SaleCreatedIntegrationEvent>("Ambev_DeveloperEvaluation_SaleCreated", 
    message =>
{
    Console.WriteLine("Mensagem recebida: -----------------------------------------");
    Console.WriteLine("DataVenda: " + message.DateEvent);
    Console.WriteLine("Id da Venda: " + message.Id);
    Console.WriteLine("Numero Venda: " + message.SaleNumber);
    Console.WriteLine("Evento: " + message.TipoEvent);
});

bus.PubSub.Subscribe<SaleCancelledIntegrationEvent>("Ambev_DeveloperEvaluation_SaleCancelled", 
    message =>
{
    Console.WriteLine("Mensagem recebida: -----------------------------------------");
    Console.WriteLine("DataVenda: " + message.DateEvent);
    Console.WriteLine("Id da Venda: " + message.Id);
    Console.WriteLine("Numero Venda: " + message.SaleNumber);
    Console.WriteLine("Evento: " + message.TipoEvent);
});

bus.PubSub.Subscribe<SaleItemCancelledIntegrationEvent>("Ambev_DeveloperEvaluation_SaleItemCancelled", 
    message =>
{
    Console.WriteLine("Mensagem recebida: -----------------------------------------");
    Console.WriteLine("DataVenda: " + message.DateEvent);
    Console.WriteLine("Id da Venda: " + message.Id);
    Console.WriteLine("Numero Venda: " + message.SaleNumber);
    Console.WriteLine("Evento: " + message.TipoEvent);
});

bus.PubSub.Subscribe<SaleModifiedIntegrationEvent>("Ambev_DeveloperEvaluation_SaleChanged", 
    message =>
{
    Console.WriteLine("Mensagem recebida: -----------------------------------------");
    Console.WriteLine("DataVenda: " + message.DateEvent);
    Console.WriteLine("Id da Venda: " + message.Id);
    Console.WriteLine("Numero Venda: " + message.SaleNumber);
    Console.WriteLine("Evento: " + message.TipoEvent);
});


Console.WriteLine("Subscriber ativo. Pressione [Enter] para sair.");
Console.ReadLine();