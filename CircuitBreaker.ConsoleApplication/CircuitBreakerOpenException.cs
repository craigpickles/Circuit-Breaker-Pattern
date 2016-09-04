using System;

namespace CircuitBreaker.ConsoleApplication
{
    public class CircuitBreakerOpenException : Exception
    {
        public CircuitBreakerOpenException(Exception lastException) : base("CircuitBreakerOpenException", lastException) { }
    }
}