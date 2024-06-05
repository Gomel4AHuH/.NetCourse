using System.Numerics;
using System.Text.RegularExpressions;

namespace Task2
{
    internal struct Player
    {
        public string Name { get; set; }
        
        public Player(ILanguage language, MyConsole myConsole, int number)
        {
            do
            {
                //myConsole.Clear();
                Console.WriteLine(language.regex);
                language.EnterPlayerName(number);
                this.Name = myConsole.ReadMessage();
                Console.WriteLine(this.Name);
            } while (!Regex.IsMatch(this.Name, language.regex));
        }

        public void ReturnPlayerName()
        {
            Console.WriteLine(this.Name);
        }

        public bool ReadPlayerWord(ILanguage language, MyConsole myConsole, string mainWord, List<string> playerWords)
        {
            bool result = true;
            string playerWord;
            List<char> mainWordList = mainWord.ToList();
           
            language.EnterPlayerWord(this.Name, mainWord);
            playerWord = myConsole.ReadMessage();

            foreach (char ch in playerWord)
            {
                if (mainWordList.IndexOf(Char.ToLower(ch)) < 0)
                {
                    //printMessage($"'{ch}'" + arrNotChar[choosedLanguage - 1]);
                    language.CharIsNotInWord(ch);
                    result = false;
                }
                mainWordList.Remove(ch);
            }
            playerWords.Add(playerWord);

            return result;
        }
    }
}

