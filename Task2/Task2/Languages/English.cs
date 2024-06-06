using System.Text.RegularExpressions;

namespace Task2.Languages
{
    internal class English : ILanguage
    {
        public string Name { get { return "English"; } }
        public MyConsole myConsole = new MyConsole();
        public string regex { get { return @"^[a-zA-Z]+$"; } }

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
        public void GetWinner(string playerName)
        {
            myConsole.WriteMessage($"{playerName} is the winner! Game is over.");
        }
        public void CharIsNotInWord(char ch)
        {
            myConsole.WriteMessage($"There is no '{ch}' char in the main word.");
        }
    }
}
