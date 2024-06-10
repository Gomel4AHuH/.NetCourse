using System;
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
        const string filePath = @"../../../gameResult.json";

        public File(ILogger logger)
        {
            _logger = logger;
        }

        //save player data to file
        public async Task SaveDataAsync(List<Player> Players)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                await JsonSerializer.SerializeAsync<List<Player>>(fs, Players);
                _logger.Info("Data has been saved to file");
            }
        }

        //load player data from file
        public async Task<List<Player>> LoadDataAsync()
        {
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                _logger.Info("Data has been loaded from file1");
                List<Player> players = await JsonSerializer.DeserializeAsync<List<Player>>(fs);
                //return await JsonSerializer.DeserializeAsync<List<Player>>(fs);
                //Console.WriteLine($"Name: {players[0].Name}");
                _logger.Info("Data has been loaded from file");
                return players;
            }
        }
    }
}
