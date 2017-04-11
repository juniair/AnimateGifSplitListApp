using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace PrismUnityApp.Service.Model
{
    public class ImageFile : BindableBase
    {
        private ObservableCollection<ImageFrame> frameList;
        public ObservableCollection<ImageFrame> FrameList
        {
            get { return frameList; }
            set { SetProperty(ref frameList, value); }
        }

        private int loopCount;
        public int LoopCount
        {
            get { return loopCount; }
            set { SetProperty(ref loopCount, value); }
        }
    }
}
