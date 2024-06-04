namespace Task2
{
    internal class MyConsole
    {
        public void Clear()
        {
            Console.Clear();
        }

        public string ReadMessage()
        {
            return Console.ReadLine();
        }

        public void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
