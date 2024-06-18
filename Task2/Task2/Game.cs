using System.Xml.Linq;
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
        public List<string> words = [];
        public List<Player> players = [];
        public List<string> activePlayers = [];
        public List<Player> allPlayers = [];
        public Dictionary<string, int> gameStatistic = [];

        public async Task StartAsync()
        {

            try
            {
                WelcomeText();

                PlayerNumber = PlayerNumbers();

                EnterWord();

                //load game statistic form file
                gameStatistic = await FileService.LoadDataAsync();
                this.Language.DataLoaded();

                //add players depends on 'playerNumber' value and set prev score
                for (int i = 1; i <= PlayerNumber; i++)
                {
                    ProcessPlayer(i);
                }

                while (activePlayers.Count > 1)
                {
                    for (int i = 0; i < PlayerNumber; i++)
                    {
                        //delete player form 'ActivePlayers' list if player made a mistake
                        if (activePlayers.Any(word => word == players[i].Name))
                        {
                            if (!ReadPlayerWord(players[i])) Language.PlayerIsOut(players[i].Name);
                        }

                        //finish game if there is only one player in current turn
                        if (activePlayers.Count == 1)
                        {
                            FinishAsync();
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
            this.activePlayers.Add(Player.Name);
            players.Add(Player);
            this.MyConsole.Clear();
        }

        public async void FinishAsync()
        {

            try
            {
                foreach (Player player in players)
                {
                    if (gameStatistic.ContainsKey(player.Name))
                    {
                        gameStatistic[player.Name] += player.Score;
                    }
                    else
                    {
                        gameStatistic.Add(player.Name, player.Score);
                    }
                }

                await FileService.SaveDataAsync(gameStatistic);
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
                maxScore = players.Max(player => player.Score);
                List<Player> winnerPlayers = players.FindAll(player => player.Score == maxScore);

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

            } while (!DataCheck.CheckWithRegex(strPlayerNumbers, PlayerNumberRegex));

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

                } while (!DataCheck.CheckWithRegex(tmpValue, NumberRegex));

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

                } while (!DataCheck.CheckWithRegex(this.Word, tmpRegex));

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
                foreach (Player player in players)
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
                foreach (Player player in players)
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
                Language.ShowTotalScore(gameStatistic);
            }
            catch
            {
                Language.ErrorMessage("ShowScore");
            }
        }

        public bool ReadPlayerWord(Player player)
        {
            bool result = true;
            string playerWord;
            List<char> mainWordList = [.. Word.ToLower()];

            try
            {
                do
                {
                    Language.EnterPlayerWord(player.Name, Word);
                    playerWord = MyConsole.ReadMessage(this);

                } while (!DataCheck.CheckWithRegex(playerWord, Language.Regex));


                List<char> chars = DataCheck.CheckPlayerWord(Word, playerWord);

                if (chars.Count > 0)
                {
                    for (int i = 0; i < chars.Count; i++)
                    {
                        Language.CharIsNotInWord(chars[i]);
                    }
                    result = false;
                }

                if (DataCheck.WordIsInList(playerWord, words))
                {
                    Language.WordIsInList(playerWord);
                    result = false;
                }

                if (result)
                {
                    player.Words.Add(playerWord);
                    words.Add(playerWord);
                    player.Score += playerWord.Length;
                    MyConsole.Clear();
                }
                else
                {
                    activePlayers.Remove(player.Name);
                }
            }
            catch
            {
                Language.ErrorMessage("ReadPlayerWord");
            }
            return result;
        }
    }
}

