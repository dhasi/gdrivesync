using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;

namespace GDriveSync.Client
{
    public class GDriveContentViewModel : ViewModelBase, IViewModel
    {
        #region Properties
        public string Title { get; set; }

        private FolderViewModel _activeFolder;
        public FolderViewModel ActiveFolder
        {
            get { return _activeFolder; }
            set
            {
                _activeFolder = value;
                RaisePropertyChanged(() => ActiveFolder);
            }
        }

        public ObservableCollection<FolderViewModel> Folders { get; private set; }
        #endregion Properties

        #region Ctor
        public GDriveContentViewModel(IMessenger messenger)
            : base(messenger)
        {
            InitializeCommands();
            Folders = new ObservableCollection<FolderViewModel>();
        }
        #endregion Ctor

        #region Methods
        private void InitializeCommands()
        {
            AddBaseDirectoryCommand = new RelayCommand(AddBaseDirectory, CanAddBaseDirectory);
        }

        public RelayCommand AddBaseDirectoryCommand { get; private set; }

        public bool CanAddBaseDirectory()
        {
            return true;
        }

        public void AddBaseDirectory()
        {
            if (CanAddBaseDirectory())
            {
                var ctx = new SelectTargetViewModel
                    {
                        CommitAction = o =>
                        {
                            Folders.Add(o);
                            ActiveFolder = o;
                        }
                    };
                MessengerInstance.Send(new ShowViewMessage
                {
                    Context = ctx
                });
            }
        }
        #endregion Methods
    }
}
