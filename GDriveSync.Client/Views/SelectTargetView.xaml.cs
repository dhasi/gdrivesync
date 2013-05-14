using System.Windows;
using System.Windows.Controls;

namespace GDriveSync.Client
{
    /// <summary>
    /// Interaction logic for SelectTargetView.xaml
    /// </summary>
    public partial class SelectTargetView : UserControl, IView
    {
        #region Properties
        public IViewModel Context
        {
            get { return DataContext as IViewModel; }
        }
        #endregion Properties

        #region Ctor
        public SelectTargetView(SelectTargetViewModel context)
        {
            InitializeComponent();
            DataContext = context;
            context.OnClose += OnClose;
        }
        #endregion Ctor

        #region Methods
        private void OnClose(object sender, System.EventArgs e)
        {
            var parent = Parent as Window;
            if (parent != null)
                parent.Close();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var ctx = DataContext as SelectTargetViewModel;
            if (ctx != null)
                ctx.LoadFolders();
        }
        #endregion Methods
    }
}