namespace Task2
{
    public class Player(int number)
    {
        public string Name { get; set; } = "";
        public int Number { get; set; } = number;
        public int Score { get; set; } = 0;
        public List<string> Words = [];
        public DataCheck dataCheck = new();

        public void SetName(Game Game)
        {
            do
            {
                Game.Language.EnterPlayerName(Number);
                Name = Game.MyConsole.ReadMessage(Game);

            } while (DataCheck.CheckWithRegex(Name, Game.Language.Regex) != true);
        }

        public bool ReadPlayerWord(Game Game)
        {
            bool result = true;
            string playerWord;
            List<char> mainWordList = [.. Game.Word.ToLower()];

            try
            {
                do
                {
                    Game.Language.EnterPlayerWord(Name, Game.Word);
                    playerWord = Game.MyConsole.ReadMessage(Game);

                } while (DataCheck.CheckWithRegex(playerWord, Game.Language.Regex) != true);


                List<char> chars = DataCheck.CheckPlayerWord(Game.Word, playerWord);

                if (chars.Count > 0)
                {
                    for (int i = 0; i < chars.Count; i++)
                    {
                        Game.Language.CharIsNotInWord(chars[i]);
                    }
                    result = false;
                }

                if (DataCheck.WordIsInList(playerWord, Game.Words))
                {
                    Game.Language.WordIsInList(playerWord);
                    result = false;
                }

                if (result)
                {
                    Words.Add(playerWord);
                    Game.Words.Add(playerWord);
                    this.Score += playerWord.Length;
                    Game.MyConsole.Clear();
                }
                else
                {
                    Game.ActivePlayers.Remove(Name);
                }
            }
            catch
            {
                Game.Language.ErrorMessage("ReadPlayerWord");
            }
            return result;
        }
    }
}

