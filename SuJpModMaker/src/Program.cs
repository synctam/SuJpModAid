namespace SuJpModMaker
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

            //// 翻訳シートを読み込む。
            var transSheet = SuTransSheetDao.LoadFromCsv(opt.Arges.FileNameSheet);
            //// 翻訳シートと原文の言語情報から翻訳された言語ファイルを保存する。
            SuLanguageDao.SaveToFolder(
                transSheet,
                opt.Arges.FolderNameInput,
                opt.Arges.FileNameOutput,
                opt.Arges.UseMachineTrans,
                opt.Arges.UseReplace);

            TDebugUtils.Pause();
            return 0;
        }
    }
}
