using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GifSplitListSampleApp.ListModule.ViewModels
{
    public class SplitImageListViewModel : BindableBase
    {

        public ObservableCollection<Tuple<string, int>> SplitImages { get; set; }

        public SplitImageListViewModel()
        {

        }
    }
}
