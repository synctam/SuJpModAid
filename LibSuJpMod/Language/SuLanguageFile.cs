namespace LibSuJpMod.Language
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 言語ファイル
    /// </summary>
    public class SuLanguageFile
    {
        /// <summary>
        /// コンスタント
        /// </summary>
        /// <param name="episodeID">エピソードフォルダー名</param>
        /// <param name="fileType">ファイルタイプ</param>
        public SuLanguageFile(string episodeID, NFileType fileType)
        {
            this.EpisodeID = episodeID;
            this.FileType = fileType;
        }

        /// <summary>
        /// XMLファイルの区分
        /// </summary>
        public enum NFileType
        {
            /// <summary>
            /// None
            /// </summary>
            None,

            /// <summary>
            /// Dialog
            /// </summary>
            Dialog,

            /// <summary>
            /// UI
            /// </summary>
            UI,
        }

        /// <summary>
        /// 言語エントリーの辞書。
        /// キーはID
        /// </summary>
        public Dictionary<string, SuLanguageEntry> Items { get; } =
            new Dictionary<string, SuLanguageEntry>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// エピソードフォルダー名
        /// </summary>
        public string EpisodeID { get; } = string.Empty;

        /// <summary>
        /// File type
        /// </summary>
        public NFileType FileType { get; } = NFileType.None;

        /// <summary>
        /// ファイルのキーを返す。
        /// </summary>
        public string FileKey
        {
            get
            {
                var result = $"{this.EpisodeID}:{this.FileTypeString}";
                return result;
            }
        }

        /// <summary>
        /// ファイルタイプをテキスト形式で返す。
        /// </summary>
        public string FileTypeString
        {
            get
            {
                return this.GetFileTypeStrint(this.FileType);
            }
        }

        /// <summary>
        /// 指定したFileKeyのEpisodeIDを返す。
        /// </summary>
        /// <param name="fileKey">FileKey</param>
        /// <returns>EpisodeID</returns>
        public string GetEpisodeID(string fileKey)
        {
            var keys = fileKey.Split(':');
            if (keys.Length != 2)
            {
                throw new Exception($"Invalid fileKey({fileKey})");
            }
            else
            {
                return keys[0];
            }
        }

        /// <summary>
        /// 指定したFileKeyのFileTypeを返す。
        /// </summary>
        /// <param name="fileKey">FileKey</param>
        /// <returns>FileType</returns>
        public NFileType GetFileType(string fileKey)
        {
            var keys = fileKey.Split(':');
            if (keys.Length != 2)
            {
                throw new Exception($"Invalid fileKey({fileKey})");
            }
            else
            {
                var fileTypeString = keys[1];
                switch (fileTypeString)
                {
                    case "DIALOGUE":
                        return NFileType.Dialog;
                    case "UI":
                        return NFileType.UI;
                    default:
                        return NFileType.None;
                }
            }
        }

        /// <summary>
        /// FileTypeをテキスト形式で返す。
        /// </summary>
        /// <param name="fileType">FileType</param>
        /// <returns>テキスト形式のFileType</returns>
        public string GetFileTypeStrint(NFileType fileType)
        {
            switch (fileType)
            {
                case NFileType.None:
                    throw new Exception($"File type is none.");
                case NFileType.Dialog:
                    return "DIALOGUE";
                case NFileType.UI:
                    return "UI";
                default:
                    throw new Exception($"unknown error: fileType({this.FileType.ToString()})");
            }
        }

        /// <summary>
        /// 言語エントリーを追加する。
        /// </summary>
        /// <param name="langEntry">言語エントリー</param>
        public void AddEntry(SuLanguageEntry langEntry)
        {
            if (this.Items.ContainsKey(langEntry.ID))
            {
                throw new Exception($"Duplicate key({langEntry.ID})");
            }
            else
            {
                this.Items.Add(langEntry.ID, langEntry);
            }
        }
    }
}
