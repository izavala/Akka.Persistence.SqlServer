using Akka.Actor;
using Akka.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence_Failure_Test
{

    public sealed class Init
    {
        public static Init Instance { get; } = new Init();
        private Init() { }
    }

    public sealed class Finish
    {
        public static Finish Instance { get; } = new Finish();
        private Finish() { }
    }

    public sealed class Done
    {
        public static Done Instance { get; } = new Done();
        private Done() { }
    }

    public sealed class Finished
    {
        public Finished(long state)
        {
            State = state;
        }

        public long State { get; }
    }

    public sealed class Store
    {
        public Store(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public sealed class Stored
    {
        public Stored(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class TestActor : UntypedPersistentActor
    {
        private long _state = 0L;

        public TestActor(string persistenceId)
        {
            PersistenceId = persistenceId;
        }

        public sealed override string PersistenceId { get; }

        protected override void OnCommand(object message)
        {
            switch (message)
            {
                case Init i:
                    var sender = Sender;
                    Persist(new Stored(0), s =>
                    {
                        _state += s.Value;
                        Journal.Tell(Done.Instance);
                        sender.Tell(Done.Instance);
                        Console.Write("I'm Inititated");
                    });
                    break;
                case Store store:
                    Persist(new Stored(store.Value), s =>
                    {
                        Journal.Tell(Done.Instance);
                        _state += s.Value;
                        Console.Write("Received package");
                    });
                    break;
                case Finish _:
                    Sender.Tell(new Finished(_state));
                    Console.Write("Finished");
                    break;
            }
        }

        protected override void OnRecover(object message)
        {
            switch (message)
            {
                case Stored s:
                    _state += s.Value;
                    break;
            }
        }
    }
}
