namespace Task2.Languages
{
    internal class Russian : ILanguage
    {
        public string Name { get { return "Русский"; } }
        public MyConsole myConsole = new MyConsole();
        public string regex { get { return @"^[а-яёА-ЯЁ]+$"; } }

        public void WelcomeText()
        {
            myConsole.Clear();
            myConsole.WriteMessage("Добро пожаловать в игру \"Слова\"!!!\n");
            myConsole.WriteMessage("Пожалуйста, используйте следующие команды для получения дополнительной информации во время игры:\n");
            myConsole.WriteMessage("/помощь - показать помощь,");
            myConsole.WriteMessage("/показать-слова - показать все слова текущей игры,");
            myConsole.WriteMessage("/очки - показать очки текущих игроков,");
            myConsole.WriteMessage("/общие-очки - показать очки всех игроков.\n");
        }
        public void PlayerNumbers()
        {
            myConsole.WriteMessage($"Пожалуйства, введите количество игроков от 2 до 5:");
        }
        public void EnterPlayerName(int number)
        {
            myConsole.WriteMessage($"Пожалуйста, введите имя для игрока {number}:");
        }
        public void EnterMainWordMinChars()
        {
            myConsole.WriteMessage("Пожалуйста, введите минимальное количество букв для главного слова:");
        }
        public void EnterMainWordMaxChars(int number)
        {
            myConsole.WriteMessage($"Пожалуйста, введите максимальное количество букв для главного слова больше чем {number}:");
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
        public void ShowWinner(Player player)
        {
            myConsole.WriteMessage($"{player.Name} победитель с {player.Score} очками! Игра окончена.");
        }
        public void ShowWinner(List<Player> players)
        {
            myConsole.WriteMessage("Ничья.");
            try
            {
                for (int i = 0; i < players.Count; i++)
                {
                    myConsole.WriteMessage($"{players[i].Name} победитель с {players[i].Score} очками!");
                }
            }
            catch
            {
                myConsole.WriteMessage("Ошибка в методе 'ShowWinner'.");
            }
            myConsole.WriteMessage("Игра окончена.");
        }
        public void CharIsNotInWord(char ch)
        {
            myConsole.WriteMessage($"Буквы '{ch}' нет в главном слове.");
        }
        public void ShowWords(Player player)
        {
            myConsole.WriteMessage($"Слова игрока {player.Name}:");
            try
            {
                for (int i = 0; i < player.Words.Count; i++)
                {
                    myConsole.WriteMessage(player.Words[i]);
                }
            }
            catch
            {
                myConsole.WriteMessage("Ошибка в методе 'ShowWords'.");
            }
        }
        public void ShowScore(Player player)
        {
            myConsole.WriteMessage($"{player.Name} имеет {player.Score} очков.");
        }
        public void PlayerIsOut(string name)
        {
            myConsole.WriteMessage($"{name} выбывает из игры.");
        }
        public void DataSaved()
        {
            myConsole.WriteMessage("Данные сохранены в файл.");
        }
        public void DataLoaded()
        {
            myConsole.WriteMessage("Данные загружены из файла.");
        }
        public void ErrorMessage(string method)
        {
            myConsole.WriteMessage($"Ошибка в методе '{method}'.");
        }
    }
}
