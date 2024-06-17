namespace Task2.Languages
{
    internal class English : ILanguage
    {
        public string Name { get { return "English"; } }
        public static ConsoleLogger Logger = new();
        public MyConsole MyConsole = new(Logger);
        public string Regex { get { return @"^[a-zA-Z]+$"; } }

        public void WelcomeText()
        {
            MyConsole.Clear();
            MyConsole.WriteMessage("Welcome to \"Word\" game!!!\n");
            MyConsole.WriteMessage("Please use the following commands to get some additional information during the game:\n");
            MyConsole.WriteMessage("/help - to see help information,");
            MyConsole.WriteMessage("/show-words - to see all words in the game,");
            MyConsole.WriteMessage("/score - to see score of current players,");
            MyConsole.WriteMessage("/total-score - to see all players score.\n");
        }
        public void EnterPlayerNumbers()
        {
            MyConsole.WriteMessage($"Please, enter player numbers from 2 to 5:");
        }
        public void EnterPlayerName(int number)
        {
            MyConsole.WriteMessage($"Please, enter name for player {number}:");
        }
        public void EnterMainWordMinChars()
        {
            MyConsole.WriteMessage("Please, enter min chars for main word:");
        }
        public void EnterMainWordMaxChars(int number)
        {
            MyConsole.WriteMessage($"Please, enter max chars for main word more than {number}:");
        }
        public void EnterMainWord(int minChars, int maxChars)
        {
            MyConsole.WriteMessage($"Please, enter main word between {minChars} - {maxChars} chars:");
        }
        public void EnterPlayerWord(string name, string word)
        {
            MyConsole.WriteMessage($"{name}, your turn with '{word}' word:");
        }
        public void WordIsInList(string word)
        {
            MyConsole.WriteMessage($"'{word}' was used before.");
        }
        public void ShowWinner(Player player)
        {
            MyConsole.WriteMessage($"{player.Name} is the winner with {player.Score} points! Game is over.");
        }
        public void ShowWinner(List<Player> players)
        {
            MyConsole.WriteMessage("It's a draw.");

            try
            {
                foreach (Player player in players)
                {
                    MyConsole.WriteMessage($"{player.Name} is the winner with {player.Score} points!");
                }
            }
            catch
            {
                ErrorMessage("ShowWinner");
            }
            MyConsole.WriteMessage("Game is over.");
        }
        public void CharIsNotInWord(char ch)
        {
            MyConsole.WriteMessage($"There is no '{ch}' char in the main word.");
        }
        public void ShowWords(Player player)
        {
            MyConsole.WriteMessage($"Words of {player.Name} player:");
            try
            {
                for (int i = 0; i < player.Words.Count; i++)
                {
                    MyConsole.WriteMessage(player.Words[i]);
                }
            }
            catch
            {
                ErrorMessage("ShowWords");
            }
        }
        public void ShowScore(Player player)
        {
            MyConsole.WriteMessage($"{player.Name} has {player.Score} points.");
        }
        public void ShowTotalScore(Dictionary<string, int> gameStatistic)
        {
            foreach (var player in gameStatistic)
            {
                MyConsole.WriteMessage($"{player.Key} has {player.Value} points.");
            }
        }

        public void PlayerIsOut(string name)
        {
            MyConsole.WriteMessage($"{name} is out of the game.");
        }
        public void DataSaved()
        {
            MyConsole.WriteMessage("Data has been saved to file.");
        }
        public void DataLoaded()
        {
            MyConsole.WriteMessage("Data has been loaded from file.");
        }
        public void ErrorMessage(string method)
        {
            MyConsole.WriteMessage($"Error in '{method}' method.");
        }

    }
}
