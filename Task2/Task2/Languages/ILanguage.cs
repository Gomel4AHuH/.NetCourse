namespace Task2.Languages
{
    public interface ILanguage
    {
        string Name { get; }
        string Regex { get; }

        void WelcomeText();
        void EnterPlayerNumbers();
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
        void ShowTotalScore(Dictionary<string, int> GameStatistic);
        void PlayerIsOut(string name);
        void DataSaved();
        void DataLoaded();
        void ErrorMessage(string method);
    }
}
