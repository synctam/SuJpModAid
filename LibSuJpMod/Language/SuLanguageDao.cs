namespace LibSuJpMod.Language
{
    using System;
    using System.IO;
    using System.Xml.Linq;
    using LibCrsJpMod.FileUtils;
    using LibSuJpMod.TransSheet;

    /// <summary>
    /// 言語ファイルの入出力
    /// </summary>
    public class SuLanguageDao
    {
        /// <summary>
        /// 翻訳された言語ファイルを指定フォルダーに保存する。
        /// </summary>
        /// <param name="transSheet">翻訳シート情報</param>
        /// <param name="langFolderEN">原文のフォルダーのパス</param>
        /// <param name="langFolderJP">翻訳されたデータを格納するフォルダーのパス</param>
        /// <param name="useMT">機械翻訳の使用有無</param>
        /// <param name="useReplace">上書きの有無</param>
        public static void SaveToFolder(
            SuTransSheetInfo transSheet,
            string langFolderEN,
            string langFolderJP,
            bool useMT,
            bool useReplace)
        {
            var fullPath = Path.GetFullPath(langFolderEN);
            var di = new DirectoryInfo(fullPath);
            var files = di.GetFiles("*.xml", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                if (!Path.GetExtension(f.FullName)
                    .Equals(".xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                var episodeID = GetEpisodeID(f.FullName);
                var fileType = GetFileType(f.FullName);
                var rc = SaveToXml(
                    transSheet,
                    episodeID,
                    fileType,
                    f.FullName,
                    langFolderJP,
                    useMT,
                    useReplace);
                if (!rc)
                {
                    Console.WriteLine(
                        $"{f.FullName}が既に存在します。" +
                        $"{Environment.NewLine}このファイルの処理をスキップしました。");
                }
            }
        }

        /// <summary>
        /// 日本語化されたデータをXMLファイルに保存する。
        /// </summary>
        /// <param name="transSheet">翻訳シート情報</param>
        /// <param name="episodeID">EpisodeID</param>
        /// <param name="fileType">FileType</param>
        /// <param name="langFilePathEN">原文のXMLファイルのパス</param>
        /// <param name="langFolderPathJP">翻訳されたXMLファイルを格納するフォルダーのパス</param>
        /// <param name="useMT">機械翻訳の使用有無</param>
        /// <param name="useReplace">上書きの有無</param>
        /// <returns>エラーの有無</returns>
        public static bool SaveToXml(
            SuTransSheetInfo transSheet,
            string episodeID,
            SuLanguageFile.NFileType fileType,
            string langFilePathEN,
            string langFolderPathJP,
            bool useMT,
            bool useReplace)
        {
            var langFile = new SuLanguageFile(episodeID, fileType);

            //// 保存フォルダーを作成する。
            var saveFolder = Path.Combine(langFolderPathJP, langFile.EpisodeID);
            SuFileUtils.SafeCreateDirectory(saveFolder);
            //// XMLファイルを保存する。
            var fileName = Path.GetFileName(langFilePathEN);
            var savePath = Path.Combine(saveFolder, fileName);
            //// 出力ファイルの存在確認。
            if (!useReplace && File.Exists(savePath))
            {
                return false;
            }

            //// 原文を読み込む。
            var xml = XDocument.Load(langFilePathEN);
            XElement table = null;
            if (langFile.FileKey.Equals("E04:UI", StringComparison.OrdinalIgnoreCase))
            {
                //// E04のUI.xmlのみUIではなく DIALOGUE
                var fileTypeString =
                    langFile.GetFileTypeStrint(SuLanguageFile.NFileType.Dialog);
                table = xml.Element(fileTypeString);
            }
            else
            {
                table = xml.Element(langFile.FileTypeString);
            }

            //// 各エントリーのテキストを翻訳する。
            var rows = table.Elements("txt");
            foreach (var row in rows)
            {
                var attr = row.Attribute("id");
                var id = attr.Value;
                //// このエントリーの翻訳シートエントリーを取得する。
                var sheetEntry = transSheet.GetEntry(langFile.EpisodeID, id);
                //// 翻訳する。
                string text =
                    transSheet.Translate(langFile.EpisodeID, id, row.Value, useMT);
                //// 翻訳済みのテキストをXMLに反映する。
                row.Value = text;
            }

            xml.Save(savePath);
            return true;
        }

        /// <summary>
        /// 指定されたフォルダーの言語データを読み込み、言語情報に格納する。
        /// </summary>
        /// <param name="langInfo">言語情報</param>
        /// <param name="folderPath">言語情報を読み込むフォルダーのパス</param>
        public static void LoadFromFolder(SuLanguageInfo langInfo, string folderPath)
        {
            var fullPath = Path.GetFullPath(folderPath);
            var di = new DirectoryInfo(fullPath);
            var files = di.GetFiles("*.xml", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                if (!Path.GetExtension(f.FullName).Equals(
                    ".xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                var episodeID = GetEpisodeID(f.FullName);
                var fileType = GetFileType(f.FullName);
                if (fileType == SuLanguageFile.NFileType.None)
                {
                    Console.WriteLine($"Warning: Unsupported file({fullPath})");
                }
                else
                {
                    LoadFromXml(langInfo, episodeID, fileType, f.FullName);
                }
            }
        }

        /// <summary>
        /// 指定されたXMLファイルを読み込み、言語情報に格納する。
        /// </summary>
        /// <param name="langInfo">言語情報</param>
        /// <param name="episodeID">EpisodeID</param>
        /// <param name="fileType">FileType</param>
        /// <param name="path">XMLファイルのパス</param>
        public static void LoadFromXml(
            SuLanguageInfo langInfo,
            string episodeID,
            SuLanguageFile.NFileType fileType,
            string path)
        {
            var langFile = new SuLanguageFile(episodeID, fileType);
            langInfo.AddFile(langFile);

            var xml = XDocument.Load(path);
            XElement table = null;
            if (langFile.FileKey.Equals("E04:UI", StringComparison.OrdinalIgnoreCase))
            {
                //// E04のUI.xmlのみUIではなく DIALOGUE
                var fileTypeString =
                    langFile.GetFileTypeStrint(SuLanguageFile.NFileType.Dialog);
                table = xml.Element(fileTypeString);
            }
            else
            {
                table = xml.Element(langFile.FileTypeString);
            }

            var rows = table.Elements("txt");
            foreach (var row in rows)
            {
                var attr = row.Attribute("id");
                var id = attr.Value;
                var text = row.Value;
                var entry = new SuLanguageEntry(id, text);
                langFile.AddEntry(entry);
            }
        }

        /// <summary>
        /// ファイルのパスからEpisodeIDを返す。
        /// </summary>
        /// <param name="path">ファイルのパス</param>
        /// <returns>EpisodeID</returns>
        private static string GetEpisodeID(string path)
        {
            var fullPath = Path.GetFullPath(path);
            var folderName = Path.GetDirectoryName(fullPath);
            if (folderName.Contains(@"\E01"))
            {
                return "E01";
            }
            else if (folderName.Contains(@"\E02"))
            {
                return "E02";
            }
            else if (folderName.Contains(@"\E03"))
            {
                return "E03";
            }
            else if (folderName.Contains(@"\E04"))
            {
                return "E04";
            }
            else
            {
                throw new Exception($"folder error({path})");
            }
        }

        /// <summary>
        /// ファイルのパスからファイルタイプを返す。
        /// </summary>
        /// <param name="path">ファイルのパス</param>
        /// <returns>ファイルタイプ</returns>
        private static SuLanguageFile.NFileType GetFileType(string path)
        {
            var fileName = Path.GetFileName(path);
            if (fileName.Equals("DIA.xml", StringComparison.InvariantCultureIgnoreCase))
            {
                return SuLanguageFile.NFileType.Dialog;
            }
            else if (fileName.Equals("UI.xml", StringComparison.InvariantCultureIgnoreCase))
            {
                return SuLanguageFile.NFileType.UI;
            }
            else
            {
                //// 未対応のファイル
                return SuLanguageFile.NFileType.None;
            }
        }
    }
}
