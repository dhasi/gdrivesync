using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace GDriveSync.Client
{
    public class MainViewModel : ViewModelBase, IViewModel
    {
        private readonly IViewManager _viewManager;

        public MainViewModel(IMessenger messenger, IViewManager viewManager)
            : base(messenger)
        {
            InitializeCommands();
            _viewManager = viewManager;
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
