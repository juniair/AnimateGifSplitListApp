using Prism.Commands;
using Prism.Mvvm;
using PrismUnityApp.Service;
using PrismUnityApp.Service.Model;
using System.Linq;
using System.Windows.Input;

namespace PrismUnityApp.AnimatedGifSplitList.ViewModels
{
    public class SplitImageListViewModel : BindableBase
    {
        IImageService Engine { get; set; }

        private ImageFile imageInfo;
        public ImageFile ImageInfo
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

        public SplitImageListViewModel(IImageService engine)
        {

            FileName = "파일 선택";
            Engine = engine;
            SplitImageCommand = new DelegateCommand<object>(run);
            SelectedCommand = new DelegateCommand<object[]>(OnItemSelected);
            ImageInfo = new ImageFile();
        }

        private void OnItemSelected(object[] selectedItems)
        {
            if(selectedItems != null && selectedItems.Length > 0)
            {
                ImageFrame frame = selectedItems.FirstOrDefault() as ImageFrame;
                if(frame != null)
                {
                    FrameName = string.Format("FramenName : {0}", frame.Name);
                    FrameDuration = string.Format("FramenDuration : {0} ms", frame.Duration);
                    
                }
            }
        }

        private void run(object obj)
        {
            ImageInfo = Engine.Build();
            
       
        }
    }
}
