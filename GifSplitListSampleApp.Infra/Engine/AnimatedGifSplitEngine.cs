using GifSplitListSampleApp.Infra.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Reflection;

namespace GifSplitListSampleApp.Infra.Engine
{
    public class AnimatedGifSplitEngine : IAnimatedGifSplitEngine
    {
        private const int PROPERTY_TAG_FRAME_DELAY = 0x5100;

        public string FramName { get; set; }
        public int FrameCount { get; set; }
        public bool IsAnimated { get; set; }
        public int[] FrameDelay { get; set; }

        private IList<ImageInfo> SplitImageList { get; set; }
        
        private IList<byte[]> ImageStreamData { get; set; }

        private Dictionary<Guid, ImageFormat> GuidToImageFormatMap { get; set; }


        public AnimatedGifSplitEngine()
        {
            GuidToImageFormatMap = new Dictionary<Guid, ImageFormat>()
            {
                { ImageFormat.Bmp.Guid,  ImageFormat.Bmp},
                { ImageFormat.Gif.Guid,  ImageFormat.Png},
                { ImageFormat.Icon.Guid, ImageFormat.Png},
                { ImageFormat.Jpeg.Guid, ImageFormat.Jpeg},
                { ImageFormat.Png.Guid,  ImageFormat.Png}
            };
            ImageStreamData = new List<byte[]>();
        }

        public void Run(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("파일이 존재 하지 않습니다.");
            }

            Image image = Image.FromFile(path);


            ImageFormat imageFormat = null;
            Guid imageGuid = image.RawFormat.Guid;

            foreach(KeyValuePair<Guid, ImageFormat> pair in GuidToImageFormatMap)
            {
                if(imageGuid == pair.Key)
                {
                    imageFormat = pair.Value;
                    break;
                }
            }

            if (imageFormat == null)
            {
                throw new NoNullAllowedException("Unable to determine image format");
            }

            IsAnimated = ImageAnimator.CanAnimate(image);
            SplitImageList = new List<ImageInfo>();

            if (IsAnimated)
            {
                FrameCount = image.GetFrameCount(FrameDimension.Time);
                PropertyItem freamDelayItme = image.GetPropertyItem(PROPERTY_TAG_FRAME_DELAY);

                if (freamDelayItme != null)
                {
                    byte[] values = freamDelayItme.Value;

                    FrameDelay = new int[FrameCount];

                    for (int i = 0; i < FrameCount; i++)
                    {
                        FrameDelay[i] = values[i * 4] + 256 * values[i * 4 + 1] + 256 * 256 * values[i * 4 + 2] + 256 * 256 * 256 * values[i * 4 + 3];
                        image.SelectActiveFrame(FrameDimension.Time, i);
                        using(MemoryStream memoryStream = new MemoryStream())
                        {
                            image.Save(memoryStream, imageFormat);
                            ImageStreamData.Add(memoryStream.ToArray());
                        }
                    }
                }
            }
            else
            {
                FrameCount = 1;
                image.SelectActiveFrame(FrameDimension.Time, 0);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, imageFormat);
                    ImageStreamData.Add(memoryStream.ToArray());
                }
            }

            if (FrameDelay == null)
            {
                FrameDelay = new int[FrameCount];
            }

            for (int i = 0; i < FrameCount; i++)
            {
                SplitImageList.Add(new ImageInfo
                {
                    SplitImage = makeBitmap(ImageStreamData[i]),
                    SplitImageName = string.Format("Frame[{0}]", i),
                    Duration = FrameDelay[i]
                });
            }

        }

        private Bitmap makeBitmap(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return null;
            }

            try
            {
                using(MemoryStream memoryStream = new MemoryStream(imageBytes))
                {
                    using (Bitmap bitmap = new Bitmap(memoryStream))
                    {
                        return (Bitmap)bitmap.Clone();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error type: " + ex.GetType().ToString() + "\n" +
                    "Message: " + ex.Message,
                    "Error in " + MethodBase.GetCurrentMethod().Name
                    );
            }

            return null;

        }

        public ObservableCollection<ImageInfo> GetSplitImageInfo()
        {
            return new ObservableCollection<ImageInfo>(SplitImageList);
        }
    }
}
