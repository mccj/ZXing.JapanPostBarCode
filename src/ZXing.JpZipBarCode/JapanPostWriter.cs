using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ZXing.Common;

namespace ZXing.JapanPost
{
    /// <summary>
    /// 只能编码 字母数字（0-9，AZ）和连字符（-）
    /// https://www.post.japanpost.jp/zipcode/zipmanual/p11.html
    /// </summary>
    public class JapanesePostalWriter : Writer
    {
        // 文字→バーコード辞書
        private static readonly Dictionary<char, ImageKey[]> _barcodeCharDic = new Dictionary<char, ImageKey[]>()
        {
            {'A',new []{ImageKey.CC1,ImageKey.Num0}},
            {'B',new []{ImageKey.CC1,ImageKey.Num1}},
            {'C',new []{ImageKey.CC1,ImageKey.Num2}},
            {'D',new []{ImageKey.CC1,ImageKey.Num3}},
            {'E',new []{ImageKey.CC1,ImageKey.Num4}},
            {'F',new []{ImageKey.CC1,ImageKey.Num5}},
            {'G',new []{ImageKey.CC1,ImageKey.Num6}},
            {'H',new []{ImageKey.CC1,ImageKey.Num7}},
            {'I',new []{ImageKey.CC1,ImageKey.Num8}},
            {'J',new []{ImageKey.CC1,ImageKey.Num9}},
            {'K',new []{ImageKey.CC2,ImageKey.Num0}},
            {'L',new []{ImageKey.CC2,ImageKey.Num1}},
            {'M',new []{ImageKey.CC2,ImageKey.Num2}},
            {'N',new []{ImageKey.CC2,ImageKey.Num3}},
            {'O',new []{ImageKey.CC2,ImageKey.Num4}},
            {'P',new []{ImageKey.CC2,ImageKey.Num5}},
            {'Q',new []{ImageKey.CC2,ImageKey.Num6}},
            {'R',new []{ImageKey.CC2,ImageKey.Num7}},
            {'S',new []{ImageKey.CC2,ImageKey.Num8}},
            {'T',new []{ImageKey.CC2,ImageKey.Num9}},
            {'U',new []{ImageKey.CC3,ImageKey.Num0}},
            {'V',new []{ImageKey.CC3,ImageKey.Num1}},
            {'W',new []{ImageKey.CC3,ImageKey.Num2}},
            {'X',new []{ImageKey.CC3,ImageKey.Num3}},
            {'Y',new []{ImageKey.CC3,ImageKey.Num4}},
            {'Z',new []{ImageKey.CC3,ImageKey.Num5}},
            {'0',new []{ImageKey.Num0}},
            {'1',new []{ImageKey.Num1}},
            {'2',new []{ImageKey.Num2}},
            {'3',new []{ImageKey.Num3}},
            {'4',new []{ImageKey.Num4}},
            {'5',new []{ImageKey.Num5}},
            {'6',new []{ImageKey.Num6}},
            {'7',new []{ImageKey.Num7}},
            {'8',new []{ImageKey.Num8}},
            {'9',new []{ImageKey.Num9}},
            {'-',new []{ ImageKey.Hyphen }},
            {' ',new []{ ImageKey.CC4 }},
            {'\t',new []{ ImageKey.CC4 }}
        };
        // 画像リソースキー辞書
        private static readonly Dictionary<ImageKey, int[]> _imageKeyDic = new Dictionary<ImageKey, int[]>()
        {
            {ImageKey.Num1,new[]{1,1,4}},
            {ImageKey.Num2,new[]{1,3,2}},
            {ImageKey.Num3,new[]{3,1,2}},
            {ImageKey.Num4,new[]{1,2,3}},
            {ImageKey.Num5,new[]{1,4,1}},
            {ImageKey.Num6,new[]{3,2,1}},
            {ImageKey.Num7,new[]{2,1,3}},
            {ImageKey.Num8,new[]{2,3,1}},
            {ImageKey.Num9,new[]{4,1,1}},
            {ImageKey.Num0,new[]{1,4,4}},
            {ImageKey.Hyphen,new[]{4,1,4}},//-
            {ImageKey.CC1,new[]{3,2,4}},
            {ImageKey.CC2,new[]{3,4,2}},
            {ImageKey.CC3,new[]{2,3,4}},
            {ImageKey.CC4,new[]{4,3,2}},
            {ImageKey.CC5,new[]{2,4,3}},
            {ImageKey.CC6,new[]{4,2,3}},
            {ImageKey.CC7,new[]{4,4,1}},
            {ImageKey.CC8,new[]{1,1,1}},
            {ImageKey.Start,new[]{0,1,3}},
            {ImageKey.End,new[]{3,1,0}},
        };

        // チェックデジット計算用辞書
        private static readonly Dictionary<ImageKey, int> _checkSumDic = new Dictionary<ImageKey, int>()
        {
            {ImageKey.Num0,0},
            {ImageKey.Num1,1},
            {ImageKey.Num2,2},
            {ImageKey.Num3,3},
            {ImageKey.Num4,4},
            {ImageKey.Num5,5},
            {ImageKey.Num6,6},
            {ImageKey.Num7,7},
            {ImageKey.Num8,8},
            {ImageKey.Num9,9},
            {ImageKey.Hyphen,10},
            {ImageKey.CC1,11},
            {ImageKey.CC2,12},
            {ImageKey.CC3,13},
            {ImageKey.CC4,14},
            {ImageKey.CC5,15},
            {ImageKey.CC6,16},
            {ImageKey.CC7,17},
            {ImageKey.CC8,18}
        };
        /// <summary>
        /// Encode a barcode using the default settings.
        /// </summary>
        /// <param name="contents">The contents to encode in the barcode</param>
        /// <param name="format">The barcode format to generate</param>
        /// <param name="width">The preferred width in pixels</param>
        /// <param name="height">The preferred height in pixels</param>
        /// <returns>
        /// The generated barcode as a Matrix of unsigned bytes (0 == black, 255 == white)
        /// </returns>
        public BitMatrix encode(string contents, BarcodeFormat format, int width, int height)
        {
            return encode(contents, format, width, height, null);
        }
        /// <summary>
        /// </summary>
        /// <param name="contents">The contents to encode in the barcode</param>
        /// <param name="format">The barcode format to generate</param>
        /// <param name="width">The preferred width in pixels</param>
        /// <param name="height">The preferred height in pixels</param>
        /// <param name="hints">Additional parameters to supply to the encoder</param>
        /// <returns>
        /// The generated barcode as a Matrix of unsigned bytes (0 == black, 255 == white)
        /// </returns>
        public BitMatrix encode(string contents, BarcodeFormat format, int width, int height, IDictionary<EncodeHintType, object> hints)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(contents, @"\A[\d\w- ]*\Z"))
            {
                throw new ArgumentException("编码内容只能是 字母数字（0-9，AZ）和连字符（-）、占位付 空格。");
            }
            contents = contents.ToUpper();
            //TODO:字符检查
            var code = contents.SelectMany(c => _barcodeCharDic[c]).ToArray();
            var code2 = code.SelectMany(f => _imageKeyDic[f]).ToArray();
            var list = new List<int>();
            list.AddRange(_imageKeyDic[ImageKey.Start]);//开始字符
            list.AddRange(code2);//内容
            list.AddRange(_imageKeyDic[CalcCheckDigit(code)]);//效验码
            list.AddRange(_imageKeyDic[ImageKey.End]);//结束字符

            return renderResult(list.ToArray(), width, height);
        }
        /// <summary>
        /// チェックデジット計算
        /// </summary>
        /// <param name="zipCode">郵便番号</param>
        /// <param name="customerCode">住所表示番号</param>
        /// <returns>チェックデジット</returns>
        private ImageKey CalcCheckDigit(ImageKey[] zipCode)
        {
            // 郵便番号の各値と住所表示番号の各値の合計＋CD=19の倍数
            var sum = zipCode
                .Select(x => _checkSumDic[x]).Sum();

            if (sum % 19 == 0) return ImageKey.Num0;

            var mod = sum / 19;
            var checkD = (mod + 1) * 19 - sum;
            return _checkSumDic.Where(x => x.Value == checkD).First().Key;
        }
        private BitMatrix renderResult(int[] code, int width, int height)
        {
            int outputWidth = width <= 0 && height > 0 ? -1 : Math.Max( width, code.Length * 2);
            int outputHeight = height <= 0 && width > 0 ? -1 : Math.Max(height, 6);
            int multiple = Math.Min(outputHeight == -1 ? int.MaxValue : outputHeight / 6, outputWidth == -1 ? int.MaxValue : outputWidth / code.Length / 2);

            var output = new BitMatrix(outputWidth == -1 ? multiple * code.Length * 2 : outputWidth, outputHeight == -1 ? multiple * 6 : outputHeight);

            int leftPadding = Math.Max(0, (outputWidth - (code.Length * 2 * multiple)) / 2);
            int topPadding = Math.Max(0, (outputHeight - (6 * multiple)) / 2);


            foreach (var c in code.Select((x, i) => new { Value = x, Index = i }))
            {
                draw(output, c.Index, c.Value, multiple, leftPadding, topPadding);
            }
            return output;
        }
        /// <summary>
        /// https://www.post.japanpost.jp/zipcode/zipmanual/p11.html
        /// 条码形状
        /// </summary>
        /// <param name="output"></param>
        /// <param name="index"></param>
        /// <param name="ll"></param>
        private void draw(BitMatrix output, int index, int value, int multiple, int leftPadding, int topPadding)
        {
            if (multiple < 1) throw (new Exception($"{nameof(multiple)} 必须是必须大于0"));
            switch (value)
            {
                //case 0:
                //    break;
                //case 1:
                //    output.setRegion(index * 12, 0, 6, 36);
                //    break;
                //case 2:
                //    output.setRegion(index * 12, 0, 6, 24);
                //    break;
                //case 3:
                //    output.setRegion(index * 12, 12, 6, 24);
                //    break;
                //case 4:
                //    output.setRegion(index * 12, 12, 6, 12);
                //    break;
                case 0:
                    break;
                case 1:
                    output.setRegion((index * 2 * multiple) + leftPadding, (0 * multiple) + topPadding, 1 * multiple, 6 * multiple);
                    break;
                case 2:
                    output.setRegion((index * 2 * multiple) + leftPadding, (0 * multiple) + topPadding, 1 * multiple, 4 * multiple);
                    break;
                case 3:
                    output.setRegion((index * 2 * multiple) + leftPadding, (2 * multiple) + topPadding, 1 * multiple, 4 * multiple);
                    break;
                case 4:
                    output.setRegion((index * 2 * multiple) + leftPadding, (2 * multiple) + topPadding, 1 * multiple, 2 * multiple);
                    break;

                default:
                    throw new Exception($"{nameof(value)} 必须是0-4的数字");
            }

        }

        enum ImageKey
        {
            Num0,
            Num1,
            Num2,
            Num3,
            Num4,
            Num5,
            Num6,
            Num7,
            Num8,
            Num9,
            /// <summary>
            /// -
            /// </summary>
            Hyphen,//-
            CC1,
            CC2,
            CC3,
            CC4,
            CC5,
            CC6,
            CC7,
            CC8,
            Start,
            End
        }
    }
}
