using Microsoft.Practices.Unity;
using Prism.Unity;
using GifSplitListSampleApp.Views;
using System.Windows;

namespace GifSplitListSampleApp
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
