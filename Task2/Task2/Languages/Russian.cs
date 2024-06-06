using System.Text.RegularExpressions;

namespace Task2.Languages
{
    internal class Russian : ILanguage
    {
        public string Name { get { return "Русский"; } }
        public MyConsole myConsole = new MyConsole();
        public string regex { get { return @"^[а-яёА-ЯЁ]+$"; } }

        public void EnterPlayerName(int number)
        {
            myConsole.WriteMessage($"Пожалуйста, введите имя для игрока {number}:");
        }
        public void EnterMainWordMinChars()
        {
            myConsole.WriteMessage("Пожалуйста, введите минимальное количество букв для главного слова:");
        }
        public void EnterMainWordMaxChars()
        {
            myConsole.WriteMessage("Пожалуйста, введите максимальное количество букв для главного слова:");
        }
        public void EnterMainWord(int minChars, int maxChars)
        {
            myConsole.WriteMessage($"Пожалуйста, введите главное слово размером {minChars} - {maxChars} букв:");
        }
        public void EnterPlayerWord(string name, string word)
        {
            myConsole.WriteMessage($"{name}, ваш ход со словом '{word}':");
        }
        public void WordIsInList(string word)
        {
            myConsole.WriteMessage($"'{word}' было использовано раньше.");
        }
        public void GetWinner(string name)
        {
            myConsole.WriteMessage($"{name} победитель! Игра окончена.");
        }
        public void CharIsNotInWord(char ch)
        {
            myConsole.WriteMessage($"Буквы '{ch}' нет в главном слове.");
        }
    }
}
