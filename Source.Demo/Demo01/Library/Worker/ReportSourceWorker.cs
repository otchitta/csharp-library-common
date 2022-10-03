using System.Data;
using Otchitta.Example.Demo01.Entity;
using Otchitta.Example.Demo01.Entity.Report;
using Otchitta.Libraries.Common;

namespace Otchitta.Example.Demo01.Worker;

/// <summary>
/// 基本処理クラスです。
/// </summary>
public sealed class ReportSourceWorker : IDisposable {
	#region メンバー変数定義
	/// <summary>
	/// 実行処理
	/// </summary>
	private IDbCommand? invokeData;
	/// <summary>
	/// 基本情報
	/// </summary>
	private ReportSourceEntity? entityData;
	/// <summary>
	/// 実行番号
	/// </summary>
	private int invokeCode;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 実行処理を取得します。
	/// </summary>
	/// <value>実行処理</value>
	private IDbCommand InvokeData => this.invokeData ?? throw new ObjectDisposedException(GetType().FullName);
	/// <summary>
	/// 基本情報を取得します。
	/// </summary>
	/// <value>基本情報</value>
	private ReportSourceEntity EntityData => this.entityData ?? throw new ObjectDisposedException(GetType().FullName);
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 基本処理を生成します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="sourceName">基本名称</param>
	/// <param name="sourceText">基本内容</param>
	public ReportSourceWorker(IDbCommand invokeData, string sourceName, string? sourceText) {
		ReportSourceEntity.UpdateList(invokeData, sourceName, ExeFileUtilities.ExecuteName);
		ReportInvokeEntity.UpdateList(invokeData, sourceName, ExeFileUtilities.ExecuteName);
		this.invokeData = invokeData;
		this.entityData = ReportSourceEntity.InsertData(invokeData, sourceName, sourceText, ExeFileUtilities.ExecuteName);
		this.invokeCode = 1;
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

	#region 公開メソッド定義(CreateSource/CreateWorker)
	/// <summary>
	/// 実行情報を生成します。
	/// </summary>
	/// <param name="sourceData">要素情報</param>
	/// <param name="invokeName">実行名称</param>
	/// <param name="invokeText">実行内容</param>
	/// <typeparam name="TValue">要素種別</typeparam>
	/// <returns>実行情報</returns>
	public ReportInvokeSource<TValue> CreateSource<TValue>(TValue sourceData, string invokeName, string? invokeText) {
		var sourceCode = EntityData.SourceCode;
		var invokeCode = this.invokeCode ++;
		var entityData = ReportInvokeEntity.InsertData(InvokeData, sourceCode, invokeCode, invokeName, invokeText, ExeFileUtilities.ExecuteName);
		return new ReportInvokeSource<TValue>(sourceData, entityData);
	}
	/// <summary>
	/// 実行情報を生成します。
	/// </summary>
	/// <param name="sourceData">要素情報</param>
	/// <param name="invokeName">実行名称</param>
	/// <param name="invokeText">実行内容</param>
	/// <param name="finishSize">実行総数</param>
	/// <typeparam name="TValue">要素種別</typeparam>
	/// <returns>実行情報</returns>
	public ReportInvokeSource<TValue> CreateSource<TValue>(TValue sourceData, string invokeName, string? invokeText, long finishSize) {
		var sourceCode = EntityData.SourceCode;
		var invokeCode = this.invokeCode ++;
		var entityData = ReportInvokeEntity.InsertData(InvokeData, sourceCode, invokeCode, invokeName, invokeText, finishSize, ExeFileUtilities.ExecuteName);
		return new ReportInvokeSource<TValue>(sourceData, entityData);
	}
	/// <summary>
	/// 実行処理を生成します。
	/// </summary>
	/// <param name="sourceData">実行情報</param>
	/// <typeparam name="TValue">要素種別</typeparam>
	/// <returns>実行処理</returns>
	public ReportInvokeWorker CreateWorker<TValue>(ReportInvokeSource<TValue> sourceData) =>
		new ReportInvokeWorker(InvokeData, sourceData.EntityData);
	#endregion 公開メソッド定義(CreateSource/CreateWorker)

	#region 公開メソッド定義(Success/Failure)
	/// <summary>
	/// 成功処理を実行します。
	/// </summary>
	/// <returns>成功処理に失敗した場合</returns>
	public bool Success() =>
		EntityData.FinishData(InvokeData, null, ExeFileUtilities.ExecuteName);
	/// <summary>
	/// 失敗処理を実行します。
	/// </summary>
	/// <param name="resultText">失敗内容</param>
	/// <returns>開始処理に失敗した場合</returns>
	public bool Failure(string resultText) =>
		EntityData.FinishData(InvokeData, resultText, ExeFileUtilities.ExecuteName);
	/// <summary>
	/// 失敗処理を実行します。
	/// </summary>
	/// <param name="exception">例外情報</param>
	/// <returns>開始処理に失敗した場合</returns>
	public bool Failure(Exception exception) =>
		Failure(exception.ToString());
	#endregion 公開メソッド定義(Success/Failure)

	#region 公開メソッド定義(Initialize)
	/// <summary>
	/// 設定情報を初期化します。
	/// </summary>
	/// <param name="connector">接続名称</param>
	/// <returns>設定を行った場合、<c>True</c>を返却</returns>
	public static bool Initialize(string connector) =>
		CommandExtension.SetCommandPath(ExeFileUtilities.ExecutePath, connector);
	#endregion 公開メソッド定義(Initialize)
}
