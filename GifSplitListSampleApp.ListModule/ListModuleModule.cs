using GifSplitListSampleApp.ListModule.Views;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace GifSplitListSampleApp.ListModule
{
    public class ListModuleModule : IModule
    {
        IRegionManager _regionManager;

        public ListModuleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("SplitImageListRegion", typeof(SplitImageListView));
        }
    }
}