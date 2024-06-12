namespace Task2
{   
    public class MyConsole
    {
        public void Clear()
        {
            Console.Clear();
        }

        public string ReadMessage(Game game)
        {
            string? message = Console.ReadLine();

            try
            {
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
                        game.ShowTotalScore();
                        return "";
                    default:
                        return message;
                }
            }
            catch
            {
                game.language.ErrorMessage("ReadMessage");
                return message;
            }
        }

        public void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
