using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs.Host.Dispatch;
using Microsoft.Azure.WebJobs.ServiceBus;
using ServiceBusTopic;
using System.Text;
using ServiceBusConfiguration = ServiceBusTopic.ServiceBusConfiguration;

const string TopicName = "ProductChangedTopic";
IConfiguration _configuration;
const string SubscriptionName = "paulista_store";
string _storeId = string.Empty;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

_configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

var serviceBusNamespace = _configuration.GetServiceBusNamespace();


if (!serviceBusNamespace.Topics.List()
  .Any(t => t.Name
  .Equals(TopicName, StringComparison.InvariantCultureIgnoreCase)))
{
    serviceBusNamespace.Topics
        .Define(TopicName)
        .WithSizeInMB(1024)
        .Create();
}

var topic = serviceBusNamespace.Topics.GetByName(TopicName);

if (topic.Subscriptions.List()
  .Any(subscription => subscription.Name
  .Equals(SubscriptionName,
         StringComparison.InvariantCultureIgnoreCase)))
{
    topic.Subscriptions.DeleteByName(SubscriptionName);
}

topic.Subscriptions
    .Define(SubscriptionName)
    .Create();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

async void ReceiveMessages(ServiceBusConfiguration serviceBusConfiguration)
{
    var subscriptionClient = new SubscriptionClient(serviceBusConfiguration.ConnectionString, TopicName, SubscriptionName);

    //by default a 1=1 rule is added when subscription is created, so we need to remove it
    await subscriptionClient.RemoveRuleAsync("$Default");

    await subscriptionClient.AddRuleAsync(new RuleDescription
    {
        Filter = new CorrelationFilter { Label = _storeId },
        Name = "filter-store"
    });

    var mo = new MessageHandlerOptions(ExceptionHandle) { AutoComplete = true };

    subscriptionClient.RegisterMessageHandler(MessageHandler, mo);

    Console.ReadLine();
}


static Task MessageHandler(Message message,CancellationToken arg2)
{
    Console.WriteLine($"message Label: {message.Label}");
    Console.WriteLine($"CorrelationId: {message.CorrelationId}");
    var prodChangesString = Encoding.UTF8.GetString(message.Body);

    Console.WriteLine("Message Received");
    Console.WriteLine(prodChangesString);

    //Thread.Sleep(40000);

    return Task.CompletedTask;
}

static Task ExceptionHandle(ExceptionReceivedEventArgs arg)
{
    Console.WriteLine($"Handler exception {arg.Exception}.");
    var context = arg.ExceptionReceivedContext;
    Console.WriteLine($"Endpoint: {context.Endpoint}, Paht: {context.EntityPath}, Action: {context.Action}");
    return Task.CompletedTask;
}

