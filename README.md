# ZXing.JapanPostBarCode
郵便番号、住所から郵便物用のカスタマバーコードを生成する

# Samples
```C#
            var code = BarCode.ConvertCustomerBarCode("0140113", "秋田県大仙市堀見内　南田茂木　添60-1");
            var writer = new ZXing.BarcodeWriter
            {
                Encoder = new JapanPostWriter()
            };
            writer.Options.PureBarcode = false;
            writer.Options.Width = 0;
            writer.Options.Height = 300;
            // pictureBox1.Image = writer.Write("0123456789-ABCDEFGHIJKMNOPQRSTUVWXYZ");
            // pictureBox1.Image = writer.Write("1234567ABC".PadRight(22));
            // pictureBox1.Image = writer.Write("15400233-16-4-205".PadRight(22));
            pictureBox1.Image = writer.Write(code);
```
