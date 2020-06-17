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
            //var code = BarCode.ConvertCustomerBarCode("0140113", "秋田県大仙市堀見内　南田茂木　添60-1");
            var writer = new ZXing.BarcodeWriter
            {
                //Format = ZXing.BarcodeFormat.CODE_128,
                Encoder = new JapanesePostalWriter()
            };
            writer.Options.PureBarcode = false;
            writer.Options.Width = 0;// code.Length*2*100;
            writer.Options.Height = 300;// 3*100;
            //pictureBox1.Image = writer.Write("0123456789-ABCDEFGHIJKMNOPQRSTUVWXYZ");
            //pictureBox1.Image = writer.Write("1234567ABC".PadRight(22));
            //pictureBox1.Image = writer.Write("15400233-16-4-205".PadRight(22));
            pictureBox1.Image = writer.Write(code);
        }
    }
}
