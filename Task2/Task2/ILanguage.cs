namespace Task2
{
    internal interface ILanguage
    {
        string Name { get; }
        string regex { get; }
        void EnterPlayerName(int number);
        void EnterMainWord();
        void EnterPlayerWord(string word);
        void GetWinner(string name);
        void CharIsNotInWord(char ch);
    }
}
