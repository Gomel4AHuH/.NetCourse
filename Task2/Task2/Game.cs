using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Task2.Languages;

namespace Task2
{
    public class Game
    {
        public Game game { get; set; }

        public int playerNumber;
        public string Word { get; set; }
        public int MinChars { get; set; }
        public int MaxChars { get; set; }
        public ILanguage language { get; set; }
        public MyConsole myConsole { get; set; }
        private string numberRegex = @"^\d+$";
        public List<string> Words = new List<string>();

        public List<Player> Players = new List<Player>();
        public List<string> ActivePlayers = new List<string>();

        public Game()
        {
            this.myConsole = new MyConsole();
            this.language = ChooseLanguage();            
        }

        public void Start()
        {

            //string json = JsonSerializer.Serialize(Players);
            //myConsole.WriteMessage("json " + json);

            //new File().SaveData(Players[0]);

            //File file = new File();
            //await file.SaveDataPlayerAsync();
            
            WelcomeText();

            playerNumber = PlayerNumbers();

            EnterWord();

            for (int i = 1; i <= playerNumber; i++)
            {
                Players.Add(new Player(this, i));
            }

            while (ActivePlayers.Count > 1)
            {
                for (int i = 0; i < playerNumber; i++)
                {
                    if (ActivePlayers.Count == 1)
                    {
                        Finish();
                    }
                    if (ActivePlayers.Any(word => word == Players[i].Name))
                    {
                        Players[i].ReadPlayerWord(this);
                    }
                }
            }
        }

        public void WelcomeText()
        {
            language.WelcomeText();
        }

        public async void Finish()
        {
            await new File(new ConsoleLogger()).SaveDataAsync(Players);
            ShowWinner();
        }

        public void ShowWinner()
        {
            int maxScore = 0;
            List<Player> winnerPlayers = new List<Player>();

            foreach (Player player in Players)
            {
                if (player.Score > maxScore)
                {
                    maxScore = player.Score;
                }
            }
            foreach (Player player in Players)
            {
                if (player.Score == maxScore)
                {
                    winnerPlayers.Add(player);
                }
            }

            if (winnerPlayers.Count > 1)
            {
                language.ShowWinner(winnerPlayers);
            }
            else
            {
                language.ShowWinner(winnerPlayers[0]);
            }
            //winnerPlayers.Count > 1 ? Console.WriteLine("several") : Console.WriteLine("the one");
            //language.ShowWinner((winnerPlayers.Count > 1) ? winnerPlayers : winnerPlayers[0]);
            //(winnerPlayers.Count == 1) ? language.ShowWinner(winnerPlayers[0]) : language.ShowWinner(winnerPlayers);
        }

        public ILanguage ChooseLanguage()
        {
            bool result = false;
            while (!result)
            {
                myConsole.WriteMessage("Please, choose language:");
                myConsole.WriteMessage("1. English");
                myConsole.WriteMessage("2. Русский");

                string? userChoice = myConsole.ReadMessage(this);

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

            return language;
        }

        public int PlayerNumbers()
        {
            string? playerNumbers;
            do
            {
                language.PlayerNumbers();
                playerNumbers = myConsole.ReadMessage(game);

            } while (new DataCheck().CheckWithRegex(playerNumbers, "^[2-5]$") != true);

            myConsole.Clear();
            return int.Parse(playerNumbers);
        }

        public void EnterWord()
        {
            string? tmpValue;
            do
            {
                language.EnterMainWordMinChars();
                tmpValue = myConsole.ReadMessage(game);

            } while (new DataCheck().CheckWithRegex(tmpValue, this.numberRegex) != true);

            myConsole.Clear();
            this.MinChars = int.Parse(tmpValue);

            do
            {
                language.EnterMainWordMaxChars();
                tmpValue = myConsole.ReadMessage(game);

            } while (new DataCheck().CheckWithRegex(tmpValue, this.numberRegex) != true);

            myConsole.Clear();
            // || (this.MinChars < int.Parse(tmpValue))
            this.MaxChars = int.Parse(tmpValue);

            string tmpRegex = language.regex.Replace("+$", "{" + this.MinChars + "," + this.MaxChars + "}$");
            do
            {
                language.EnterMainWord(this.MinChars, this.MaxChars);
                this.Word = myConsole.ReadMessage(game);

            } while (new DataCheck().CheckWithRegex(this.Word, tmpRegex) != true);

            myConsole.Clear();
        }

        public void ShowWords()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                language.ShowWords(Players[i]);
            }
        }

        public void ShowScore()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                language.ShowScore(Players[i]);
            }
        }
    }
}

