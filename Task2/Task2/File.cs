using System.Text.Json;

namespace Task2
{

    public interface ILogger
    {
        void Info(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class File
    {
        private readonly ILogger _logger;

        public File(ILogger logger)
        {
            _logger = logger;
        }
        public async Task SaveDataAsync(List<Player> Players)
        {
            //const 
            using (FileStream fs = new FileStream(@"../../../gameResult.json", FileMode.OpenOrCreate))
            {
                await JsonSerializer.SerializeAsync<List<Player>>(fs, Players);
                _logger.Info("Data has been saved to file");
            }
        }        
    }
}
