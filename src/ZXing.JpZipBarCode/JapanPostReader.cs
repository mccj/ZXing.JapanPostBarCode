//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using ZXing.Common;

//namespace ZXing.JapanPost
//{
//    /// <summary>
//    /// 
//    /// https://www.post.japanpost.jp/zipcode/zipmanual/p11.html
//    /// </summary>
//    public class JapanPostReader : Reader
//    {
//        public Result decode(BinaryBitmap image)
//        {
//            return decode(image, null);
//        }

//        public Result decode(BinaryBitmap image, IDictionary<DecodeHintType, object> hints)
//        {
//            if (image == null || image.BlackMatrix == null)
//            {
//                // something is wrong with the image
//                return null;
//            }
//            //var dfsdfsd = System.Linq.Enumerable.Range(0, image.Height).Select(y => image.getBlackRow(y, null)).Where(f=>f.Array.Any(ff=>ff>0)).Distinct().ToArray();

//        //var ssdfsd=    dfsdfsd.Select(f => f.Array.Select(ff => ff > 0).ToArray()).ToArray();

//      //var dffff=      BitMatrix.parse(ssdfsd);
//            var sss = Render(image.BlackMatrix);
//            //var sss = Render(dffff);
//            //var sss = Render(image.rotateCounterClockwise().BlackMatrix);
//            sss.Save("d://aa.png");

//            return null;


//            var result = new Result("tttttttttttttttttt", null, null, 0);


//            return result;
//        }
//        public Bitmap Render(BitMatrix matrix)
//        {
//            int t = 10;
//            int width = matrix.Width;
//            int height = matrix.Height;

//            var bmp = new Bitmap(width * t, height * t);
//            var gg = Graphics.FromImage(bmp);
//            gg.Clear(System.Drawing.Color.DarkGray);

//            for (int x = 0; x < width - 1; x++)
//            {
//                for (int y = 0; y < height - 1; y++)
//                {
//                    if (matrix[x, y])
//                    {
//                        gg.FillRectangle(System.Drawing.Brushes.Black, x * t, y * t, 1*t, 1 * t);
//                    }
//                    else
//                    {
//                        gg.FillRectangle(System.Drawing.Brushes.DarkGray, x * t, y * t, 1 * t, 1 * t);
//                    }
//                }
//            }

//            return bmp;
//        }

//        public void reset() { }
//    }
//}
