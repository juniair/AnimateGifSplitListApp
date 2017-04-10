using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnityApp.Infra
{
    public interface IEngine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">파일 위치</param>
        /// <returns>프레임 정보가 있는 리스트</returns>
        ObservableCollection<FrameInfo> Run(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">파일 위치</param>
        /// <returns>반복 횟수(-1 : 파일이 존재 하지 않음, 0 : 무한 반복, n : n번 반복</returns>
        string GetLoopCount(string path);
    }

    public class FrameInfo
    {
        public byte[] ImageSource { get; set; }
        public string ImageName { get; set; }
        public int Duration { get; set; }
    }

    public class ImageInfo : BindableBase
    {

        private ObservableCollection<FrameInfo> frameList;
        public ObservableCollection<FrameInfo> FrameList
        {
            get { return frameList; }
            set { SetProperty(ref frameList, value); }
        }

        private string loopCount;
        public string LoopCount
        {
            get { return loopCount; }
            set { SetProperty(ref loopCount, value); }
        }
    }

    
}
