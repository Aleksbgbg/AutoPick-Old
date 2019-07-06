namespace AutoPick.Services.GameInteraction
{
    public interface ISelectedRoleStore
    {
        string Champion { set; }

        string Lane { set; }
    }
}