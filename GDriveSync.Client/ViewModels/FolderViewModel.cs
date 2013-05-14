using GalaSoft.MvvmLight;
using GDriveSync.Core;
using System.Collections.ObjectModel;

namespace GDriveSync.Client
{
    public class FolderViewModel : ObservableObject, IViewModel
    {
        #region Properties
        private string _title;
        public string Title
        {
            get { return Model != null ? Model.Title : _title; }
            set { _title = value; }
        }

        public Item Model { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                RaisePropertyChanged(() => IsExpanded);
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        public ObservableCollection<FolderViewModel> Children { get; private set; }
        #endregion Properties

        #region Ctor
        public FolderViewModel()
        {
            Children = new ObservableCollection<FolderViewModel>();
        }
        #endregion Ctor
    }
}
