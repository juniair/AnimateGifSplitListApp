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
            imageInfo = new ImageInfo();
        }

        private void run(object obj)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "jpg";
            openFile.Filter = "Images Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png; *.psd";
            openFile.ShowDialog();

            FileName = openFile.SafeFileName;
            
            var tempList = Engine.Run(openFile.FileName);
            string tempLoop = Engine.GetLoopCount(openFile.FileName);
            if(tempList != null && tempLoop != null)
            {
                FrameList = tempList;
                LoopCount = tempLoop;
            }
            else
            {
                LoopCount = "파일을 찾을 수 없습니다.";
            }

            

            
        }
    }
}
