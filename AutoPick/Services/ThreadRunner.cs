namespace AutoPick.Services
{
    using System.Threading.Tasks;

    using AutoPick.Services.Interfaces;

    public class ThreadRunner : IThreadRunner
    {
        private int _delay;

        public event ThreadCallback ThreadAwake;

        public void ChangeDelay(int newDelayMs)
        {
            _delay = newDelayMs;
        }

        public void Start()
        {
            Task.Factory.StartNew(Run, TaskCreationOptions.LongRunning);
        }

        private async Task Run()
        {
            ThreadAwake?.Invoke();
            await Task.Delay(_delay);
        }
    }
}