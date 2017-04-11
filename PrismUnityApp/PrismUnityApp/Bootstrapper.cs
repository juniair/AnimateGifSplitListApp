using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;
using PrismUnityApp.Service;
using PrismUnityApp.Views;
using System.Windows;

namespace PrismUnityApp
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

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            this.RegisterTypeIfMissing(typeof(IImageService), typeof(ImageService), true);
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
        }
    }
}
