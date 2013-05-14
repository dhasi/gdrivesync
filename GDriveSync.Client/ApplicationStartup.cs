using Castle.Facilities.Startable;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace GDriveSync.Client
{
    public class ApplicationStartup
    {
        #region Fields        
        private readonly Application _app;
        private IWindsorContainer _container;
        #endregion Fields

        #region Ctor
        public ApplicationStartup(Application app)
        {
            _app = app;

            InitializeContainer();
        }
        #endregion Ctor

        public void Run<TMainWindow>() where TMainWindow : Window
        {
            var view = _container.Resolve<MainWindow>();
            _app.MainWindow = view;
            _app.MainWindow.Show();
        }

        private void InitializeContainer()
        {
            _container = new WindsorContainer();
            _container.AddFacility<TypedFactoryFacility>();
            _container.AddFacility<StartableFacility>();

            _container.Register(Component
                .For<IWindsorContainer>()
                .Instance(_container));

            //Register Messenger
            _container.Register(Component
                .For<IMessenger>()
                .ImplementedBy<Messenger>());

            _container.Register(Component
                .For<IViewLocator>()
                .ImplementedBy<ViewLocator>());
            
            //Register ViewManager
            _container.Register(Component
                .For<IViewManager>()
                .ImplementedBy<ViewManager>()
                .Start());

            //Register Views
            _container.Register(Types
                .FromAssembly(GetType().Assembly)
                .BasedOn<IView>()
                .LifestyleTransient());

            //Register ViewModels           
            _container.Register(Types
                .FromAssembly(GetType().Assembly)
                .BasedOn<IViewModel>()
                .LifestyleTransient());

            _container.Install();

        }
    }
}
