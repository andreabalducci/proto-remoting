using System;
using System.Threading.Tasks;
using Messages;
using Proto;
using Proto.Remote;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            Serialization.RegisterFileDescriptor(ProtocolReflection.Descriptor);
            Console.WriteLine("SERVER");
            Remote.Start("127.0.0.1", 8000);
            var props = Actor.FromProducer(() => new ServerActor());
            var server = Actor.SpawnNamed(props,"server");

            Console.ReadLine();
        }
    }

    public class ServerActor : IActor
    {
        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case Started _:
                {
                    Console.WriteLine("Server started");
                    break;
                }

                case Connect c:
                {
                    Console.WriteLine("Incoming connection");
                    context.Tell(c.Sender, new Connected()
                    {
                        Message = "Welcome on board"
                    });
                    break;
                }
            }

            return Actor.Done;
        }
    }
}
