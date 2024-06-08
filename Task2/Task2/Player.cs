namespace Task2
{
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
        //public bool IsWinner { get; set; }
        public List<string> Words = new List<string>();

        public Player() { }

        public Player(string name)
        {
            this.Name = name;
        }

        public Player(Game game, int number)
        {
            do
            {
                game.language.EnterPlayerName(number);
                Name = game.myConsole.ReadMessage(game);

            } while (new DataCheck().CheckWithRegex(Name, game.language.regex) != true);

            Score = 0;
            //IsWinner = false;
            game.ActivePlayers.Add(Name);
            game.myConsole.Clear();
        }

        public void ReturnPlayerName()
        {
            Console.WriteLine(Name);
        }

        public bool ReadPlayerWord(Game game)
        {
            bool result = true;
            string playerWord;
            List<char> mainWordList = game.Word.ToLower().ToList();
            //this.IsWinner = false;

            do
            {
                game.language.EnterPlayerWord(Name, game.Word);
                playerWord = game.myConsole.ReadMessage(game);

            } while (new DataCheck().CheckWithRegex(playerWord, game.language.regex) != true);

            foreach (char ch in playerWord)
            {
                if (mainWordList.IndexOf(Char.ToLower(ch)) < 0)
                {
                    game.language.CharIsNotInWord(ch);
                    result = false;
                }
                mainWordList.Remove(ch);
            }
            
            if (Words.Any(word => word.ToLower() == playerWord.ToLower()))
            {
                game.language.WordIsInList(playerWord);
                result = false;
            }

            if (result)
            {
                Words.Add(playerWord);
                game.Words.Add(playerWord);
                this.Score += playerWord.Length;
                //this.IsWinner = true;
                game.myConsole.Clear();
            }
            else
            {
                game.ActivePlayers.Remove(Name);
            }

            return result;
        }
    }
}

