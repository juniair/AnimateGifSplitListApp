using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using PrismUnityApp.Infra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PrismUnityApp.AnimatedGifSplitList.ViewModels
{
    public class SplitImageListViewModel : BindableBase
    {
        IEngine Engine { get; set; }

        private ImageInfo imageInfo;
        public ImageInfo ImageInfo
        {
            get { return imageInfo; }
            set { SetProperty(ref imageInfo, value); }
        }

        private string animatedGIF;
        public string AnimatedGIF
        {
            get { return animatedGIF; }
            set { SetProperty(ref animatedGIF, value); }
        }

        public ICommand SplitImageCommand { get; set; }

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { SetProperty(ref fileName, value); }
        }
        public SplitImageListViewModel(IEngine engine)
        {

            FileName = "파일 선택";
            Engine = engine;
            SplitImageCommand = new DelegateCommand<object>(run);
            ImageInfo = new ImageInfo();
        }

        private void run(object obj)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "jpg";
            openFile.Filter = "Images Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png; *.psd";
            Nullable<bool> result = openFile.ShowDialog();
            if(result == true)
            {
                FileName = openFile.SafeFileName;
                AnimatedGIF = openFile.FileName;
                var tempList = Engine.Run(AnimatedGIF);
                string tempLoop = Engine.GetLoopCount(openFile.FileName);
                if (tempList != null && tempLoop != null)
                {
                    ImageInfo.FrameList = tempList;
                    ImageInfo.LoopCount = tempLoop;
                }
                else
                {
                    ImageInfo.LoopCount = "파일을 찾을 수 없습니다.";
                }
            }
            
       
        }
    }
}
