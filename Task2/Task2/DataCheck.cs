using System.Text.RegularExpressions;

namespace Task2
{
    internal class DataCheck
    {
        //check if incomming value matches condition
        public bool CheckWithRegex(string str, string regex)
        {
            return Regex.IsMatch(str, regex);
        }

        //return list of chars that are not in the main word
        public List<char> CheckPlayerWord(string mainWord, string playerWord)
        {
            List<char> mainWordList = mainWord.ToList();
            List<char> chars = new List<char>();

            foreach (char ch in playerWord)
            {
                if (mainWordList.IndexOf(Char.ToLower(ch)) < 0)
                {
                    chars.Add(ch);
                }
                mainWordList.Remove(ch);
            }

            return chars;
        }

        //check if the word was used before by other players
        public bool wordIsInList(string word, List<string> words)
        {
            return (words.Any(w => w.ToLower() == word.ToLower())) ? true : false;
        }
    }
}
