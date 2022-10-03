using System.Data;
using Otchitta.Libraries.Common.Rdb;

namespace Otchitta.Example.Demo01.Entity.Report;

/// <summary>
/// 基本情報クラスです。
/// </summary>
public sealed class ReportSourceEntity {
	#region メンバー定数定義
	/// <summary>
	/// 状態種別：実行中
	/// </summary>
	private const byte StatusCodeInvoking = 1;
	/// <summary>
	/// 状態種別：実行済
	/// </summary>
	private const byte StatusCodeFinished = 2;
	/// <summary>
	/// 状態種別：取消済
	/// </summary>
	private const byte StatusCodeCanceled = 3;
	#endregion メンバー定数定義

	#region プロパティー定義
	/// <summary>
	/// 基本番号を取得します。
	/// </summary>
	/// <value>基本番号</value>
	public int SourceCode {
		get;
	}
	/// <summary>
	/// 基本名称を取得します。
	/// </summary>
	/// <value>基本名称</value>
	public string SourceName {
		get;
	}
	/// <summary>
	/// 基本内容を取得します。
	/// </summary>
	/// <value>基本内容</value>
	public string? SourceText {
		get;
	}
	/// <summary>
	/// 状態種別を取得します。
	/// </summary>
	/// <value>状態種別</value>
	public byte StatusCode {
		get;
		private set;
	}
	/// <summary>
	/// 開始日時を取得します。
	/// </summary>
	/// <value>開始日時</value>
	public DateTime InvokeTime {
		get;
	}
	/// <summary>
	/// 終了日時を取得します。
	/// </summary>
	/// <value>終了日時</value>
	public DateTime? FinishTime {
		get;
		private set;
	}
	/// <summary>
	/// 結果内容を取得します。
	/// </summary>
	/// <value>結果内容</value>
	public string? ResultText {
		get;
		private set;
	}
	/// <summary>
	/// 登録名称を取得します。
	/// </summary>
	/// <value>登録名称</value>
	public string InsertName {
		get;
	}
	/// <summary>
	/// 登録日時を取得します。
	/// </summary>
	/// <value>登録日時</value>
	public DateTime InsertTime {
		get;
	}
	/// <summary>
	/// 更新名称を取得します。
	/// </summary>
	/// <value>更新名称</value>
	public string UpdateName {
		get;
		private set;
	}
	/// <summary>
	/// 更新日時を取得します。
	/// </summary>
	/// <value>更新日時</value>
	public DateTime UpdateTime {
		get;
		private set;
	}
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 基本情報を生成します。
	/// </summary>
	/// <param name="sourceCode">基本番号</param>
	/// <param name="sourceName">基本名称</param>
	/// <param name="sourceText">基本内容</param>
	/// <param name="statusCode">状態種別</param>
	/// <param name="invokeTime">開始日時</param>
	/// <param name="finishTime">終了日時</param>
	/// <param name="resultText">結果内容</param>
	/// <param name="insertName">登録名称</param>
	/// <param name="insertTime">登録日時</param>
	/// <param name="updateName">更新名称</param>
	/// <param name="updateTime">更新日時</param>
	private ReportSourceEntity(int sourceCode, string sourceName, string? sourceText, byte statusCode, DateTime invokeTime, DateTime? finishTime, string? resultText, string insertName, DateTime insertTime, string updateName, DateTime updateTime) {
		SourceCode = sourceCode;
		SourceName = sourceName;
		SourceText = sourceText;
		StatusCode = statusCode;
		InvokeTime = invokeTime;
		FinishTime = finishTime;
		ResultText = resultText;
		InsertName = insertName;
		InsertTime = insertTime;
		UpdateName = updateName;
		UpdateTime = updateTime;
	}
	/// <summary>
	/// 基本情報を生成します。
	/// </summary>
	/// <param name="recordData">読込情報</param>
	/// <returns>基本情報</returns>
	private static ReportSourceEntity CreateData(IDataRecord recordData) {
		var sourceCode = recordData.GetInt32(0);
		var sourceName = recordData.GetString(1);
		var sourceText = recordData.GetString(2, null);
		var statusCode = recordData.GetByte(3);
		var invokeTime = recordData.GetDateTime(4);
		var finishTime = recordData.GetDateTime(5, null);
		var resultText = recordData.GetString(6, null);
		var insertName = recordData.GetString(7);
		var insertTime = recordData.GetDateTime(8);
		var updateName = recordData.GetString(9);
		var updateTime = recordData.GetDateTime(10);
		return new ReportSourceEntity(sourceCode, sourceName, sourceText, statusCode, invokeTime, finishTime, resultText, insertName, insertTime, updateName, updateTime);
	}
	#endregion 生成メソッド定義

	#region 抽出メソッド定義
	/// <summary>
	/// 基本一覧を抽出します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <returns>基本一覧</returns>
	public static List<ReportSourceEntity> SelectList(IDbCommand invokeData) {
		invokeData.SetCommandText("Report:Source:SelectList");
		return invokeData.SelectList(CreateData);
	}
	#endregion 抽出メソッド定義

	#region 登録メソッド定義
	/// <summary>
	/// 基本情報を登録します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="sourceName">基本名称</param>
	/// <param name="sourceText">基本内容</param>
	/// <param name="insertName">登録名称</param>
	/// <returns>基本情報</returns>
	public static ReportSourceEntity InsertData(IDbCommand invokeData, string sourceName, string? sourceText, string insertName) {
		static (int, DateTime, DateTime, DateTime) ChooseData(IDataRecord recordData) {
			var sourceCode = recordData.GetInt32(0);
			var invokeTime = recordData.GetDateTime(1);
			var insertTime = recordData.GetDateTime(2);
			var updateTime = recordData.GetDateTime(3);
			return (sourceCode, invokeTime, insertTime, updateTime);
		}
		var statusCode = StatusCodeInvoking;
		var finishTime = (DateTime?)null;
		var resultText = (string?)null;
		invokeData.SetCommandText("Report:Source:InsertData");
		invokeData.AddParamString("SourceName", sourceName);
		invokeData.AddParamString("SourceText", sourceText);
		invokeData.AddParamByte("StatusCode", statusCode);
		invokeData.AddParamDateTime("FinishTime", finishTime);
		invokeData.AddParamString("ResultText", resultText);
		invokeData.AddParamString("InsertName", insertName);
		var (sourceCode, invokeTime, insertTime, updateTime) = invokeData.SelectData(ChooseData);
		return new ReportSourceEntity(sourceCode, sourceName, sourceText, statusCode, invokeTime, finishTime, resultText, insertName, insertTime, insertName, insertTime);
	}
	#endregion 登録メソッド定義

	#region 更新メソッド定義
	/// <summary>
	/// 状態種別を更新します。
	/// <param>状態種別が「実行中」となっている情報を全て「中止済」に変更します。</param>
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="sourceName">基本名称</param>
	/// <param name="updateName">更新名称</param>
	/// <returns>更新件数</returns>
	public static int UpdateList(IDbCommand invokeData, string sourceName, string updateName) {
		invokeData.SetCommandText("Report:Source:UpdateList");
		invokeData.AddParamString("SourceName", sourceName);
		invokeData.AddParamByte("StatusCode", StatusCodeCanceled);
		invokeData.AddParamByte("BeforeCode", StatusCodeInvoking);
		invokeData.AddParamString("UpdateName", updateName);
		return invokeData.InvokeData();
	}
	/// <summary>
	/// 終了情報を更新します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="resultText">結果内容</param>
	/// <param name="updateName">更新名称</param>
	/// <returns>当該情報を更新した場合、<c>True</c>を返却</returns>
	public bool FinishData(IDbCommand invokeData, string? resultText, string updateName) {
		static (DateTime, DateTime) ChooseData(IDataRecord recordData) {
			var finishTime = recordData.GetDateTime(0);
			var updateTime = recordData.GetDateTime(1);
			return (finishTime, updateTime);
		}
		const byte statusCode = StatusCodeFinished;
		invokeData.SetCommandText("Report:Source:FinishData");
		invokeData.AddParamInt32("SourceCode", SourceCode);
		invokeData.AddParamByte("StatusCode", statusCode);
		invokeData.AddParamString("ResultText", resultText);
		invokeData.AddParamString("UpdateName", updateName);
		invokeData.AddParamDateTime("UpdateTime", UpdateTime);
		if (invokeData.SelectData(ChooseData, out var chooseData)) {
			StatusCode = statusCode;
			FinishTime = chooseData.Item1;
			ResultText = resultText;
			UpdateName = updateName;
			UpdateTime = chooseData.Item2;
			return true;
		} else {
			return false;
		}
	}
	#endregion 更新メソッド定義
}
