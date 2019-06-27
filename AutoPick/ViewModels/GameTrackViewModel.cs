namespace AutoPick.ViewModels
{
    using System.Windows.Media;

    using AutoPick.Models;
    using AutoPick.Services.GameInteraction;
    using AutoPick.ViewModels.Interfaces;

    public class GameTrackViewModel : ViewModelBase, IGameTrackViewModel
    {
        public GameTrackViewModel(IGameMonitor gameMonitor)
        {
            gameMonitor.GameUpdated += HandleStatusUpdate;
        }

        private ImageSource _image;
        public ImageSource Image
        {
            get => _image;

            set
            {
                if (_image == value) return;

                _image = value;
                NotifyOfPropertyChange(nameof(Image));
            }
        }

        private GameStatus _status;
        public GameStatus Status
        {
            get => _status;

            set
            {
                if (_status == value) return;

                _status = value;
                NotifyOfPropertyChange(nameof(Status));
            }
        }

        private void HandleStatusUpdate(GameStatusUpdate statusUpdate)
        {
            Status = statusUpdate.GameStatus;
            Image = statusUpdate.GameImage;
        }
    }
}