using Akka.Actor;
using Akka.Configuration;
using Persistence_Failure_Test;
using System;

namespace SQLTest
{
    class Program
    {
        private static Config config = ConfigurationFactory.ParseString($@"
            akka.loglevel = INFO
            akka.test.single-expect-default = 10s
            akka.persistence.journal.plugin = ""akka.persistence.journal.sql-server""
            akka.persistence.journal.sql-server {{
                class = ""Akka.Persistence.SqlServer.Journal.BatchingSqlServerJournal, Akka.Persistence.SqlServer""
                plugin-dispatcher = ""akka.actor.default-dispatcher""
                table-name = EventJournal
                schema-name = dbo
                auto-initialize = on
                connection-string = ""Server = localhost\\SQLEXPRESS; Database=Akka;Trusted_Connection=True;""
                refresh-interval = 1s
            }}");


        static void Main(string[] args)
        {
            var system = ActorSystem.Create("SQLBatch", config);
            var actor = system.ActorOf(Props.Create(() => new TestActor("A4")));
            var watcher = system.ActorOf(Props.Create(() => new Watcher()));
            watcher.Tell(new Store(1), actor);
            Console.WriteLine("Actor Initialize");
            while (true)
            {
                Console.WriteLine("TypeAnyting to Test");
                Console.ReadLine();
                actor.Tell(new Store(1));
            }

        }
    }
}

