namespace Task2
{
    internal class Players
    {
        public List<string> Names { get; set; }
        public List<int> Scores { get; set; }

        public Players(List<string> names, List<int> scores)
        {
            Names = names;
            Scores = scores;
        }
    }
}
