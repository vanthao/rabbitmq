using MassTransit;
using Ncua.Logging.Entity;
using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
   public class OrderSubmittedEventConsumer :
           IConsumer<CustomMessage>
    {
        public async Task Consume(ConsumeContext<CustomMessage> context)
        {
            Console.WriteLine("Order Submitted: {0}", context.Message.Message);
        }
    }
}