namespace Otchitta.Example.Demo01;

/// <summary>
/// 取込処理クラスです。
/// </summary>
internal sealed class Process {
	#region プロパティー定義
	/// <summary>
	/// ファイル名を取得します。
	/// </summary>
	/// <value>ファイル名</value>
	public string FileName {
		get;
	}
	/// <summary>
	/// ファイル長を取得します。
	/// </summary>
	/// <value>ファイル長</value>
	public long FileSize {
		get;
	}
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 取込処理を生成します。
	/// </summary>
	/// <param name="fileName">ファイル名</param>
	/// <param name="fileSize">ファイル長</param>
	private Process(string fileName, long fileSize) {
		FileName = fileName;
		FileSize = fileSize;
	}
	/// <summary>
	/// 取込一覧を生成します。
	/// </summary>
	/// <returns>取込一覧</returns>
	public static List<Process> CreateList() {
		var result = new List<Process>();
		var random = new Random();
		var length = random.Next(5, 10);
		for (var index = 0; index < length; index ++) {
			// 4MB～10MBの仮想ファイルをランダム生成
			result.Add(new Process($"{DateTime.Now:yyyyMMdd}_{index + 1:000}.csv", random.Next(04_000_000, 10_000_000)));
		}
		return result;
	}
	#endregion 生成メソッド定義

	#region 公開メソッド定義
	/// <summary>
	/// 取込処理を実行します。
	/// </summary>
	/// <param name="action">進捗処理</param>
	public void Import(Func<long, long, bool> action) {
		for (var index = 0; index < FileSize; index += 4096) {
			var choose = DateTime.Now;
			if (choose.Second % 10 == 0 && choose.Millisecond == 0) {
				throw new Exception("読取エラー");
			} else if (action(index, FileSize) == false) {
				// 読込中断
				return;
			} else {
				Thread.Sleep(10);
			}
		}
		action(FileSize, FileSize);
	}
	#endregion 公開メソッド定義
}
