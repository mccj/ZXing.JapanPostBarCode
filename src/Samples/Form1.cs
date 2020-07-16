using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZXing;
using ZXing.JapanPost;

namespace LibExecuter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var code = BarCode.ConvertCustomerBarCode(textBox1.Text, textBox2.Text);

            //pictureBox1.Image = CreatetBarcode("0123456789-ABCDEFGHIJKMNOPQRSTUVWXYZ");
            //pictureBox1.Image = CreatetBarcode("1234567ABC".PadRight(22));
            //pictureBox1.Image = CreatetBarcode("15400233-16-4-205".PadRight(22));
            pictureBox1.Image = CreatetBarcode(code);

            //var s1 = "27385601111         ";
            //var s2 = "81200246-13-4F     ";
            //var b1 = CreatetBarcode(s1, 0, 100, null);
            //var b2 = CreatetBarcode(s2, 0, 100, null);
            //var bb = new System.Drawing.Bitmap(Math.Max(b1.Width, b2.Width), b1.Height + b2.Height + b2.Height + 50);
            //var x = System.Drawing.Graphics.FromImage(bb);
            //x.Clear(System.Drawing.Color.Red);
            //x.DrawImage(b1, 0, 0);
            //x.DrawString(s1.Replace(" ", "-"), System.Drawing.SystemFonts.DefaultFont, System.Drawing.Brushes.Black, 20, 100);
            //x.DrawImage(b2, 0, 120);
            //var img = System.Drawing.Image.FromFile(@"C:\Users\mccj\Desktop\zip_8120024 6-13-4F.png");
            //x.DrawImage(img, 33, 230, b2.Width - 66, b2.Height);
            //x.DrawString(s2.Replace(" ", "-"), System.Drawing.SystemFonts.DefaultFont, System.Drawing.Brushes.Black, 20, 330);
            //x.Dispose();
            //pictureBox1.Image = bb;
        }

        private Bitmap CreatetBarcode(string code, int? width = null, int? height = null, bool? pureBarcode = null)
        {
            //var code = BarCode.ConvertCustomerBarCode("0140113", "秋田県大仙市堀見内　南田茂木　添60-1");
            var writer = new ZXing.BarcodeWriter
            {
                //Format = ZXing.BarcodeFormat.CODE_128,
                Encoder = new JapanesePostalWriter()
            };
            writer.Options.PureBarcode = pureBarcode ?? false;
            writer.Options.Width = width ?? 0;// code.Length*2*100;
            writer.Options.Height = height ?? 300;// 3*100;
            return writer.Write(code);
        }
    }
}
