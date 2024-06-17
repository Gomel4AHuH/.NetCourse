namespace Task2
{
    public interface IConsole
    {
        void Info(string message);
        void Clear();
        string? Read();
    }

    public class ConsoleLogger : IConsole
    {
        public void Info(string message)
        {
            Console.WriteLine(message);
        }
        public void Clear()
        {
            Console.Clear();
        }
        public string? Read()
        {
            return Console.ReadLine();
        }
    }

    public class MyConsole(IConsole logger)
    {
        public readonly IConsole _logger = logger;
        public void Clear()
        {
            _logger.Clear();
        }

        public string ReadMessage(Game game)
        {
            string? message = _logger.Read();

            try
            {
                switch (message)
                {
                    case "/help":
                    case "/помощь":
                        game.Language.WelcomeText();
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
                        game.ShowTotalScore();
                        return "";
                    default:
                        return message;
                }
            }
            catch
            {
                game.Language.ErrorMessage("ReadMessage");
                return message;
            }
        }

        public void WriteMessage(string message)
        {
            _logger.Info(message);
        }
    }
}
