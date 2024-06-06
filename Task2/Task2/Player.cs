using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Task2.Languages;

namespace Task2
{
    internal struct Player
    {
        public string Name { get; set; }        
        public int Score = 0;
        public bool IsWinner;

        public Player(ILanguage language, MyConsole myConsole, int number)
        {
            do
            {
                myConsole.Clear();
                language.EnterPlayerName(number);
                this.Name = myConsole.ReadMessage();

            } while (new DataCheck().CheckWithRegex(this.Name, language.regex) != true);
            //} while (!Regex.IsMatch(this.Name, language.regex));
        }

        public void ReturnPlayerName()
        {
            Console.WriteLine(this.Name);
        }

        public bool ReadPlayerWord(ILanguage language, MyConsole myConsole, string mainWord, List<string> Words)
        {
            bool result = true;
            string playerWord;
            List<char> mainWordList = mainWord.ToList();
            this.IsWinner = false;
           
            myConsole.Clear();
            language.EnterPlayerWord(this.Name, mainWord);
            playerWord = myConsole.ReadMessage();

            foreach (char ch in playerWord)
            {
                if (mainWordList.IndexOf(Char.ToLower(ch)) < 0)
                {
                    language.CharIsNotInWord(ch);
                    result = false;
                }
                mainWordList.Remove(ch);
            }
            
            if (Words.Any(word => word.ToLower() == playerWord.ToLower()))
            {
                language.WordIsInList(playerWord);
                result = false;
            }
                        
            if (result)
            {
                Words.Add(playerWord);
                this.Score += playerWord.Length;
                this.IsWinner = true;
            }

            return result;
        }
    }
}

