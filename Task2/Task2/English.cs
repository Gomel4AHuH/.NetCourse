using System.Text.RegularExpressions;

namespace Task2
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
        public void EnterMainWord(int minChars, int MaxChars)
        {
            myConsole.WriteMessage($"Please, enter main word between {minChars} - {MaxChars} chars:");
        }
        public void EnterPlayerWord(string name, string word)
        {
            myConsole.WriteMessage($"{name}, your trun with '{word}' word:");
        }
        public void GetWinner(string playerName)
        {
            myConsole.WriteMessage($"{playerName} is the winner!");
        }
        public void CharIsNotInWord(char ch)
        {
            myConsole.WriteMessage($"There is no {ch} char in the main word.");
        }
    }
}
