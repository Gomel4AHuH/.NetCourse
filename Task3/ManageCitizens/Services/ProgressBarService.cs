namespace ManageCitizens.Services
{
    public class ProgressBarService(string text, int count)
    {
        private string _text { get; set; }  = text;
        private int _count { get; set; } = count;
        private double _progress { get; set; }

        public void Start()
        {

        }

        public void Finish()
        {
            _text = "";
            _progress = 0;
        }
        public double UpdateStatus()
        {
            return _progress;
        }

    }
}
