using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace GDriveSync.Client
{
    public class MainViewModel : ViewModelBase, IViewModel
    {
        private readonly IViewModelLocator _locator;

        public GDriveContentViewModel GDrive { get; set; }

        public MainViewModel(IMessenger messenger, IViewModelLocator locator)
            : base(messenger)
        {
            InitializeCommands();
            _locator = locator;
            GDrive = _locator.Locate<GDriveContentViewModel>();
        }

        #region Methods
        private void InitializeCommands()
        {
            SelectTargetCommand = new RelayCommand(SelectTarget, CanSelectTarget);
        }

        public RelayCommand SelectTargetCommand { get; private set; }

        private bool CanSelectTarget() { return true; }

        private void SelectTarget()
        {
            MessengerInstance.Send(new ShowViewMessage
            {
                Context = new SelectTargetViewModel(),
                Parent = this
            });
        }
        #endregion Methods
    }
}
