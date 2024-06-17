using Task2.Languages;

namespace Task2
{
    public class Game(ConsoleLogger Logger)
    {
        public int PlayerNumber;
        public string Word { get; set; } = "";
        public int MinChars { get; set; }
        public int MaxChars { get; set; }
        public ILanguage Language { get; set; }
        public MyConsole MyConsole { get; set; } = new(Logger);
        private const string NumberRegex = @"^\d+$";
        private const string PlayerNumberRegex = "^[2-5]$";
        public List<string> Words = [];
        public List<Player> Players = [];
        public List<string> ActivePlayers = [];
        public List<Player> AllPlayers = [];
        public Dictionary<string, int> GameStatistic = [];

        public async Task Start()
        {

            try
            {
                WelcomeText();

                PlayerNumber = PlayerNumbers();

                EnterWord();

                //load game statistic form file
                GameStatistic = await FileService.LoadDataAsync();
                this.Language.DataLoaded();

                //add players depends on 'playerNumber' value and set prev score
                for (int i = 1; i <= PlayerNumber; i++)
                {
                    ProcessPlayer(i);
                }

                while (ActivePlayers.Count > 1)
                {
                    for (int i = 0; i < PlayerNumber; i++)
                    {
                        //delete player form 'ActivePlayers' list if player made a mistake
                        if (ActivePlayers.Any(word => word == Players[i].Name))
                        {
                            if (!Players[i].ReadPlayerWord(this)) Language.PlayerIsOut(Players[i].Name);
                        }

                        //finish game if there is only one player in current turn
                        if (ActivePlayers.Count == 1)
                        {
                            Finish();
                            break;
                        }
                    }
                }
            }
            catch
            {
                Language.ErrorMessage("Start");
            }

        }
        
        public void WelcomeText()
        {
            Language.WelcomeText();
        }

        public void ProcessPlayer(int i)
        {
            Player Player = new(i);
            Player.SetName(this);
            this.ActivePlayers.Add(Player.Name);
            Players.Add(Player);
            this.MyConsole.Clear();
        }

        public async void Finish()
        {

            try
            {
                foreach (Player player in Players)
                {
                    if (GameStatistic.ContainsKey(player.Name))
                    {
                        GameStatistic[player.Name] += player.Score;
                    }
                    else
                    {
                        GameStatistic.Add(player.Name, player.Score);
                    }
                }

                await FileService.SaveDataAsync(GameStatistic);
                ShowWinner();
            }
            catch
            {
                Language.ErrorMessage("Fihish");
            }
        }

        public void ShowWinner()
        {
            int maxScore = 0;

            try
            {
                List<Player> winnerPlayers = [];

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
                    Language.ShowWinner(winnerPlayers);
                }
                else
                {
                    Language.ShowWinner(winnerPlayers[0]);
                }
            }
            catch
            {
                Language.ErrorMessage("ShowWinner");
            }

        }

        public ILanguage ChooseLanguage()
        {
            bool result = false;
            
            try
            {
                while (!result)
                {
                    MyConsole.WriteMessage("Please, choose language:");
                    MyConsole.WriteMessage("1. English");
                    MyConsole.WriteMessage("2. Русский");

                    string? userChoice = MyConsole.ReadMessage(this);

                    if (DataCheck.CheckWithRegex(userChoice, "^[1-2]$"))
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
            }
            catch
            {
                Language.ErrorMessage("ChooseLanguage");
            }
            return Language;
        }

        public int PlayerNumbers()
        {
            string? strPlayerNumbers;

            do
            {
                Language.EnterPlayerNumbers();
                strPlayerNumbers = MyConsole.ReadMessage(this);

            } while (DataCheck.CheckWithRegex(strPlayerNumbers, PlayerNumberRegex) != true);

            MyConsole.Clear();

            return int.Parse(strPlayerNumbers);
        }

        public void EnterWord()
        {
            string? tmpValue;

            try
            {
                do
                {
                    Language.EnterMainWordMinChars();
                    tmpValue = MyConsole.ReadMessage(this);

                } while (DataCheck.CheckWithRegex(tmpValue, NumberRegex) != true);

                MyConsole.Clear();
                this.MinChars = int.Parse(tmpValue);

                bool result = false;
                while (!result)
                {
                    Language.EnterMainWordMaxChars(this.MinChars);
                    tmpValue = MyConsole.ReadMessage(this);

                    result = DataCheck.CheckWithRegex(tmpValue, NumberRegex);

                    if (result) result = (this.MinChars < int.Parse(tmpValue));
                }

                MyConsole.Clear();

                this.MaxChars = int.Parse(tmpValue);

                string tmpRegex = Language.Regex.Replace("+$", "{" + this.MinChars + "," + this.MaxChars + "}$");
                do
                {
                    Language.EnterMainWord(this.MinChars, this.MaxChars);
                    this.Word = MyConsole.ReadMessage(this);

                } while (DataCheck.CheckWithRegex(this.Word, tmpRegex) != true);

                MyConsole.Clear();
            }
            catch
            {
                Language.ErrorMessage("EnterWord");
            }

        }

        public void ShowWords()
        {
            try
            {
                foreach (Player player in Players)
                {
                    Language.ShowWords(player);
                }
            }
            catch
            {
                Language.ErrorMessage("ShowWords");
            }
        }

        public void ShowScore()
        {
            try
            {
                foreach (Player player in Players)
                {
                    Language.ShowScore(player);
                }
            }
            catch
            {
                Language.ErrorMessage("ShowScore");
            }
        }

        public void ShowTotalScore()
        {
            try
            {
                Language.ShowTotalScore(GameStatistic);
            }
            catch
            {
                Language.ErrorMessage("ShowScore");
            }
        }
    }
}

