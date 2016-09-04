using System;

namespace CircuitBreaker.ConsoleApplication
{
    public class CircuitBreakerState
    {
        private State State { get; set; }

        public bool IsClosed
        {
            get { return State == State.Closed; }
        }

        public DateTime LastStateChangedDateUtc { get; set; }
        public Exception LastException { get; set; }

        public void Trip(Exception exception)
        {
            SetState(State.Open);
            LastException = exception;
        }

        public void HalfOpen() => SetState(State.HalfOpen);

        public void Reset() => SetState(State.Closed);

        private void SetState(State state)
        {
            State = state;
            LastStateChangedDateUtc = DateTime.UtcNow;
        }
    }
}
