using System.Data;
using Otchitta.Example.Demo01.Entity.Report;
using Otchitta.Libraries.Common;

namespace Otchitta.Example.Demo01.Worker;

/// <summary>
/// 実行処理クラスです。
/// </summary>
public sealed class ReportInvokeWorker : IDisposable {
	#region メンバー変数定義
	/// <summary>
	/// 実行処理
	/// </summary>
	private IDbCommand? invokeData;
	/// <summary>
	/// 実行情報
	/// </summary>
	private ReportInvokeEntity? entityData;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 実行処理を取得します。
	/// </summary>
	/// <value>実行処理</value>
	private IDbCommand InvokeData => this.invokeData ?? throw new ObjectDisposedException(GetType().FullName);
	/// <summary>
	/// 実行情報を取得します。
	/// </summary>
	/// <value>実行情報</value>
	private ReportInvokeEntity EntityData => this.entityData ?? throw new ObjectDisposedException(GetType().FullName);
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 実行処理を生成します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="entityData">実行情報</param>
	public ReportInvokeWorker(IDbCommand invokeData, ReportInvokeEntity entityData) {
		this.invokeData = invokeData;
		this.entityData = entityData;
	}
	#endregion 生成メソッド定義

	#region 破棄メソッド定義
	/// <summary>
	/// 保持情報を破棄します。
	/// </summary>
	void IDisposable.Dispose() {
		this.invokeData = default;
		this.entityData = default;
	}
	#endregion 破棄メソッド定義

	#region 公開メソッド定義
	/// <summary>
	/// 開始処理を実行します。
	/// </summary>
	/// <returns>開始処理に失敗した場合</returns>
	public bool Start() =>
		EntityData.InvokeData(InvokeData, ExeFileUtilities.ExecuteName);
	/// <summary>
	/// 進捗処理を実行します。
	/// </summary>
	/// <param name="currentSize">現在個数</param>
	/// <param name="summarySize">全体個数</param>
	/// <returns>進捗処理に失敗した場合</returns>
	public bool Progress(long currentSize, long summarySize) =>
		EntityData.UpdateData(InvokeData, currentSize, summarySize, ExeFileUtilities.ExecuteName);
	/// <summary>
	/// 成功処理を実行します。
	/// </summary>
	/// <returns>成功処理に失敗した場合</returns>
	public bool Success() =>
		EntityData.FinishData(InvokeData, true, null, ExeFileUtilities.ExecuteName);
	/// <summary>
	/// 失敗処理を実行します。
	/// </summary>
	/// <param name="resultText">失敗内容</param>
	/// <returns>失敗処理に失敗した場合</returns>
	public bool Failure(string resultText) =>
		EntityData.FinishData(InvokeData, false, resultText, ExeFileUtilities.ExecuteName);
	/// <summary>
	/// 失敗処理を実行します。
	/// </summary>
	/// <param name="exception">例外情報</param>
	/// <returns>開始処理に失敗した場合</returns>
	public bool Failure(Exception exception) =>
		Failure(exception.ToString());
	#endregion 公開メソッド定義
}
