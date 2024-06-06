using System.Diagnostics;
using System.Text.RegularExpressions;
using Task2.Languages;

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
        public List<string> Words = new List<string>();
        //public string winnerName;
        public List<Player> Players = new List<Player>();

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

            Players.Add(player1);
            Players.Add(player2);

            while (true)
            {
                if (!player1.ReadPlayerWord(language, myConsole, Word, Words)) break;
                myConsole.WriteMessage(player1.Score.ToString());
                myConsole.ReadMessage();
                if (!player2.ReadPlayerWord(language, myConsole, Word, Words)) break;
                myConsole.WriteMessage(player2.Score.ToString());
                myConsole.ReadMessage();
            }

            //this.winnerName = player1.IsWinner ? player1.Name : player2.Name;
        }

        public void Finish()
        {
            string tmpWinnerName = "";
            for (int i = 0; i < this.Players.Count; i++)
            {
                if (Players[i].IsWinner && tmpWinnerName != "") tmpWinnerName = Players[i].Name;
            }
            language.GetWinner(tmpWinnerName);
            myConsole.WriteMessage("Список слов");
            for (int i = 0; i < Words.Count; i++)
            {
                myConsole.WriteMessage(Words[i]);
            }
            myConsole.WriteMessage("Очки игроков");
            for (int i = 0; i < this.Players.Count; i++)
            {
                myConsole.WriteMessage(Players[i].Name + ": " + Players[i].Score.ToString());
            }

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

                //if (Regex.IsMatch(userChoice, "^[1-2]$"))
                if (new DataCheck().CheckWithRegex(userChoice, "^[1-2]$"))
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

            } while (new DataCheck().CheckWithRegex(tmpValue, this.numberRegex) != true);
            //} while (!Regex.IsMatch(tmpValue, this.numberRegex));

            this.MinChars = int.Parse(tmpValue);

            do
            {
                myConsole.Clear();
                language.EnterMainWordMaxChars();
                tmpValue = myConsole.ReadMessage();

            } while (new DataCheck().CheckWithRegex(tmpValue, this.numberRegex) != true);
            //} while (!Regex.IsMatch(tmpValue, this.numberRegex));

            // || (this.MinChars < int.Parse(tmpValue))
            this.MaxChars = int.Parse(tmpValue);

            string tmpRegex = language.regex.Replace("+$", "{" + this.MinChars + "," + this.MaxChars + "}$");
            do
            {
                myConsole.Clear();
                language.EnterMainWord(this.MinChars, this.MaxChars);
                this.Word = myConsole.ReadMessage();

            } while (new DataCheck().CheckWithRegex(this.Word, tmpRegex) != true);
            //} while (!Regex.IsMatch(this.Word, tmpRegex));
        }
    }
}

