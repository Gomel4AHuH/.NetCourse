namespace Task2
{
    internal interface ILanguage
    {
        string Name { get; }
        string regex { get; }
        void EnterPlayerName(int number);
        void EnterMainWordMinChars();
        void EnterMainWordMaxChars();
        void EnterMainWord(int minChars, int MaxChars);
        void EnterPlayerWord(string name, string word);
        void GetWinner(string name);
        void CharIsNotInWord(char ch);
    }
}
