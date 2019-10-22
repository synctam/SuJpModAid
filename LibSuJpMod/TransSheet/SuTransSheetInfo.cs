namespace LibSuJpMod.TransSheet
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 翻訳シート情報
    /// </summary>
    public class SuTransSheetInfo
    {
        /// <summary>
        /// 言語ファイルの辞書。
        /// キーはエピソード。
        /// </summary>
        public Dictionary<string, SuTransSheetFile> Items { get; } =
            new Dictionary<string, SuTransSheetFile>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 翻訳シートエントリーを追加する。
        /// </summary>
        /// <param name="episodeID">EpisodeID</param>
        /// <param name="entry">翻訳シートエントリー</param>
        public void AddEntry(string episodeID, SuTransSheetEntry entry)
        {
            if (this.Items.ContainsKey(episodeID))
            {
                var transSheetFile = this.Items[episodeID];
                transSheetFile.AddEntry(entry);
            }
            else
            {
                //// 翻訳シートファイルが存在しない場合は新たに作成する。
                var transSheetFile = new SuTransSheetFile();
                transSheetFile.AddEntry(entry);

                this.Items.Add(episodeID, transSheetFile);
            }
        }

        /// <summary>
        /// 指定された EpisodeID の翻訳シートファイルを返す。
        /// </summary>
        /// <param name="episodeID">EpisodeID</param>
        /// <returns>翻訳シートファイル</returns>
        public SuTransSheetFile GetFile(string episodeID)
        {
            if (this.Items.ContainsKey(episodeID))
            {
                return this.Items[episodeID];
            }
            else
            {
                return new SuTransSheetFile();
            }
        }

        /// <summary>
        /// 翻訳シートエントリーを返す。
        /// </summary>
        /// <param name="episodeID">EpisodeID</param>
        /// <param name="key">キー</param>
        /// <returns>翻訳シートエントリー</returns>
        public SuTransSheetEntry GetEntry(string episodeID, string key)
        {
            return this.GetFile(episodeID).GetEntry(key);
        }

        /// <summary>
        /// 翻訳済みテキストを返す。
        /// </summary>
        /// <param name="episodeID">EpisodeID</param>
        /// <param name="id">ID</param>
        /// <param name="textEN">原文</param>
        /// <param name="useMT">機械翻訳の有無</param>
        /// <returns>翻訳済みテキスト</returns>
        public string Translate(string episodeID, string id, string textEN, bool useMT)
        {
            var sheetFile = this.GetFile(episodeID);
            var translatedText = sheetFile.Translate(id, textEN, useMT);

            return translatedText;
        }
    }
}
