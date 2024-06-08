using System.Numerics;

namespace Task2.Languages
{
    internal class English : ILanguage
    {
        public string Name { get { return "English"; } }
        public MyConsole myConsole = new MyConsole();
        public string regex { get { return @"^[a-zA-Z]+$"; } }

        public void WelcomeText()
        {
            myConsole.Clear();
            myConsole.WriteMessage("Welcome to \"Word\" game!!!\n");
            myConsole.WriteMessage("Please use the following commands to get some additional information during the game:\n");
            myConsole.WriteMessage("/help - to see help information,");
            myConsole.WriteMessage("/show-words - to see all words in the game,");
            myConsole.WriteMessage("/score - to see score of current players,");
            myConsole.WriteMessage("/total-score - to see all players score.\n");
        }
        public void PlayerNumbers()
        {
            myConsole.WriteMessage($"Please, enter player numbers from 2 to 5:");
        }
        public void EnterPlayerName(int number)
        {
            myConsole.WriteMessage($"Please, enter name for player {number}:");
        }
        public void EnterMainWordMinChars()
        {
            myConsole.WriteMessage("Please, enter min chars for main word:");
        }
        public void EnterMainWordMaxChars()
        {
            myConsole.WriteMessage("Please, enter max chars for main word:");
        }
        public void EnterMainWord(int minChars, int maxChars)
        {
            myConsole.WriteMessage($"Please, enter main word between {minChars} - {maxChars} chars:");
        }
        public void EnterPlayerWord(string name, string word)
        {
            myConsole.WriteMessage($"{name}, your turn with '{word}' word:");
        }
        public void WordIsInList(string word)
        {
            myConsole.WriteMessage($"'{word}' was used before.");
        }
        public void ShowWinner(Player player)
        {
            myConsole.WriteMessage($"{player.Name} is the winner with {player.Score} points! Game is over.");
        }
        public void ShowWinner(List<Player> players)
        {
            myConsole.WriteMessage("It's a draw.");
            for (int i = 0; i < players.Count; i++)
            {
                myConsole.WriteMessage($"{players[i].Name} is the winner with {players[i].Score} points!");
            }
            myConsole.WriteMessage("Game is over.");
        }
        public void CharIsNotInWord(char ch)
        {
            myConsole.WriteMessage($"There is no '{ch}' char in the main word.");
        }
        public void ShowWords(Player player)
        {
            myConsole.WriteMessage($"Words of {player.Name} player:");
            for (int i = 0; i < player.Words.Count; i++)
            {
                myConsole.WriteMessage(player.Words[i]);
            }
        }
        public void ShowScore(Player player)
        {
            myConsole.WriteMessage($"{player.Name} has {player.Score} points.");
        }
    }
}
