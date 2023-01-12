using System.Data;
using Otchitta.Example.Demo01.Worker;
using Otchitta.Libraries.Common.Rdb;

namespace Otchitta.Example.Demo01;

/// <summary>
/// デモプログラムクラスです。
/// </summary>
internal static class Program {
	/// <summary>
	/// データベース接続名称
	/// </summary>
	//private const string DatabaseConnector = "System.Data.SqlClient";
	private const string DatabaseConnector = "System.Data.SQLite";
	/// <summary>
	/// データベース接続引数
	/// </summary>
	//private const string DatabaseParameter = "Data Source=localhost; Initial Catalog=data_report; User ID=data_worker; Password=xxx123XXX;";
	private const string DatabaseParameter = "Data Source=..\\report.db";

	#region 出力メソッド定義
	/// <summary>
	/// ログ情報を出力します。
	/// </summary>
	/// <param name="source">ログ情報</param>
	private static void Output(string source) =>
		Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss.fff")}]{source}");
	#endregion 出力メソッド定義

	#region 取込メソッド定義
	/// <summary>
	/// 取込処理を実行します。
	/// </summary>
	/// <param name="sourceWorker">基本処理</param>
	/// <param name="invokeSource">実行情報</param>
	private static void Invoke(ReportSourceWorker sourceWorker, ReportInvokeSource<Process> invokeSource) {
		var processData = invokeSource.SourceData;
		using (var invokeWorker = sourceWorker.CreateWorker(invokeSource))
		using (var wrapper = new Wrapper(processData, invokeWorker)) {
			if (invokeWorker.Start()) {
				try {
					processData.Import(wrapper.Update);
					if (invokeWorker.Success()) {
						Output($"情報:成功失敗:{processData.FileName}");
					}
				} catch (Exception error) {
					if (invokeWorker.Failure(error)) {
						Output($"情報:例外失敗:{processData.FileName}");
					}
				}
			} else {
				Output($"情報:処理取消:{processData.FileName}");
			}
		}
	}
	/// <summary>
	/// 取込処理を実行します。
	/// </summary>
	/// <param name="command">実行処理</param>
	private static void Invoke(IDbCommand command) {
		using (var sourceWorker = new ReportSourceWorker(command, "取込処理", null)) {
			try {
				var sourceList = new List<ReportInvokeSource<Process>>();
				// ファイル一覧取得
				foreach (var sourceData in Process.CreateList()) {
					sourceList.Add(sourceWorker.CreateSource(sourceData, sourceData.FileName, null, sourceData.FileSize));
				}
				// 取込処理実行
				foreach (var sourceData in sourceList) {
					Invoke(sourceWorker, sourceData);
				}
				sourceWorker.Success();
			} catch (Exception error) {
				sourceWorker.Failure(error);
			}
		}
	}
	/// <summary>
	/// 取込処理を実行します。
	/// </summary>
	/// <param name="connector">接続名称</param>
	/// <param name="parameter">接続引数</param>
	/// <remarks>トランザクションは行わない事とします。
	/// トランザクションを行うと強制終了した場合に処理が終わった情報も全て消えてしまう事による。</remarks>
	private static void Invoke(string connector, string parameter) {
		if (ReportSourceWorker.Initialize(connector)) {
			using (var connection = ConnectUtilities.Create(connector, parameter))
			using (var command = connection.CreateCommand()) {
				Invoke(command);
			}
		} else {
			Output("情報:SQLのXML定義ファイルが存在しません");
		}
	}
	#endregion 取込メソッド定義

	#region 実行メソッド定義
	/// <summary>
	/// デモプログラムを実行します。
	/// </summary>
	private static void Invoke() {
		System.Data.Common.DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
		System.Data.Common.DbProviderFactories.RegisterFactory("System.Data.SQLite",    System.Data.SQLite.SQLiteFactory.Instance);
		Invoke(DatabaseConnector, DatabaseParameter);
	}
	#endregion 実行メソッド定義

	#region 起動メソッド定義
	/// <summary>
	/// デモプログラムを実行します。
	/// </summary>
	/// <param name="args">コマンドライン引数</param>
	public static void Main(string[] args) {
		Output("開始:デモプログラム01");
		try {
			Invoke();
		} catch (Exception error) {
			Output("失敗:想定外エラーが発生しました");
			Output(error.ToString());
		} finally {
			Output("終了:デモプログラム01");
		}
	}
	#endregion 起動メソッド定義

	#region 非公開クラス定義
	/// <summary>
	/// 出力処理クラスです。
	/// </summary>
	private class Wrapper : IDisposable {
		/// <summary>
		/// 取込処理
		/// </summary>
		private Process? source;
		/// <summary>
		/// 進捗処理
		/// </summary>
		private ReportInvokeWorker? worker;
		/// <summary>
		/// 出力基準
		/// </summary>
		private long border;

		/// <summary>
		/// 取込処理を取得します。
		/// </summary>
		/// <returns>取込処理</returns>
		private Process Source => this.source ?? throw new ObjectDisposedException(GetType().FullName);
		/// <summary>
		/// 進捗処理を取得します。
		/// </summary>
		/// <returns>進捗処理</returns>
		private ReportInvokeWorker Worker => this.worker ?? throw new ObjectDisposedException(GetType().FullName);

		/// <summary>
		/// 出力処理を生成します。
		/// </summary>
		/// <param name="source">取込処理</param>
		/// <param name="worker">進捗処理</param>
		public Wrapper(Process source, ReportInvokeWorker worker) {
			this.source = source;
			this.worker = worker;
			this.border = 0;
		}

		/// <summary>
		/// 保持情報を破棄します。
		/// </summary>
		void IDisposable.Dispose() {
			this.source = default;
			this.worker = default;
			this.border = default;
		}

		/// <summary>
		/// 進捗処理を実行します。
		/// </summary>
		/// <param name="current">現在個数</param>
		/// <param name="summary">全体個数</param>
		/// <returns>進捗処理に失敗した場合</returns>
		public bool Update(long current, long summary) {
			if (this.border <= current) {
				Output($"情報:[{current,10:#,0}/{summary,10:#,0}]{Source.FileName}");
				this.border += 4096 * 16; // 16KBずつ出力
			}
			if (Worker.Progress(current, summary)) {
				// 更新成功
				return true;
			} else {
				// 更新失敗
				Output($"情報:処理中断:{Source.FileName}");
				return false;
			}
		}
	}
	#endregion 非公開クラス定義
}
