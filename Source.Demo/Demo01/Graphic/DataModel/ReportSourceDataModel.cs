using System.Collections.Generic;
using System.Data;
using Otchitta.Example.Demo01.Entity.Report;

namespace Otchitta.Example.Demo01.DataModel;

/// <summary>
/// 基本情報モデルを生成します。
/// </summary>
public sealed class ReportSourceDataModel {
	#region プロパティー定義
	/// <summary>
	/// 基本情報を取得します。
	/// </summary>
	/// <value>基本情報</value>
	public ReportSourceEntity SourceData {
		get;
	}
	/// <summary>
	/// 実行一覧を取得します。
	/// </summary>
	/// <value>実行一覧</value>
	public ReportInvokeListModel InvokeList {
		get;
	}
	/// <summary>
	/// 基本番号を取得します。
	/// </summary>
	/// <value>基本番号</value>
	public int SourceCode => SourceData.SourceCode;
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 基本情報モデルを生成します。
	/// </summary>
	/// <param name="sourceData">基本情報</param>
	/// <param name="invokeList">実行一覧</param>
	private ReportSourceDataModel(ReportSourceEntity sourceData, ReportInvokeListModel invokeList) {
		SourceData = sourceData;
		InvokeList = invokeList;
	}
	/// <summary>
	/// 基本情報モデルを生成します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="sourceData">基本情報</param>
	/// <returns>基本情報モデル</returns>
	public static ReportSourceDataModel Create(IDbCommand invokeData, ReportSourceEntity sourceData) {
		var invokeList = ReportInvokeListModel.Create(invokeData, sourceData.SourceCode);
		return new ReportSourceDataModel(sourceData, invokeList);
	}
	#endregion 生成メソッド定義

	#region 分解メソッド定義
	/// <summary>
	/// 保持情報を出力します。
	/// </summary>
	/// <param name="sourceData">基本情報</param>
	/// <param name="invokeList">実行一覧</param>
	public void Deconstruct(out ReportSourceEntity sourceData, out ReportInvokeListModel invokeList) {
		sourceData = SourceData;
		invokeList = InvokeList;
	}
	#endregion 分解メソッド定義
}
