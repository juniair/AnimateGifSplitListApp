using PrismUnityApp.Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnityApp.Service
{
    public interface IImageService
    {

        //void LoadImage();
        //void ConfigurationFrameSaveDirectoryPath();
        //void SaveFrame();
        ///// <summary>
        ///// GIF 이미지에 대한 정보를 얻습니다.
        ///// </summary>
        ///// <param name="imagePath"></param>
        ///// <returns></returns>
        //void AnimatedImageSplit();
      
        //void GetAnimatedImageLoopCount();

        

        ImageFile Build();

    }
}
