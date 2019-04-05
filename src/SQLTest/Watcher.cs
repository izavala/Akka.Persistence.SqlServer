using Akka.Actor;
using System;

namespace Persistence_Failure_Test
{
    public class Watcher : ReceiveActor
    {

        public Watcher()
        {
            Receive<Store>(x =>
            {
                // notify us if the actor who sent
                // this message dies in the future.
                Context.Watch(Sender);
            });

            // message we'll receive anyone we DeathWatch
            // dies, OR if the network terminates
            Receive<Terminated>(t =>
            {

                Console.Write("Actor has been terminated");
            });
        }

    }
}
