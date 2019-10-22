namespace LibSuJpMod.Language
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 言語情報
    /// </summary>
    public class SuLanguageInfo
    {
        /// <summary>
        /// 言語ファイルの辞書。
        /// キーは FileKey。
        /// </summary>
        public Dictionary<string, SuLanguageFile> Items { get; } =
            new Dictionary<string, SuLanguageFile>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 言語ファイルを追加する。
        /// </summary>
        /// <param name="langFile">言語ファイル</param>
        public void AddFile(SuLanguageFile langFile)
        {
            if (this.Items.ContainsKey(langFile.FileKey))
            {
                throw new Exception($"Duplicate EpisodeFolderName({langFile.EpisodeID})");
            }
            else
            {
                this.Items.Add(langFile.FileKey, langFile);
            }
        }
    }
}
