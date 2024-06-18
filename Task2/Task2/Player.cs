namespace Task2
{
    public class Player(int number)
    {
        public string Name { get; set; } = "";
        public int Number { get; set; } = number;
        public int Score { get; set; } = 0;
        public List<string> Words = [];
        public DataCheck dataCheck = new();

        public void SetName(Game game)
        {
            do
            {
                game.Language.EnterPlayerName(Number);
                Name = game.MyConsole.ReadMessage(game);

            } while (!DataCheck.CheckWithRegex(Name, game.Language.Regex));
        }        
    }
}

