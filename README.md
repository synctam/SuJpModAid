# SuJpModAid
Stories Untold 日本語化支援ツール

## SuSheetMaker

	言語フォルダー内のXMLファイルから翻訳シートを作成する。
	  usage: SuSheetMaker.exe -i <lang folder path> -s <trans sheet path> [-r]
	OPTIONS:
	  -i, --in=VALUE             オリジナル版の言語フォルダーのパスを指定する。
	  -s, --sheet=VALUE          CSV形式の翻訳シートのパス名。
	  -r                         翻訳シートが既に存在する場合はを上書きする。
	  -h, --help                 ヘルプ
	Example:
	  言語フォルダー(-i)から翻訳シート(-s)を作成する。
		SuSheetMaker.exe -i Localisation\en -s data\csv\SuTransSheet.csv
	終了コード:
	 0  正常終了
	 1  異常終了
 
 
## SuJpModMaker
 
	日本語化MODを作成する。
	  usage: SuJpModMaker.exe -i <original lang folder path> -o <japanized lang folder path> -s <Trans Sheet path> [-m] [-r]
	OPTIONS:
	  -i, --in=VALUE             オリジナル版の言語フォルダーのパスを指定する。
	  -o, --out=VALUE            日本語化された言語フォルダーのパスを指定する。
	  -s, --sheet=VALUE          CSV形式の翻訳シートのパス名。
	  -m                         有志翻訳がない場合は機械翻訳を使用する。
	  -r                         出力用言語ファイルが既に存在する場合はを上書きする。
	  -h, --help                 ヘルプ
	Example:
	  翻訳シート(-s)とオリジナルの言語フォルダー(-i)から日本語の言語フォルダーに日本語化MOD(-o)を作成する。
		SuJpModMaker.exe -i Localisation\EN -o Localisation\JP -s SuTransSheet.csv
	終了コード:
	 0  正常終了
	 1  異常終了
