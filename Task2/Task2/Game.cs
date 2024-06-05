using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Task2
{
    internal class Game
    {
        public string Word { get; set; }
        public int MinChars { get; set; }
        public int MaxChars { get; set; }
        public ILanguage language { get; set; }
        public MyConsole myConsole = new MyConsole();
        private string numberRegex = @"^\d+$";
        public List<string> playerWords = new List<string>();

        public Game()
        {
        }

        public void Start()
        {
            WelcomeText();

            ChooseLanguage();

            EnterWord();

            Player player1 = new Player(language, myConsole, 1);
            Player player2 = new Player(language, myConsole, 2);

            player1.ReadPlayerWord(language, myConsole, Word, playerWords);
            player2.ReadPlayerWord(language, myConsole, Word, playerWords);

            /*string[] arr = playerWords.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                string[] tmpArr = arr[i].Split('|');
                myConsole.WriteMessage(playerWords[i]);
            }*/
        }

        public void Finish()
        {
            myConsole.WriteMessage("Game is over.");
        }

        public void WelcomeText()
        {
            myConsole.WriteMessage("Welcome to \"Word\" game!!!\n");
        }

        public void ChooseLanguage()
        {
            bool result = false;
            while (!result)
            {
                myConsole.WriteMessage("Please, choose language:");
                myConsole.WriteMessage("1. English");
                myConsole.WriteMessage("2. Русский");

                string? userChoice = myConsole.ReadMessage();

                if (Regex.IsMatch(userChoice, "^[1-2]$"))
                {
                    result = true;
                    switch (int.Parse(userChoice))
                    {
                        case 1:
                            language = new English();
                            break;
                        case 2:
                            language = new Russian();
                            break;
                    }
                }
            }
        }

        public void EnterWord()
        {
            string? tmpValue;
            do
            {
                myConsole.Clear();
                language.EnterMainWordMinChars();
                tmpValue = myConsole.ReadMessage();

            } while (!Regex.IsMatch(tmpValue, this.numberRegex));

            this.MinChars = int.Parse(tmpValue);

            do
            {
                myConsole.Clear();
                language.EnterMainWordMaxChars();
                tmpValue = myConsole.ReadMessage();

            } while (!Regex.IsMatch(tmpValue, this.numberRegex));
            // || (this.MinChars < int.Parse(tmpValue))
            this.MaxChars = int.Parse(tmpValue);

            string tmpRegex = language.regex.Replace("+$", "{" + this.MinChars + "," + this.MaxChars + "}$");
            do
            {
                myConsole.Clear();
                language.EnterMainWord(this.MinChars, this.MaxChars);
                this.Word = myConsole.ReadMessage();

            } while (!Regex.IsMatch(this.Word, tmpRegex));
        }
    }
}

