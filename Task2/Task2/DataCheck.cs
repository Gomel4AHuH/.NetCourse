using System.Text.RegularExpressions;

namespace Task2
{
    internal class DataCheck
    {
        public bool CheckWithRegex(string str, Regex regex)
        {
            return regex.IsMatch(str);
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
