using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace PrismUnityApp.AnimatedGifSplitList
{
    public class AnimatedGifSplitListModule : IModule
    {
        IRegionManager _regionManager;

        public AnimatedGifSplitListModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;

        }

        public void Initialize()
        {
            
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(Views.SplitImageList));
        }
    }
}