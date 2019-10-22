namespace SuSheetMaker
{
    using LibSuJpMod.Language;
    using LibSuJpMod.TransSheet;
    using MonoOptions;
    using S5mDebugTools;

    internal class Program
    {
        private static int Main(string[] args)
        {
            var opt = new TOptions(args);
            if (opt.IsError)
            {
                TDebugUtils.Pause();
                return 1;
            }

            if (opt.Arges.Help)
            {
                opt.ShowUsage();

                TDebugUtils.Pause();
                return 1;
            }

            //// 空の言語情報を作成
            var langInfo = new SuLanguageInfo();
            //// 言語ファイルを格納されているフォルダーから読み込み、言語情報に格納する。
            SuLanguageDao.LoadFromFolder(langInfo, opt.Arges.FolderNameLangInput);
            //// 言語情報を使い翻訳シートをCSV形式で保存する。
            SuTransSheetDao.SaveToCsv(langInfo, opt.Arges.FileNameSheet);

            TDebugUtils.Pause();
            return 0;
        }
    }
}
