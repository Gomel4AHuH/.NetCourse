namespace Task2
{
    internal class Russian : ILanguage
    {
        public string Name { get { return "Русский"; } }
        public MyConsole myConsole = new MyConsole();
        public string regex { get { return "^[а-яёА-ЯЁ]"; } }
        public void EnterPlayerName(int number)
        {
            myConsole.WriteMessage($"Пожалуйста, введите имя для игрока {number}");
        }
        public void EnterMainWord()
        {
            myConsole.WriteMessage("Пожалуйста, введите главное слово");
        }
        public void EnterPlayerWord(string name)
        {
            myConsole.WriteMessage($"{name}, ваш ход");
        }
        public void GetWinner(string name)
        {
            myConsole.WriteMessage($"{name} победитель!");
        }
        public void CharIsNotInWord(char ch)
        {
            myConsole.WriteMessage($"Буквы {ch} нет в главном слове");
        }
    }
}
