namespace LibSuJpMod.Language
{
    /// <summary>
    /// 言語エントリー
    /// </summary>
    public class SuLanguageEntry
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="text">テキスト</param>
        public SuLanguageEntry(string id, string text)
        {
            this.ID = id;
            this.Text = text;
        }

        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; } = string.Empty;

        /// <summary>
        /// テキスト
        /// </summary>
        public string Text { get; } = string.Empty;

        /// <summary>
        /// タイプ
        /// </summary>
        public string Type
        {
            get
            {
                if (this.ID.StartsWith("USER_"))
                {
                    return "Command";
                }
                else
                {
                    return "Text";
                }
            }
        }
    }
}
