using System.Text.RegularExpressions;

namespace Task2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILanguage Language = null;
            MyConsole myConsole = new MyConsole();
            bool result = false;

            myConsole.WriteMessage("Welcome to \"Word\" game!!!");

            while (!result)
            {
                myConsole.WriteMessage("Please, choose language:");
                myConsole.WriteMessage("1. English");
                myConsole.WriteMessage("2. Русский");

                string? userChoice = Console.ReadLine();

                if (Regex.IsMatch(userChoice, @"^[1-2]$"))
                {
                    result = true;
                    switch (int.Parse(userChoice))
                    {
                        case 1:
                            Language = new English();
                            break;
                        case 2:
                            Language = new Russian();
                            break;
                    }
                }
            }

            myConsole.Clear();
            Player player1 = new Player(Language, 1);
            Player player2 = new Player(Language, 2);

            Language.EnterMainWord();
            string mainWord = myConsole.ReadMessage();

            myConsole.WriteMessage(mainWord);

            myConsole.WriteMessage(Language.regex);
            //по очереди вводим слова игроков
        }
    }
}