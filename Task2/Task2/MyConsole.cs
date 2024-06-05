namespace Task2
{
    internal struct MyConsole
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
