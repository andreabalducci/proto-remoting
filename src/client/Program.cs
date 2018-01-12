using System;
using System.Threading.Tasks;
using Messages;
using Proto;
using Proto.Remote;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            Serialization.RegisterFileDescriptor(ProtocolReflection.Descriptor);
            Console.WriteLine("CLIENT");
            Remote.Start("127.0.0.1", 8001);

            var props = Actor.FromProducer(() => new ClientActor());
            var client = Actor.Spawn(props);

            Console.WriteLine("Press a key to send message");
            client.Tell(new Connect());

            Console.WriteLine("Press a key to quit");
            Console.ReadLine();
        }
    }

    public class ClientActor : IActor
    {
        private readonly PID _server = new PID("127.0.0.1:8000","server");

        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case Started _:
                {
                    Console.WriteLine("Client started");
                    break;
                }

                case Connect c:
                {
                    context.Tell(_server, new Connect()
                    {
                        Sender = context.Self
                    });
                    break;
                }

                case Connected c:
                {
                    Console.WriteLine("Connected to server!!!!");
                    break;
                }
            }

            return Actor.Done;
        }
    }
}
