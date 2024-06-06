using System.Text.RegularExpressions;
using Task2.Languages;

namespace Task2
{
    internal class DataCheck
    {
        public bool CheckWithRegex(string str, string regex)
        {
            return Regex.IsMatch(str, regex);
        }

        public bool CheckPlayerWord(string mainWord, string playerWord, ILanguage language)
        {
            bool result = true;
            List<char> mainWordList = mainWord.ToList();
            foreach (char ch in playerWord)
            {
                if (mainWordList.IndexOf(Char.ToLower(ch)) < 0)
                {
                    language.CharIsNotInWord(ch);
                    result = false;
                }
                mainWordList.Remove(ch);
            }

            return result;
        }
    }
}
