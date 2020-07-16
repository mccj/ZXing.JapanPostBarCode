using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text.RegularExpressions;

namespace LibExecuter
{
    public static class BarCode
    {
        // 全角→半角辞書
        private static readonly Dictionary<char, char> HankakuDic = new Dictionary<char, char>() {
            {'１','1'},{'２','2'},{'３','3'},{'４','4'},{'５','5'},
            {'６','6'},{'７','7'},{'８','8'},{'９','9'},{'０','0'},
            {'Ａ','A'},{'Ｂ','B'},{'Ｃ','C'},{'Ｄ','D'},{'Ｅ','E'},
            {'Ｆ','F'},{'Ｇ','G'},{'Ｈ','H'},{'Ｉ','I'},{'Ｊ','J'},
            {'Ｋ','K'},{'Ｌ','L'},{'Ｍ','M'},{'Ｎ','N'},{'Ｏ','O'},
            {'Ｐ','P'},{'Ｑ','Q'},{'Ｒ','R'},{'Ｓ','S'},{'Ｔ','T'},
            {'Ｕ','U'},{'Ｖ','V'},{'Ｗ','W'},{'Ｘ','X'},{'Ｙ','Y'},
            {'Ｚ','Z'},
            {'ａ','a'},{'ｂ','b'},{'ｃ','c'},{'ｄ','d'},{'ｅ','e'},
            {'ｆ','f'},{'ｇ','g'},{'ｈ','h'},{'ｉ','i'},{'ｊ','j'},
            {'ｋ','k'},{'ｌ','l'},{'ｍ','m'},{'ｎ','n'},{'ｏ','o'},
            {'ｐ','p'},{'ｑ','q'},{'ｒ','r'},{'ｓ','s'},{'ｔ','t'},
            {'ｕ','u'},{'ｖ','v'},{'ｗ','w'},{'ｘ','x'},{'ｙ','y'},
            {'ｚ','z'},
            {'　',' '},{'‐','-'}
        };

        private static readonly string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";


        /// <summary>
        /// 郵便番号チェック
        /// ハイフンを除いて数値7桁であること
        /// </summary>
        /// <param name="zipCode">郵便番号</param>
        /// <returns>郵便番号配列</returns>
        private static string CheckPostCode(string zipCode)
        {
            // 全角→半角
            zipCode = string.Join("", zipCode.Select(n => (HankakuDic.ContainsKey(n) ? HankakuDic[n] : n)));

            // ハイフン除去
            zipCode = zipCode.Replace("-", "");

            if (!Regex.IsMatch(zipCode, @"\A\d{7}\Z"))
            {
                throw new ArgumentException("郵便番号は7桁の数値で入力してください。");
            }

            return zipCode;
        }


        /// <summary>
        /// カスタマバーコード変換
        /// </summary>
        /// <param name="address">住所</param>
        /// <param name="zipCode">郵便番号</param>
        /// <returns></returns>
        public static string ConvertCustomerBarCode(string zipCode, string address)
        {
            // 全角→半角
            address = string.Join("", address.Select(n => (HankakuDic.ContainsKey(n) ? HankakuDic[n] : n)));

            // http://www.post.japanpost.jp/zipcode/zipmanual/p19.html
            // 1. まず、データ内にあるアルファベットの小文字は大文字に置き換えます。
            address = address.ToUpper();

            // 2. 同様に、データ内にある"&"等の下記の文字は取り除き、後ろのデータを詰めます。
            //「&」(アンパサンド)、「/」(スラッシュ)、「・」(中グロ)、「.」(ピリオド)
            address = string.Join("", address.Split('&', '＆', '/', '／', '･', '・', '.', '．'));

            // 3. 1および2で整理したデータから、算用数字、ハイフンおよび連続していないアルファベット1文字を必要な文字情報として抜き出します。
            // 4. 次に抜き出された文字の前にある下記の文字等は、ハイフン1文字に置き換えます。
            // 「漢字」、「かな文字」、「カタカナ文字」、「漢数字」、「ブランク」、「2文字以上連続したアルファベット文字」
            address = Regex.Replace(address, "[^0-9A-Z\\-]|[A-Z]{2,}", "-");

            // ハイフン処理
            address = TrimHyphen(address);
            var letterCount = address.Select(f => char.IsLetter(f) ? 1 : 0).Sum();
            return CheckPostCode(zipCode) + address.PadRight(13 - letterCount);
        }

        /// <summary>
        /// ハイフン処理
        /// </summary>
        /// <param name="target">処理対象文字列</param>
        /// <returns>処理後文字列</returns>
        private static string TrimHyphen(string target)
        {
            // 5. 4の置き換えで、ハイフンが連続する場合は1つにまとめます。
            target = Regex.Replace(target, "\\-{2,}", "-");

            //アルファベット前後のハイフンを除去
            var tmp = target.ToCharArray();
            for (int i = 0; i < target.Length; i++)
            {
                if (Alphabet.Contains(target[i].ToString()))
                {
                    if (i > 0 && target[i - 1] == '-') tmp[i - 1] = '@';
                    if (i < target.Length - 1 && target[i + 1] == '-') tmp[i + 1] = '@';
                }
            }
            target = (new string(tmp)).Replace("@", "");

            // 6. 最後に、先頭がハイフンの場合は取り除きます。            
            return target.Trim('-');
        }
    }
}
