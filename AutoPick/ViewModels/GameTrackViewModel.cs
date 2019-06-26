namespace AutoPick.ViewModels
{
    using System.Windows.Media;

    using AutoPick.Models;
    using AutoPick.Services.Interfaces;
    using AutoPick.ViewModels.Interfaces;

    public class GameTrackViewModel : ViewModelBase, IGameTrackViewModel
    {
        private readonly IStatusMessageService _statusMessageService;

        public GameTrackViewModel(IGamePollService gamePollService, IStatusMessageService statusMessageService)
        {
            _statusMessageService = statusMessageService;

            gamePollService.GameUpdated += HandleStatusUpdate;
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

        private string _status;
        public string Status
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
            Status = _statusMessageService.ConvertToStatusMessage(statusUpdate.GameStatus);
            Image = statusUpdate.GameImage;
        }
    }
}