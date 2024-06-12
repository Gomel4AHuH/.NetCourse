using System.Text.Json;

namespace Task2
{
    public class File
    {
        const string filePath = @"../../../gameResult.json";

        //save player data to file
        public async Task SaveDataAsync(List<Player> Players)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                await JsonSerializer.SerializeAsync<List<Player>>(fs, Players);

            }
        }

        //load player data from file
        public async Task<List<Player>> LoadDataAsync()
        {
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                List<Player> players = await JsonSerializer.DeserializeAsync<List<Player>>(fs);

                return players;
            }
        }
    }
}
