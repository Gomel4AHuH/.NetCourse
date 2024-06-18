using System.Text.Json;

namespace Task2
{
    public class FileService
    {
        const string filePath = @"../../../gameResult.json";

        //save game statistic to file
        public static async Task SaveDataAsync(Dictionary<string, int> gameStatistic)
        {
            using FileStream fs = new(filePath, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fs, gameStatistic);
        }

        //load game statistic from file
        public static async Task<Dictionary<string, int>> LoadDataAsync()
        {
            using FileStream fs = new(filePath, FileMode.OpenOrCreate);
            return await JsonSerializer.DeserializeAsync<Dictionary<string, int>>(fs);
        }
    }
}
