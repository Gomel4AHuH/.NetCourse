namespace Task2
{
    public struct MyConsole
    {
        public void Clear()
        {
            Console.Clear();
        }

        public string ReadMessage(Game game)
        {
            string? message = Console.ReadLine();
            switch (message)
            {
                case "/help":
                case "/помощь":
                    game.language.WelcomeText();
                    return "";
                case "/show-words":
                case "/показать-слова":
                    game.ShowWords();
                    return "";
                case "/score":
                case "/очки":
                    game.ShowScore();
                    return "";
                case "/total-score":
                case "/общие-очки":
                    return "";
                default:
                    return message;
            }
        }

        public void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
