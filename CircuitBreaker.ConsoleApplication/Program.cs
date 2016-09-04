using System;

namespace CircuitBreaker.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var breaker = new CircuitBreaker();
            int sum = 0;

            while (true)
            {
                try
                {
                    Console.Write("Add: ");
                    var value = Console.ReadLine();
                    
                    breaker.ExecuteAction(() =>
                    {
                        Console.WriteLine("-- Executing --");
                        sum  += int.Parse(value); // Throws error if not a number
                        Console.WriteLine("SUM = " + sum);
                    });

                }
                catch (CircuitBreakerOpenException)
                {
                    Console.WriteLine("Rejected: CircuitBreakerOpenException so blocked");
                }
                catch (Exception) { }
            }
        }
    }
}
