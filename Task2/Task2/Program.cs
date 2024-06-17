namespace Task2
{
    public class Program
    {
        private static readonly ConsoleLogger Logger = new();
        static async Task Main()
        {

            Game Game = new(Logger);

            Game.ChooseLanguage();

            await Game.Start();

        }
    }
}