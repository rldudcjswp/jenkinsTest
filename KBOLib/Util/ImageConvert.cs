using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;


namespace KBOLib.Util
{
    public class ImageConvert
    {
        #region 변수 선언
        public int max_width { get; set; }
        public int max_height { get; set; }
        public int quality { get; set; }
        #endregion

        public ImageConvert(int width, int height, int quality)
        {
            this.max_width = width;
            this.max_height = height;
            this.quality = quality;
        }

        /// <summary>
        /// 이미지 압축
        /// </summary>
        public byte[] Resize(HttpPostedFile postedFile)
        {
            if (postedFile.ContentLength == 0)
            {
                return new byte[0];
            }

            System.Drawing.Image sourceImage = System.Drawing.Image.FromStream(postedFile.InputStream);
            System.Drawing.Image convert = Resize(sourceImage);
            sourceImage.Dispose();

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)this.quality);
            ImageCodecInfo encoder = GetEncoder(ImageFormat.Jpeg);

            MemoryStream stream = new MemoryStream();
            convert.Save(stream, encoder, encoderParams);
            byte[] buffer = stream.GetBuffer();
            convert.Dispose();
            stream.Close();
            return buffer;
        }

        /// <summary>
        /// 이미지 리사이즈
        /// </summary>
        private System.Drawing.Image Resize(System.Drawing.Image sourceImage)
        {
            System.Drawing.Image source = new Bitmap(sourceImage);
            int width = sourceImage.Width;
            int height = sourceImage.Height;

            if (width > this.max_width)
            {
                height = (height * this.max_width) / width;
                width = this.max_width;
            }

            if (height > this.max_height)
            {
                width = (width * this.max_height) / height;
                height = this.max_height;
            }

            if (width != sourceImage.Width || height != sourceImage.Height)
            {
                source = new Bitmap(source, width, height);
            }

            return source;
        }


        /// <summary>
        /// 이미지 포맷
        /// </summary>
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
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
