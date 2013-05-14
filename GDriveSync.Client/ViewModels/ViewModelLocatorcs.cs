using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDriveSync.Client
{
    public interface IViewModelLocator
    {
        TViewModel Locate<TViewModel>() where TViewModel : IViewModel;
    }
}
