using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence_Failure_Test
{
    public sealed class StopMeasure
    {
        public static StopMeasure Instance { get; } = new StopMeasure();
        private StopMeasure() { }
    }

    public sealed class FailAt
    {
        public FailAt(long sequenceNr)
        {
            SequenceNr = sequenceNr;
        }

        public long SequenceNr { get; }
    }

    public sealed class Measure
    {
        public readonly int MessagesCount;

        public Measure(int messagesCount)
        {
            MessagesCount = messagesCount;
        }

        public DateTime StartedAt { get; private set; }
        public DateTime StopedAt { get; private set; }

        public void StartMeasure()
        {
            StartedAt = DateTime.Now;
        }

        public double StopMeasure()
        {
            StopedAt = DateTime.Now;
            return MessagesCount / (StopedAt - StartedAt).TotalSeconds;
        }
    }

    public class PerformanceTestException : Exception
    {
        public PerformanceTestException()
        {
        }

        public PerformanceTestException(string message) : base(message)
        {
        }

        public PerformanceTestException(string message, Exception innerException) : base(message, innerException)
        {
        }


    }
}
