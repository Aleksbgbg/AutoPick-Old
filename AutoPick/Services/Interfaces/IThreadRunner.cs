namespace AutoPick.Services.Interfaces
{
    public delegate void ThreadCallback();

    public interface IThreadRunner
    {
        event ThreadCallback ThreadAwake;

        void ChangeDelay(int newDelayMs);

        void Start();
    }
}