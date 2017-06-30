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

        public void SaveImage(string URL, ImageFormat format, int carId)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(URL);

                MemoryStream mem = new MemoryStream(data);

                Bitmap yourImage = (Bitmap)Image.FromStream(mem);


                Bitmap smallImage = ResizeImage(yourImage, 310, 174);

                Bitmap largeImage = ResizeImage(yourImage, 640, 348);
                string fullPath = HttpContext.Current.Server.MapPath("~"+System.Web.Configuration.WebConfigurationManager.AppSettings["ImagePath"]
                    .ToString());

                if (format == ImageFormat.Png)
                {
                    // If you want it as Png

                    smallImage.Save("/cars/s_car_" + Convert.ToString(carId) + ".png", ImageFormat.Png);
                    largeImage.Save("/cars/l_car_" + Convert.ToString(carId) + ".png", ImageFormat.Png);
                }
                if (format == ImageFormat.Jpeg)
                {
                    // If you want it as Jpeg
                    // System.IO.File.WriteAllText("C:/Users/ashutosh.sharma/Documents", "Testing valid path & permissions.");
                    
                    smallImage.Save(fullPath + "s_car_" + Convert.ToString(carId) + ".jpeg", ImageFormat.Jpeg);
                    largeImage.Save(fullPath + "l_car_" + Convert.ToString(carId) + ".jpeg", ImageFormat.Jpeg);
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
