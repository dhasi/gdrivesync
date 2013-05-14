using Castle.Windsor;
using System.Linq;

namespace GDriveSync.Client
{
    public interface IViewLocator
    {
        IView LocateFor(IViewModel context);
    }

    public class ViewLocator : IViewLocator
    {
        private readonly IWindsorContainer _container;

        public ViewLocator(IWindsorContainer container)
        {
            _container = container;
        }

        public IView LocateFor(IViewModel context)
        {
            var view = default(IView);
            var viewTypes = GetType().Assembly.GetTypes()
                .Where(x => typeof(IView).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToList();

            foreach (var type in viewTypes)
            {
                var ctor = type.GetConstructors()
                    .FirstOrDefault(x => x.GetParameters().Any(p => p.ParameterType == context.GetType()));

                if (ctor != null)
                {
                    //TODO: catch result in dictionary for next locate.
                    view = _container.Resolve(type) as IView;
                    break;
                }
            }

            return view;

        }
    }
}
