namespace Task2.Languages
{
    internal interface ILanguage
    {
        string Name { get; }
        string regex { get; }
        void EnterPlayerName(int number);
        void EnterMainWordMinChars();
        void EnterMainWordMaxChars();
        void EnterMainWord(int minChars, int maxChars);
        void EnterPlayerWord(string name, string word);
        void WordIsInList(string word);
        void GetWinner(string name);
        void CharIsNotInWord(char ch);
    }
}
