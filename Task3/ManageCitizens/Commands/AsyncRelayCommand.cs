namespace ManageCitizens.Commands
{
    class AsyncRelayCommand : AsyncBasicCommand
    {
        private readonly Func<Task> _callback;

        public AsyncRelayCommand(Func<Task> callback, Action<Exception>? onException = null) : base(onException) => _callback = callback;

        protected override async Task ExecuteAsync(object parameter)
        {
            await _callback();
        }
    }
}
