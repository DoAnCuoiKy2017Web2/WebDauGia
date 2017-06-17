using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WebDauGia.Helper
{
    public class Picture
    {
        public static bool SaveResizeImage(Image img, int width, int height, string path)
        {
            try
            {
                Bitmap b = new Bitmap(width, height);
                Graphics g = Graphics.FromImage((Image)b);
                g.InterpolationMode = InterpolationMode.Bicubic;    // Specify here
                g.DrawImage(img, 0, 0, width, height);
                g.Dispose();
                b.Save(path);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}