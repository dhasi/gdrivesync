using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GDriveSync.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GDriveSync.Client
{
    public class SelectTargetViewModel : ViewModelBase, IViewModel
    {
        #region Fields
        private readonly List<FolderViewModel> _flatFolderList = new List<FolderViewModel>();
        #endregion Fields

        #region Events
        public event EventHandler OnClose;
        #endregion Events

        #region Properties
        public Action<FolderViewModel> CommitAction { get; set; }

        public ObservableCollection<FolderViewModel> Folders { get; private set; }
        #endregion Properties

        #region Ctor
        public SelectTargetViewModel()
        {
            _flatFolderList = new List<FolderViewModel>();
            Folders = new ObservableCollection<FolderViewModel>();

            InitializeCommands();
        }
        #endregion Ctor

        #region Methods
        private void InitializeCommands()
        {
            CommitCommand = new RelayCommand(Commit, CanCommit);
        }

        public void LoadFolders()
        {
            Task.Factory.StartNew(() =>
            {
                using (var client = new DriveClient(new OAuth2Service(), new CredentialsStore()))
                {
                    var list = new List<FolderViewModel>();
                    client.GetFolders()
                        .ForEach(f =>
                        {
                            var folder = new FolderViewModel { Model = f };
                            folder.Children.Add(new FolderViewModel { Title = "Loading" });
                            folder.PropertyChanged += OnFolderChanged;
                            list.Add(folder);
                        });
                    return list;
                }
            }).ContinueWith(r =>
            {
                _flatFolderList.Clear();
                Folders.Clear();
                r.Result.ForEach(x =>
                    {
                        Folders.Add(x);
                        _flatFolderList.Add(x);
                    });
                
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void LoadFolders(FolderViewModel parent)
        {
            Task.Factory.StartNew(() =>
            {
                using (var client = new DriveClient(new OAuth2Service(), new CredentialsStore()))
                {
                    var list = new List<FolderViewModel>();
                    client.GetFolders(parent.Model.Id)
                        .ForEach(f =>
                        {
                            var folder = new FolderViewModel { Model = f };
                            folder.Children.Add(new FolderViewModel { Title = "Loading" });
                            folder.PropertyChanged += OnFolderChanged;
                            list.Add(folder);
                        });
                    return list;
                }
            }).ContinueWith(r =>
            {
                parent.Children.Clear();
                r.Result.ForEach(x => parent.Children.Add(x));
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void OnFolderChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = sender as FolderViewModel;
            if (e.PropertyName == "IsExpanded" && item != null && item.IsExpanded)
                LoadFolders(sender as FolderViewModel);
        }

        public RelayCommand CommitCommand { get; private set; }

        public bool CanCommit() { return _flatFolderList.Count(x => x.IsSelected) == 1; }

        public void Commit()
        {
            if (!CanCommit()) return;

            var folder = _flatFolderList.FirstOrDefault(x => x.IsSelected);
            if (folder != null && CommitAction != null)            
                CommitAction(folder);               
            
            if (OnClose != null)
                OnClose(this, EventArgs.Empty);
        }
        #endregion Methods
    }
}
