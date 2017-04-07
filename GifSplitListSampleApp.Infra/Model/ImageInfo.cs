using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GifSplitListSampleApp.Infra.Model
{
    public class ImageInfo
    {
        public Bitmap SplitImage { get; set; }
        public string SplitImageName { get; set; }
        public int Duration { get; set; }
    }
}
