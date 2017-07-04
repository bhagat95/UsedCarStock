using System;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace UsedCarBL
{
    public class ImageProcessorRmq
    {
        string ImagePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagePath"].ToString();
        string SmallImageFileName = System.Web.Configuration.WebConfigurationManager.AppSettings["SmallImageFileName"].ToString();
        string LargeImageFileName = System.Web.Configuration.WebConfigurationManager.AppSettings["LargeImageFileName"].ToString();
        public void SaveImage(string URL, ImageFormat format, int carId)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(URL);
                MemoryStream mem = new MemoryStream(data);
                Bitmap yourImage = (Bitmap)Image.FromStream(mem);
                Bitmap smallImage = ResizeImage(yourImage, 310, 174);
                Bitmap largeImage = ResizeImage(yourImage, 640, 348);
                
                string fullPath = HttpContext.Current.Server.MapPath("~"+ ImagePath);
                if (format == ImageFormat.Png)
                {
                    smallImage.Save(fullPath + SmallImageFileName + Convert.ToString(carId) + ".png", ImageFormat.Png);
                    largeImage.Save(fullPath + LargeImageFileName + Convert.ToString(carId) + ".png", ImageFormat.Png);
                }
                if (format == ImageFormat.Jpeg)
                {
                    smallImage.Save(fullPath + SmallImageFileName + Convert.ToString(carId) + ".jpeg", ImageFormat.Jpeg);
                    largeImage.Save(fullPath + LargeImageFileName + Convert.ToString(carId) + ".jpeg", ImageFormat.Jpeg);
                }
                mem.Dispose();
            }
        }
        public Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }
    }
}
