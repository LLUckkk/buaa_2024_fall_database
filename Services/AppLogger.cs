using Market.Interfaces;

namespace Market.Services
{
    public class AppLogger : IAppLogger<string>
    {
        public void LogInformation(string message, params object[] args)
        {
            Console.WriteLine($"INFO: {message}", args);
        }

        public void LogWarning(string message, params object[] args)
        {
            Console.WriteLine($"WARNING: {message}", args);
        }

        public void LogError(string message, params object[] args)
        {
            Console.WriteLine($"ERROR: {message}", args);
        }
    }

}