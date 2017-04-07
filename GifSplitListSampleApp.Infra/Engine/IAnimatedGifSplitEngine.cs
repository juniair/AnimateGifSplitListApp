using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GifSplitListSampleApp.Infra.Model
{
    public interface IAnimatedGifSplitEngine
    {
        

        string FramName { get; private set; }
        int FrameCount { get; set; }
        bool IsAnimated { get; set; }
        int[] FrameDelay { get; set; }


        void Run(string path);
        ObservableCollection<ImageInfo> GetSplitImageInfo();
    }
}
