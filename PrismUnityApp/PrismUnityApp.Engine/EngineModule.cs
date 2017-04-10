using Microsoft.Practices.Unity;
using Prism.Modularity;
using PrismUnityApp.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnityApp.Engine
{
    [Module(ModuleName = "EngineModule")]
    public class EngineModule: IModule
    {
        IUnityContainer _container;

        public EngineModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IEngine, Engine>(new ContainerControlledLifetimeManager());
        }
    }
}
