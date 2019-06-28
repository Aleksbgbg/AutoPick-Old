namespace AutoPick.Services.GameInteraction
{
    using System;
    using System.Threading.Tasks;

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
            while (true)
            {
#if DEBUG
                try
                {
                    ThreadAwake?.Invoke();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

#else
                ThreadAwake?.Invoke();
#endif

                await Task.Delay(_delay);
            }
        }
    }
}