using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Accounting.Utility
{
    internal class ImageCompress
    {
        /// <summary>
        /// Compress image with ImageCodecInfo and EncoderParameters, return compress bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        public static Bitmap Compress(Bitmap bitmap, long quality)
        {
            ImageCodecInfo jpgEncoder = _GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;

            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, jpgEncoder, myEncoderParameters);

            return new Bitmap(memoryStream);
        }
        /// <summary>
        /// Compress image with graphics drawimage feature.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="weight"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap Compress(Bitmap bitmap, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(bitmap, 0, 0, width, height);
            }
            return resizedImage;
        }


        private static ImageCodecInfo _GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
