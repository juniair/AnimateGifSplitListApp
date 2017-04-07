using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Reflection;
using System.IO;
using System.Drawing.Imaging;

namespace HowToSplitAGifByFramesAndSaveThemToMemory
{
    /*
     * pbImage = A PictureBox
     * lbFiles = A ListBox
     */ 
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
        }

        string pathToImage = @"test.gif";
        //string pathToImage = @"test.png";

        List<byte[]> frames = new List<byte[]>() { };

        private void Mainform_Load(object sender, EventArgs e)
        {
            try
            {
                //Assuming "test.gif" is in the directory where this application is located
                pathToImage = AppDomain.CurrentDomain.BaseDirectory + pathToImage;

                //Try extracting the frames
                frames = EnumerateFrames(pathToImage);
                if (frames == null || frames.Count() == 0)
                {
                    throw new NoNullAllowedException("Unable to obtain frames from " + pathToImage);
                }

                for (int i = 0; i < frames.Count(); i++)
                {
                    lbFrames.Items.Add("Frame-" + i.ToString());
                }

                lbFrames.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error type: " + ex.GetType().ToString() + "\n" +
                    "Message: " + ex.Message,
                    "Error in " + MethodBase.GetCurrentMethod().Name
                    );
            }
        }

        private List<byte[]> EnumerateFrames(string imagePath)
        {
            try
            {
                //Make sure the image exists
                if (!File.Exists(imagePath))
                {
                    throw new FileNotFoundException("Unable to locate " + imagePath);
                }

                Dictionary<Guid, ImageFormat> guidToImageFormatMap = new Dictionary<Guid, ImageFormat>()
                {
                    {ImageFormat.Bmp.Guid,  ImageFormat.Bmp},
                    {ImageFormat.Gif.Guid,  ImageFormat.Png},
                    {ImageFormat.Icon.Guid, ImageFormat.Png},
                    {ImageFormat.Jpeg.Guid, ImageFormat.Jpeg},
                    {ImageFormat.Png.Guid,  ImageFormat.Png}
                };

                List<byte[]> tmpFrames = new List<byte[]>() { };

                using (Image img = Image.FromFile(imagePath, true))
                {
                    //Check the image format to determine what
                    //format the image will be saved to the 
                    //memory stream in
                    ImageFormat imageFormat = null;
                    Guid imageGuid = img.RawFormat.Guid;

                    
                    foreach (KeyValuePair<Guid, ImageFormat> pair in guidToImageFormatMap)
                    {
                        if (imageGuid == pair.Key)
                        {
                            imageFormat = pair.Value;
                            break;
                        }
                    }

                    if (imageFormat == null)
                    {
                        throw new NoNullAllowedException("Unable to determine image format");
                    }

                    //Get the frame count
                    FrameDimension dimension = new FrameDimension(img.FrameDimensionsList[0]);
                    int frameCount = img.GetFrameCount(dimension);

                    //Step through each frame
                    for (int i = 0; i < frameCount; i++)
                    {
                        //Set the active frame of the image and then 
                        //write the bytes to the tmpFrames array
                        img.SelectActiveFrame(dimension, i);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            
                            img.Save(ms, imageFormat);
                            tmpFrames.Add(ms.ToArray());
                        }
                    }

                }

                return tmpFrames;

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

        private Bitmap ConvertBytesToImage(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return null;
            }

            try
            {
                //Read bytes into a MemoryStream
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    //Recreate the frame from the MemoryStream
                    using (Bitmap bmp = new Bitmap(ms))
                    {
                        return (Bitmap)bmp.Clone();
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

        private void ClearPictureBoxImage()
        {
            try
            {
                if (pbImage.Image != null)
                {
                    pbImage.Image.Dispose();
                }
                pbImage.Image = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error type: " + ex.GetType().ToString() + "\n" +
                    "Message: " + ex.Message,
                    "Error in " + MethodBase.GetCurrentMethod().Name
                    );
            }
        }

        private void lbFrames_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbFrames.SelectedIndex == -1)
                {
                    return;
                }
                
                //Make sure frames have been extracted
                if (frames == null || frames.Count() == 0)
                {
                    throw new NoNullAllowedException("Frames have not been extracted");
                }

                //Make sure the selected index is within range
                if (lbFrames.SelectedIndex > frames.Count() - 1)
                {
                    throw new IndexOutOfRangeException("Frame list does not contain index: " + lbFrames.SelectedIndex.ToString());
                }

                //Clear the PictureBox
                ClearPictureBoxImage();

                //Load the image from the byte array
                pbImage.Image = ConvertBytesToImage(frames[lbFrames.SelectedIndex]);

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error type: " + ex.GetType().ToString() + "\n" +
                    "Message: " + ex.Message,
                    "Error in " + MethodBase.GetCurrentMethod().Name
                    );
            }
        }

    }
}
