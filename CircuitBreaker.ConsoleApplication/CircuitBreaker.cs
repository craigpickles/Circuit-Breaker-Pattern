using System;
using System.Threading;

namespace CircuitBreaker.ConsoleApplication
{
    public class CircuitBreaker
    {
        private readonly TimeSpan _openToHalfOpenWaitTime = new TimeSpan(0,0,5);

        private readonly CircuitBreakerState _stateStore = new CircuitBreakerState();

        private readonly object _halfOpenSyncObject = new object();

        public bool IsClosed => _stateStore.IsClosed;

        public bool IsOpen => !IsClosed;

        public void ExecuteAction(Action action)
        {
            if (IsOpen)
            {
                if (_stateStore.LastStateChangedDateUtc + _openToHalfOpenWaitTime < DateTime.UtcNow)
                {

                    bool lockTaken = false;
                    try
                    {
                        Monitor.TryEnter(_halfOpenSyncObject, ref lockTaken);
                        if (lockTaken)
                        {
                            _stateStore.HalfOpen();

                            // Attempt the operation.
                            action();

                            this._stateStore.Reset();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this._stateStore.Trip(ex);
                        throw;
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            Monitor.Exit(_halfOpenSyncObject);
                        }
                    }
                }

                throw new CircuitBreakerOpenException(_stateStore.LastException);
            }

            try
            {
                action();
            }
            catch (Exception ex)
            {
                TrackException(ex);
                throw;

            }
        }

        private void TrackException(Exception exception)
        {
            // Add logic in here for certain exceptions of to trip only after certain number of exceptions

            _stateStore.Trip(exception);
        }
    }
}
