using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;

using beta.BusinessLogic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace beta
{
    
    public class ServiceBusConsumer : BackgroundService, IServiceBusConsumer
    {

        private readonly IConfiguration _configuration;
        
        private readonly QueueClient _queueClient;
        
        private const string QUEUE_NAME = "postoffice";
        
        private readonly ILogger _logger;

        private readonly IServiceProvider Services;

        //private readonly IRepository _repository;

        //constructor: process-data, configuration, and queue injected
        public ServiceBusConsumer(
            IConfiguration configuration,
            ILogger<ServiceBusConsumer> logger,
            IServiceProvider services
            )
        {
            
            _configuration = configuration;
            
            _queueClient = new QueueClient(
              _configuration.GetConnectionString("ServBusConStr"), QUEUE_NAME);

            _logger = logger;

            Services = services;
        }

        //Handles the events when a message becomes available
        public void RegisterOnMessageHandlerAndReceiveMessages()
        {
            
            //setup message-handler to deal with errors as message comes in from Service Bus
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };


            //set up the event listener        (what to do          , how to handle errors)
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

        }

        //process the message recieved
        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            //convert to a Number by way of String -> JSON -> Number
            BusinessLogic.Number myPayload = JsonConvert.DeserializeObject<BusinessLogic.Number>(Encoding.UTF8.GetString(message.Body));

            //Process that Number
            //will release when finished: prevents memory leaks
            using (var scope = Services.CreateScope())
            {
                //get the repository
                var _repository = scope.ServiceProvider.GetRequiredService<IRepository>();

                try
                {
                    //place the number
                    await _repository.PlaceNumberAsync(myPayload);
                }
                catch (Exception e)
                {
                    string danger = "Cannot Process this Number: " + e;
                    _logger.LogError(danger);
                    
                }
                finally
                {
                    //signal we are finished
                    await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
                }
            }

            
        }


        //process the data to the repository
        public async Task Process(BusinessLogic.Number num)
        {
            
        }
        //handles exceptions
        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            //log an exception occurance
            _logger.LogError(exceptionReceivedEventArgs.Exception, "Message handler encountered an exception");

            //the exception itself
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            //log the details of the error
            _logger.LogDebug($"- Endpoint: {context.Endpoint}");
            _logger.LogDebug($"- Entity Path: {context.EntityPath}");
            _logger.LogDebug($"- Executing Action: {context.Action}");

            //finish handling the error
            return Task.CompletedTask;
        }

        //close the queue
        public async Task CloseQueueAsync()
        {
            //do the closing
            await _queueClient.CloseAsync();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Delay(1);
             _logger.LogInformation("ExecuteAsync called, not implemented");
            return null;
        }
    }
}