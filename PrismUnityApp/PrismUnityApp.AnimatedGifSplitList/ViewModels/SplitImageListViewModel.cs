using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using PrismUnityApp.Infra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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


        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { SetProperty(ref fileName, value); }
        }

        private string frameName;
        public string FrameName
        {
            get { return frameName; }
            set { SetProperty(ref frameName, value); }
        }

        private string frameDuration;
        public string FrameDuration
        {
            get { return frameDuration; }
            set { SetProperty(ref frameDuration, value); }
        }

        public ICommand SplitImageCommand { get; set; }
        public ICommand SelectedCommand { get; set; }

        public SplitImageListViewModel(IEngine engine)
        {

            FileName = "파일 선택";
            Engine = engine;
            SplitImageCommand = new DelegateCommand<object>(run);
            SelectedCommand = new DelegateCommand<object[]>(OnItemSelected);
            ImageInfo = new ImageInfo();
        }

        private void OnItemSelected(object[] selectedItems)
        {
            if(selectedItems != null && selectedItems.Length > 0)
            {
                FrameInfo frame = selectedItems.FirstOrDefault() as FrameInfo;
                if(frame != null)
                {
                    FrameName = string.Format("FramenName : {0}", frame.ImageName);
                    FrameDuration = string.Format("FramenDuration : {0} ms", frame.Duration);
                    
                }
            }
        }

        private void run(object obj)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            
            openFile.DefaultExt = "jpg";
            openFile.Filter = "이미지(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png;*.psd";
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

                FrameName = "FrameName : ";
                FrameDuration = "FrameDuration : ";
            }
            
       
        }
    }
}
