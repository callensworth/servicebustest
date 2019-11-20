using System.Threading.Tasks;

namespace beta
{ 

  
  public interface IServiceBusConsumer
  {

    void RegisterOnMessageHandlerAndReceiveMessages();

    Task CloseQueueAsync();
  }

}