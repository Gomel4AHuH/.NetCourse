namespace Task2.Languages
{
    internal class Russian : ILanguage
    {
        public string Name { get { return "Русский"; } }
        public static ConsoleLogger Logger = new();
        public MyConsole MyConsole = new(Logger);

        public string Regex { get { return @"^[а-яёА-ЯЁ]+$"; } }

        public void WelcomeText()
        {
            MyConsole.Clear();
            MyConsole.WriteMessage("Добро пожаловать в игру \"Слова\"!!!\n");
            MyConsole.WriteMessage("Пожалуйста, используйте следующие команды для получения дополнительной информации во время игры:\n");
            MyConsole.WriteMessage("/помощь - показать помощь,");
            MyConsole.WriteMessage("/показать-слова - показать все слова текущей игры,");
            MyConsole.WriteMessage("/очки - показать очки текущих игроков,");
            MyConsole.WriteMessage("/общие-очки - показать очки всех игроков.\n");
        }
        public void EnterPlayerNumbers()
        {
            MyConsole.WriteMessage($"Пожалуйства, введите количество игроков от 2 до 5:");
        }
        public void EnterPlayerName(int number)
        {
            MyConsole.WriteMessage($"Пожалуйста, введите имя для игрока {number}:");
        }
        public void EnterMainWordMinChars()
        {
            MyConsole.WriteMessage("Пожалуйста, введите минимальное количество букв для главного слова:");
        }
        public void EnterMainWordMaxChars(int number)
        {
            MyConsole.WriteMessage($"Пожалуйста, введите максимальное количество букв для главного слова больше чем {number}:");
        }
        public void EnterMainWord(int minChars, int maxChars)
        {
            MyConsole.WriteMessage($"Пожалуйста, введите главное слово размером {minChars} - {maxChars} букв:");
        }
        public void EnterPlayerWord(string name, string word)
        {
            MyConsole.WriteMessage($"{name}, ваш ход со словом '{word}':");
        }
        public void WordIsInList(string word)
        {
            MyConsole.WriteMessage($"'{word}' было использовано раньше.");
        }
        public void ShowWinner(Player player)
        {
            MyConsole.WriteMessage($"{player.Name} победитель с {player.Score} очками! Игра окончена.");
        }
        public void ShowWinner(List<Player> players)
        {
            MyConsole.WriteMessage("Ничья.");
            try
            {
                for (int i = 0; i < players.Count; i++)
                {
                    MyConsole.WriteMessage($"{players[i].Name} победитель с {players[i].Score} очками!");
                }
            }
            catch
            {
                MyConsole.WriteMessage("Ошибка в методе 'ShowWinner'.");
            }
            MyConsole.WriteMessage("Игра окончена.");
        }
        public void CharIsNotInWord(char ch)
        {
            MyConsole.WriteMessage($"Буквы '{ch}' нет в главном слове.");
        }
        public void ShowWords(Player player)
        {
            MyConsole.WriteMessage($"Слова игрока {player.Name}:");
            try
            {
                for (int i = 0; i < player.Words.Count; i++)
                {
                    MyConsole.WriteMessage(player.Words[i]);
                }
            }
            catch
            {
                MyConsole.WriteMessage("Ошибка в методе 'ShowWords'.");
            }
        }
        public void ShowScore(Player player)
        {
            MyConsole.WriteMessage($"{player.Name} имеет {player.Score} очков.");
        }
        public void ShowTotalScore(Dictionary<string, int> gameStatistic)
        {
            foreach (var player in gameStatistic)
            {
                MyConsole.WriteMessage($"{player.Key} имеет {player.Value} очков.");
            }
        }
        public void PlayerIsOut(string name)
        {
            MyConsole.WriteMessage($"{name} выбывает из игры.");
        }
        public void DataSaved()
        {
            MyConsole.WriteMessage("Данные сохранены в файл.");
        }
        public void DataLoaded()
        {
            MyConsole.WriteMessage("Данные загружены из файла.");
        }
        public void ErrorMessage(string method)
        {
            MyConsole.WriteMessage($"Ошибка в методе '{method}'.");
        }
    }
}
