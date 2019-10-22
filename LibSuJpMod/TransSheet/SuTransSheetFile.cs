namespace LibSuJpMod.TransSheet
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 翻訳シートファイル
    /// </summary>
    public class SuTransSheetFile
    {
        /// <summary>
        /// 言語エントリーの辞書。
        /// キーは言語エントリのID。
        /// </summary>
        public Dictionary<string, SuTransSheetEntry> Items { get; } =
            new Dictionary<string, SuTransSheetEntry>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 言語エントリーを追加する。
        /// </summary>
        /// <param name="entry">言語エントリー</param>
        public void AddEntry(SuTransSheetEntry entry)
        {
            if (this.Items.ContainsKey(entry.ID))
            {
                throw new Exception($"Duplicate key({entry.ID})");
            }
            else
            {
                this.Items.Add(entry.ID, entry);
            }
        }

        /// <summary>
        /// 指定されたキーの翻訳シートエントリーを返す。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>翻訳シートエントリー</returns>
        public SuTransSheetEntry GetEntry(string id)
        {
            if (this.Items.ContainsKey(id))
            {
                return this.Items[id];
            }
            else
            {
                //// 存在しない時は null object を返す。
                return new SuTransSheetEntry();
            }
        }

        /// <summary>
        /// 指定したIDの翻訳済みのテキストを返す。
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="textEN">原文</param>
        /// <param name="useMT">機械翻訳の有無</param>
        /// <returns>翻訳済みのテキスト</returns>
        public string Translate(string id, string textEN, bool useMT)
        {
            var sheetEntry = this.GetEntry(id);
            var translatedText = sheetEntry.Translate(sheetEntry.Japanese, sheetEntry.MT, useMT);

            return translatedText;
        }
    }
}
