namespace Task2
{
    internal class Player
    {
        public string Name { get; set; }

        public Player(ILanguage language, int number)
        {
            language.EnterPlayerName(number);
            this.Name = Console.ReadLine();
        }

        public void ReturnPlayerName()
        {
            Console.WriteLine(this.Name);
        }
    }
}

