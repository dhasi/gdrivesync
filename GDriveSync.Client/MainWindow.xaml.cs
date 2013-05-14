using System.Windows.Controls.Ribbon;

namespace GDriveSync.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow, IView
    {
        public IViewModel Context
        {
            get { return DataContext as IViewModel; }
        }

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
