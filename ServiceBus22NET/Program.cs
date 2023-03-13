// See https://aka.ms/new-console-template for more information

using Microsoft.Azure.ServiceBus;
using System.Text;

//const string QueueConnectionString = "Endpoint=sb://geekburguer22.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=wLXla8SzHdd9ZhKyxa2XXhpcw3x2c3Biu+ASbPknjoA=";
const string QueueConnectionString = "Endpoint=sb://servicebussidney.servicebus.windows.net/;SharedAccessKeyName=ProductPolicy;SharedAccessKey=pfzTDEltzZou0npm/C1tAgi8arVMuQjy6+ASbM1KbXw=";
const string QueuePath = "ProductChanged";
IQueueClient _queueClient;

SendMessagesAsync().GetAwaiter().GetResult();
Console.WriteLine("messages were sent");
ReceiveMessagesAsync().GetAwaiter().GetResult();
Console.ReadLine();

Console.ReadLine();

async Task SendMessagesAsync()
{
    _queueClient = new QueueClient(QueueConnectionString, QueuePath);
    var messages = "Hi,Hello,Hey,How are you,Be Welcome"
        .Split(',')
        .Select(msg => {
            Console.WriteLine($"Will send message: {msg}");
            return new Message(Encoding.UTF8.GetBytes(msg));
        })
                .ToList();
    await _queueClient.SendAsync(messages);
    await _queueClient.CloseAsync();


}

async Task ReceiveMessagesAsync()
{
    _queueClient = new QueueClient(QueueConnectionString, QueuePath);
    _queueClient.RegisterMessageHandler(MessageHandler,
        new MessageHandlerOptions(ExceptionHandler) { AutoComplete = false });
    Console.ReadLine();
    await _queueClient.CloseAsync();
}

static Task ExceptionHandler(ExceptionReceivedEventArgs exceptionArgs)
{
    Console.WriteLine($"Message handler encountered an exception {exceptionArgs.Exception}.");
    var context = exceptionArgs.ExceptionReceivedContext;
    Console.WriteLine($"Endpoint:{context.Endpoint}, Path:{context.EntityPath}, Action:{context.Action}");
    return Task.CompletedTask;
}

async Task MessageHandler(Message message, CancellationToken cancellationToken)
{
    Console.WriteLine($"Received message:{ Encoding.UTF8.GetString(message.Body)}");
    await _queueClient.CompleteAsync(
         message.SystemProperties.LockToken);
}
