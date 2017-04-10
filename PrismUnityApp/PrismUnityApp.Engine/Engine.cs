using PrismUnityApp.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Data;
using System.Windows;
using System.Reflection;

namespace PrismUnityApp.Engine
{
    public class Engine : IEngine
    {
        // Time delay, in hundredths of a second, between two frames in an animated GIF image.
        private const int PROPERTY_TAG_FRAME_DELAY = 0x5100;

        //For an animated GIF image, the number of times to display the animation. A value of 0 specifies that the animation should be displayed infinitely
        private const int PROPERTY_TAG_FRAME_COUNT = 0x5101;

        private int frameCount;
        private bool animated;
        private int[] frameDelay;

        private IList<FrameInfo> imageInfoList;
        private IList<byte[]> imageStreamDataList;

        private Dictionary<Guid, ImageFormat> GuidToImageFormatMap { get; set; }

        public Engine()
        {
            GuidToImageFormatMap = new Dictionary<Guid, ImageFormat>()
            {
                { ImageFormat.Bmp.Guid,  ImageFormat.Bmp},
                { ImageFormat.Gif.Guid,  ImageFormat.Png},
                { ImageFormat.Icon.Guid, ImageFormat.Png},
                { ImageFormat.Jpeg.Guid, ImageFormat.Jpeg},
                { ImageFormat.Png.Guid,  ImageFormat.Png}
            };
        }

        public ObservableCollection<FrameInfo> Run(string path)
        {
            
            if (!File.Exists(path))
            {
                return null;
            }

            
            imageStreamDataList = new List<byte[]>();
            imageInfoList = new List<FrameInfo>();
            
            Image image;
            ImageFormat imageFormat;

            makeImageAndFormat(path, out image, out imageFormat);

            if (imageFormat == null)
            {
                return null;
            }


            animated = ImageAnimator.CanAnimate(image);

            if(animated)
            {
                frameCount = image.GetFrameCount(FrameDimension.Time);
                PropertyItem freamDelayItem = image.GetPropertyItem(PROPERTY_TAG_FRAME_DELAY);

                if(freamDelayItem != null)
                {
                    byte[] values = freamDelayItem.Value;
                    frameDelay = new int[frameCount];

                    for(int i = 0; i < frameCount; i++)
                    {
                        // 단위 ms(0~9ms = 0ms)
                        frameDelay[i] = values[i * 4] + 256 * values[i * 4 + 1] + 256 * 256 * values[i * 4 + 2] + 256 * 256 * 256 * values[i * 4 + 3];

                        image.SelectActiveFrame(FrameDimension.Time, i);
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            image.Save(memoryStream, imageFormat);
                            imageStreamDataList.Add(memoryStream.ToArray());
                        }
                    }
                }
            }
            else
            {
                frameCount = 1;
                image.SelectActiveFrame(FrameDimension.Time, 0);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, imageFormat);
                    imageStreamDataList.Add(memoryStream.ToArray());
                }
            }

            if (frameDelay == null)
            {
                frameDelay = new int[frameCount];
            }
            
            
            for (int i = 0; i < frameCount; i++)
            {
                imageInfoList.Add(new FrameInfo
                {
                    ImageSource = imageStreamDataList[i],
                    ImageName = string.Format("{0}__{1}", path.Split('.')[0], i),
                    Duration = frameDelay[i] * 10
                });
            }
            
            return new ObservableCollection<FrameInfo>(imageInfoList);
        }

        public string GetLoopCount(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            Image image = null;
            ImageFormat imageFormat = null;

            makeImageAndFormat(path, out image, out imageFormat);

            if (imageFormat == null)
            {
                return null;
            }

            animated = ImageAnimator.CanAnimate(image);

            if(animated)
            {
                PropertyItem freamLoopItem = image.GetPropertyItem(PROPERTY_TAG_FRAME_COUNT);
                
                if(freamLoopItem == null)
                {
                    return null;
                }

                byte loopCount = freamLoopItem.Value[0];

                switch (loopCount)
                {
                    case 0:
                        return "무한 반복되는 파일 입니다.";
                    default:
                        return string.Format("{0}번 반복 되는 파일 입니다.", loopCount+1);
                }

            }
            else
            {
                return "움짤 GIF 파일이 아닙니다.";
            }
        }


        private void makeImageAndFormat(string path, out Image image, out ImageFormat imageFormat)
        {
            image = Image.FromFile(path);
            Guid imageGuid = image.RawFormat.Guid;

            ImageFormat tmpFormat = null;

            foreach (KeyValuePair<Guid, ImageFormat> pair in GuidToImageFormatMap)
            {
                if (imageGuid == pair.Key)
                {
                    tmpFormat = pair.Value;
                    break;
                }
            }

            imageFormat = tmpFormat;
        }

       

    }
   
}
