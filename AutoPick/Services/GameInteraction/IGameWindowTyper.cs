namespace AutoPick.Services.GameInteraction
{
    public interface IGameWindowTyper
    {
        void TypeAt(int x, int y, string text);
    }
}