/*2019 Charles Allensworth for Revature
 * 
 * Sends a message to the Azure Service bus
 * 
 * Algorythm:
 * 
 *  Accept object
 *  Convert to Json
 *  Convert to String
 *  Send to Service Bus
 * 
 * Source Code based on : 
 *  https://damienbod.com/2019/04/23/using-azure-service-bus-queues-with-asp-net-core-services/
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//service bus
using Microsoft.Azure.ServiceBus;

//used by dependancy injection
using Microsoft.Extensions.Configuration;

//jsons sent and recieved
using Newtonsoft.Json;
using System.Text;

 
namespace alpha
{
    public class ServiceBusSender
    {
        //declare the sb-queue
        private readonly QueueClient _queueClient;
        
        //declare the configuration
        private readonly IConfiguration _configuration;

        //get the queue being sent to
        private const string QUEUE_NAME = "postoffice";
        
        //constructor
        public ServiceBusSender(IConfiguration configuration)
        {
            //inject the configurationj file
            _configuration = configuration;

            //inject the service bus queeue
            _queueClient = new QueueClient
              ( _configuration.GetConnectionString("ServBusConStr"), 
                QUEUE_NAME
              );
        }
         
        //Sends a message to the queue
        public async Task SendMessage(BusinessLogic.Number payload)
        {
            //convert the json to a string
            string dataStr = JsonConvert.SerializeObject(payload);
            
            //service bus message created
            Message message = new Message(Encoding.UTF8.GetBytes(dataStr));
            
            //send the message to the service-bus-queue
            await _queueClient.SendAsync(message);
        }
    }
}