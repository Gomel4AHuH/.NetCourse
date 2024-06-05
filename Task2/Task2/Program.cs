namespace Task2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Game game = new Game();
            
            game.Start();
            
            game.Finish();

            Console.WriteLine("Press...");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
            }
        }
    }
}