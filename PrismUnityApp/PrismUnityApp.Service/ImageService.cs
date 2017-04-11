using PrismUnityApp.Service.Model;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace PrismUnityApp.Service
{
    public class ImageService : IImageService
    {
        private const int UNKNOWN_LOOP_COUNT = -100;
        


        private const int PROPERTY_TAG_FRAME_DELAY = 0x5100;
        // For an animated GIF image, the number of times to display the animation. 
        // A value of 0 specifies that the animation should be displayed infinitely
        private const int PROPERTY_TAG_FRAME_COUNT = 0x5101;

        private int FrameCount;
        private bool Animated;
        private int[] FrameDelay;
        private int LoopCount;

        private IList<ImageFrame> ImageFrameList;
        private IList<byte[]> FrameStreamDataList;

        private Dictionary<Guid, ImageFormat> GuidToImageFormatMap;

        private FolderBrowserDialog FolderDialog;

        private Microsoft.Win32.OpenFileDialog FileDialog;

        private Image Image;
        private ImageFormat ImageFormat;

        private BitmapEncoder Encoder;

        public ImageService()
        {
            GuidToImageFormatMap = new Dictionary<Guid, ImageFormat>()
            {
                { ImageFormat.Bmp.Guid,  ImageFormat.Bmp},
                { ImageFormat.Gif.Guid,  ImageFormat.Png},
                { ImageFormat.Icon.Guid, ImageFormat.Png},
                { ImageFormat.Jpeg.Guid, ImageFormat.Jpeg},
                { ImageFormat.Png.Guid,  ImageFormat.Png}
            };

            
            ImageFrameList = new List<ImageFrame>();
            FrameStreamDataList = new List<byte[]>();

            FolderDialog = new FolderBrowserDialog();
            FolderDialog.Description = "프레임 저장 위치";
            FileDialog = new Microsoft.Win32.OpenFileDialog();
            FileDialog.DefaultExt = "jpg";
            FileDialog.Filter = "이미지(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png;*.psd";

            Encoder = new PngBitmapEncoder();

            LoopCount = UNKNOWN_LOOP_COUNT;
        }


        private void LoadImage()
        {
            Nullable<bool> isSelectedFile = FileDialog.ShowDialog();

            if (isSelectedFile == false)
            {
                return;
            }

            if (!File.Exists(FileDialog.FileName))
            {
                throw new FileNotFoundException("해당 파일은 존재하지 않습니다.");
            }
        }

        private void ConfigurationFrameSaveDirectoryPath()
        {
            //if (ImageFilePath == null)
            //{
            //    LoadImage();
            //}
            if(Animated)
            {
                FolderDialog.ShowDialog();
            }
        }

        private void AnimatedImageSplit()
        {

            if(ImageFrameList.Count != 0)
            {
                ImageFrameList.Clear();
            }

            if(FrameStreamDataList.Count != 0)
            {
                FrameStreamDataList.Clear();
            }

            Image = Image.FromFile(FileDialog.FileName);
            Guid imageGuid = Image.RawFormat.Guid;

            ImageFormat = null;

            foreach(KeyValuePair<Guid, ImageFormat> pair in GuidToImageFormatMap)
            {
                if(imageGuid == pair.Key)
                {
                    ImageFormat = pair.Value;
                    break;
                }
            }

            if(ImageFormat == null)
            {
                throw new FileFormatException("해당 파일에 맞는 확장자가 존재하지 않습니다.");
            }


            Animated = ImageAnimator.CanAnimate(Image);

            if(Animated)
            {
                FrameCount = Image.GetFrameCount(FrameDimension.Time);
                PropertyItem frameDelayItem = Image.GetPropertyItem(PROPERTY_TAG_FRAME_DELAY);
                

                if (frameDelayItem != null)
                {
                    byte[] values = frameDelayItem.Value;
                    FrameDelay = new int[FrameCount];
                    
                    for(int i = 0; i < FrameCount; ++i)
                    {
                        FrameDelay[i] = values[i * 4] + 256 * values[i * 4 + 1] + 256 * 256 * values[i * 4 + 2] + 256 * 256 * 256 * values[i * 4 + 3] * 10;

                        Image.SelectActiveFrame(FrameDimension.Time, i);
                        using(MemoryStream memortStream = new MemoryStream())
                        {
                            Image.Save(memortStream, ImageFormat);
                            FrameStreamDataList.Add(memortStream.ToArray());
                        }
                    }
                }
                else
                {
                    FrameCount = 1;
                    Image.SelectActiveFrame(FrameDimension.Time, 0);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Image.Save(ms, ImageFormat);
                        FrameStreamDataList.Add(ms.ToArray());
                    }
                }

            }

            if(FrameDelay == null)
            {
                FrameDelay = new int[FrameCount];
            }            
        }
        private void GetAnimatedImageLoopCount()
        {
            if(Animated)
            {
                PropertyItem frameLoopItem = Image.GetPropertyItem(PROPERTY_TAG_FRAME_COUNT);
                if(frameLoopItem != null)
                {
                    
                    MessageBox.Show(BitConverter.ToInt16(frameLoopItem.Value, 0).ToString());
                    LoopCount = BitConverter.ToInt16(frameLoopItem.Value, 0);
                    if (frameLoopItem.Value[0] != 0)
                    {
                        LoopCount++;
                    }
                }
            }
            else
            {
                LoopCount = -1;
            }
        }

        private void SaveFrame()
        {

            if (Animated)
            {
                string framePath;
                string frameName;
                for (int i = 0; i < FrameCount; i++)
                {
                    frameName = string.Format(@"{0}_frame[{1}].{2}", FileDialog.SafeFileName.Split('.')[0], i, ImageFormat.ToString());
                    framePath = string.Format(@"{0}\\{1}", FolderDialog.SelectedPath, frameName);
                    using (Image image = Image.FromStream(new MemoryStream(FrameStreamDataList[i])))
                    {
                        image.Save(framePath, ImageFormat);
                    }

                    ImageFrameList.Add(new ImageFrame
                    {
                        Name = frameName,
                        Path = framePath,
                        Source = FrameStreamDataList[i],
                        Duration = FrameDelay[i],
                    });
                }
            }
            else
            {
                ImageFrameList.Add(new ImageFrame
                {
                    Name = FileDialog.SafeFileName,
                    Path = FileDialog.FileName,
                    Source = FrameStreamDataList[0],
                    Duration = FrameDelay[0],
                });
            }
            

        }

        public ImageFile Build()
        {
            LoadImage();
            
            AnimatedImageSplit();
            GetAnimatedImageLoopCount();
            ConfigurationFrameSaveDirectoryPath();
            SaveFrame();

            

            return new ImageFile
            {
                FrameList = new ObservableCollection<ImageFrame>(ImageFrameList),
                LoopCount = this.LoopCount
            };
            
        }

        
    }
}
