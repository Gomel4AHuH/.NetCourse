namespace Task2
{ 
    internal class English : ILanguage
    {
        public string Name { get { return "English"; } }
        public MyConsole myConsole = new MyConsole();
        public string regex { get { return "^[a-zA-Z]"; } }

        public void EnterPlayerName(int number)
        {
            myConsole.WriteMessage($"Please, enter name for player {number}");
        }

        public void EnterMainWord()
        {
            myConsole.WriteMessage("Please, enter main word");
        }

        public void EnterPlayerWord(string name)
        {
            myConsole.WriteMessage($"{name}, your trun");
        }

        public void GetWinner(string name)
        {
            myConsole.WriteMessage($"{name} is the winner!");
        }
        public void CharIsNotInWord(char ch)
        {
            myConsole.WriteMessage($"There is no {ch} char in the main word");
        }
    }
}
