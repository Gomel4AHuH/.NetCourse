namespace Task2.Languages
{
    public interface ILanguage
    {
        string Name { get; }
        string regex { get; }

        void WelcomeText();
        void PlayerNumbers();
        void EnterPlayerName(int number);
        void EnterMainWordMinChars();
        void EnterMainWordMaxChars(int nubmer);
        void EnterMainWord(int minChars, int maxChars);
        void EnterPlayerWord(string name, string word);
        void WordIsInList(string word);
        void ShowWinner(Player player);        
        void ShowWinner(List<Player> players);
        void CharIsNotInWord(char ch);
        void ShowWords(Player player);
        void ShowScore(Player player);
        void PlayerIsOut(string name);
        void DataSaved();
        void DataLoaded();
        void ErrorMessage(string method);
    }
}
