namespace LibSuJpMod.TransSheet
{
    using System.IO;
    using System.Text;
    using CsvHelper;
    using LibSuJpMod.Language;

    /// <summary>
    /// 翻訳シートの入出力
    /// </summary>
    public class SuTransSheetDao
    {
        /// <summary>
        /// CSV形式の翻訳シートを読み込み、翻訳シート情報を返す。
        /// </summary>
        /// <param name="path">CSV形式の翻訳シートのパス</param>
        /// <param name="enc">文字コード</param>
        /// <returns>翻訳シート情報</returns>
        public static SuTransSheetInfo LoadFromCsv(string path, Encoding enc = null)
        {
            if (enc == null)
            {
                enc = Encoding.UTF8;
            }

            using (var sr = new StreamReader(path, enc))
            {
                using (var csv = new CsvReader(sr))
                {
                    var transSheetInfo = new SuTransSheetInfo();

                    csv.Configuration.Delimiter = ",";
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.RegisterClassMap<CsvMapper>();

                    var records = csv.GetRecords<SuTransSheetEntry>();

                    foreach (var sheetEntry in records)
                    {
                        transSheetInfo.AddEntry(sheetEntry.EpisodeID, sheetEntry);
                    }

                    return transSheetInfo;
                }
            }
        }

        /// <summary>
        /// 言語情報から翻訳シート(CSV形式)を保存する。
        /// </summary>
        /// <param name="langInfo">言語情報</param>
        /// <param name="path">CSV形式の翻訳シートのパス</param>
        public static void SaveToCsv(SuLanguageInfo langInfo, string path)
        {
            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                using (var csv = new CsvWriter(sw))
                {
                    csv.Configuration.RegisterClassMap<CsvMapper>();
                    csv.WriteHeader<SuTransSheetEntry>();
                    csv.NextRecord();

                    foreach (var langFile in langInfo.Items.Values)
                    {
                        var sequence = 0;
                        foreach (var langEntry in langFile.Items.Values)
                        {
                            var sheetEntry = new SuTransSheetEntry();

                            sheetEntry.EpisodeID = langFile.EpisodeID;
                            sheetEntry.FyleType = langFile.FileType.ToString();
                            sheetEntry.ID = langEntry.ID;
                            sheetEntry.Type = langEntry.Type;
                            sheetEntry.English = langEntry.Text;
                            sheetEntry.Japanese = string.Empty;
                            sheetEntry.Sequence = sequence;

                            csv.WriteRecord(sheetEntry);
                            csv.NextRecord();
                            sequence++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 格納ルール ：マッピングルール(一行目を列名とした場合は列名で定義することができる。)
        /// </summary>
        public class CsvMapper : CsvHelper.Configuration.ClassMap<SuTransSheetEntry>
        {
            public CsvMapper()
            {
                // 出力時の列の順番は指定した順となる。
                this.Map(x => x.EpisodeID).Name("[[EpisodeID]]");
                this.Map(x => x.FyleType).Name("[[FyleType]]");
                this.Map(x => x.ID).Name("[[ID]]");
                this.Map(x => x.Type).Name("[[Type]]");
                this.Map(x => x.English).Name("[[English]]");
                this.Map(x => x.Japanese).Name("[[Japanese]]");
                this.Map(x => x.MT).Name("[[MT]]");
                this.Map(x => x.Sequence).Name("[[Sequence]]");
            }
        }
    }
}
