namespace Task2
{
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public List<string> Words = new List<string>();

        const int turnTime = 10;                            // turn time for each player in cesonds
        int tmpTurnTime = turnTime;                         // temp value for timer
        private static System.Timers.Timer aTimer;

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

            try
            {
                //setTimer(game);
                do
                {
                    game.language.EnterPlayerWord(Name, game.Word);
                    playerWord = game.myConsole.ReadMessage(game);

                } while (new DataCheck().CheckWithRegex(playerWord, game.language.regex) != true);


                List<char> chars = new DataCheck().CheckPlayerWord(game.Word, playerWord);

                if (chars.Count > 0)
                {
                    for (int i = 0; i < chars.Count; i++)
                    {
                        game.language.CharIsNotInWord(chars[i]);
                    }
                    result = false;
                }

                if (new DataCheck().wordIsInList(playerWord, game.Words))
                {
                    game.language.WordIsInList(playerWord);
                    result = false;
                }

                if (result)
                {
                    Words.Add(playerWord);
                    game.Words.Add(playerWord);
                    this.Score += playerWord.Length;
                    game.myConsole.Clear();
                }
                else
                {
                    game.ActivePlayers.Remove(Name);
                }
            }
            catch
            {
                game.language.ErrorMessage("ReadPlayerWord");
            }

            //aTimer.Stop();
            return result;
        }

        public void SetPrevScore(List<Player> players)
        {
            foreach (Player player in players)
            {
                if (player.Name == Name) Score = player.Score;
            }
        }

        void setTimer(Game game)
        {
            aTimer = new System.Timers.Timer(1000);
            aTimer.Interval = 1000;
            aTimer.Elapsed += (o, e) => OnTimedEvent(game);
            aTimer.Start();
        }

        void OnTimedEvent(Game game)
        {
            if (tmpTurnTime > 0)
            {
                Console.WriteLine(tmpTurnTime-- + " seconds remaining...");
            }
            else
            {
                Console.WriteLine(Name);
                game.ActivePlayers.Remove(Name);
                aTimer.Stop(); // Stop the timer
                Console.WriteLine("Stoptimer");
            }
        }
    }
}

