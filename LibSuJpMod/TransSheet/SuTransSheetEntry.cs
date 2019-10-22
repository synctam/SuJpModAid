namespace LibSuJpMod.TransSheet
{
    using System;

    /// <summary>
    /// 翻訳シートエントリー
    /// </summary>
    public class SuTransSheetEntry
    {
        public string EpisodeID { get; set; }

        public string FyleType { get; set; }

        public string ID { get; set; }

        public string Type { get; set; }

        public string English { get; set; }

        public string Japanese { get; set; }

        public string MT { get; set; }

        public int Sequence { get; set; }

        /// <summary>
        /// 日本語テキスト、機械翻訳テキストを元に翻訳済みのテキストを返す。
        /// </summary>
        /// <param name="textJP">日本語テキスト</param>
        /// <param name="textMT">機械翻訳テキスト</param>
        /// <param name="useMT">機械翻訳の使用有無</param>
        /// <returns>翻訳済みのテキスト</returns>
        public string Translate(string textJP, string textMT, bool useMT)
        {
            //// en | jp | mt | result
            ////  o |  o |  o | jp
            ////  o |  o |  x | jp
            ////  o |  x |  o | mt <-> en
            ////  o |  x |  x | en
            ////  x |  o |  o | en
            ////  x |  o |  x | en
            ////  x |  x |  o | en
            ////  x |  x |  x | en
            bool en = !string.IsNullOrWhiteSpace(this.English);
            bool jp = !string.IsNullOrWhiteSpace(textJP);
            bool mt = !string.IsNullOrWhiteSpace(textMT);
            if (en && jp && mt)
            {
                return textJP;
            }
            else if (en && jp && !mt)
            {
                return textJP;
            }
            else if (en && !jp && mt)
            {
                if (useMT)
                {
                    return textMT;
                }
                else
                {
                    return this.English;
                }
            }
            else if (en && !jp && !mt)
            {
                return this.English;
            }
            else if (!en && jp && mt)
            {
                return this.English;
            }
            else if (!en && jp && !mt)
            {
                return this.English;
            }
            else if (!en && !jp && mt)
            {
                return this.English;
            }
            else if (!en && !jp && !mt)
            {
                return this.English;
            }
            else
            {
                throw new Exception($"logic error.");
            }
        }
    }
}
