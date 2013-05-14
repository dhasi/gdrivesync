using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace GDriveSync.Client
{
    public interface IViewManager
    {
        void ShowViewFor(IViewModel context, IViewModel parent);
    }

    public class ViewManager : IViewManager
    {
        private readonly IMessenger _messenger;
        private readonly IViewLocator _viewLocator;

        private List<IView> _views = new List<IView>();

        public ViewManager(IMessenger messenger, IViewLocator viewLocator)
        {
            _messenger = messenger;
            _viewLocator = viewLocator;

            _messenger.Register<ShowViewMessage>(this, OnShowView);
        }

        private void OnShowView(ShowViewMessage msg)
        {
            if (msg != null && msg.Context != null)
                ShowViewFor(msg.Context, msg.Parent);
        }

        public void ShowViewFor(IViewModel context, IViewModel parent)
        {
            var view = _viewLocator.LocateFor(context);
            if (view == null) return;

            var window = new Window { Width = 640, Height = 480 };
            window.Content = view;
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
    }
}
