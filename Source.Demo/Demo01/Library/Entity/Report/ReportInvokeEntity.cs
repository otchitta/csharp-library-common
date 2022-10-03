using System.Data;
using Otchitta.Libraries.Common.Rdb;

namespace Otchitta.Example.Demo01.Entity.Report;

/// <summary>
/// 実行情報クラスです。
/// </summary>
public sealed class ReportInvokeEntity {
	#region メンバー定数定義
	/// <summary>
	/// 状態種別：待機
	/// </summary>
	private const byte StatusCodeNothing = 0;
	/// <summary>
	/// 状態種別：実行
	/// </summary>
	private const byte StatusCodeProcess = 1;
	/// <summary>
	/// 状態種別：成功
	/// </summary>
	private const byte StatusCodeSuccess = 2;
	/// <summary>
	/// 状態種別：失敗
	/// </summary>
	private const byte StatusCodeFailure = 3;
	/// <summary>
	/// 状態種別：取消
	/// </summary>
	private const byte StatusCodeSuspend = 4;
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
	/// 実行番号を取得します。
	/// </summary>
	/// <value>実行番号</value>
	public int InvokeCode {
		get;
	}
	/// <summary>
	/// 実行名称を取得します。
	/// </summary>
	/// <value>実行名称</value>
	public string InvokeName {
		get;
	}
	/// <summary>
	/// 実行内容を取得します。
	/// </summary>
	/// <value>実行内容</value>
	public string? InvokeText {
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
	/// 実行件数を取得します。
	/// </summary>
	/// <value>実行件数</value>
	public long? InvokeSize {
		get;
		private set;
	}
	/// <summary>
	/// 実行総数を取得します。
	/// </summary>
	/// <value>実行総数</value>
	public long? FinishSize {
		get;
		private set;
	}
	/// <summary>
	/// 開始日時を取得します。
	/// </summary>
	/// <value>開始日時</value>
	public DateTime? InvokeTime {
		get;
		private set;
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
	/// 実行情報を生成します。
	/// </summary>
	/// <param name="sourceCode">基本番号</param>
	/// <param name="invokeCode">実行番号</param>
	/// <param name="invokeName">実行名称</param>
	/// <param name="invokeText">実行内容</param>
	/// <param name="statusCode">状態種別</param>
	/// <param name="invokeSize">実行件数</param>
	/// <param name="finishSize">実行総数</param>
	/// <param name="invokeTime">開始日時</param>
	/// <param name="finishTime">終了日時</param>
	/// <param name="resultText">結果内容</param>
	/// <param name="insertName">登録名称</param>
	/// <param name="insertTime">登録日時</param>
	/// <param name="updateName">更新名称</param>
	/// <param name="updateTime">更新日時</param>
	private ReportInvokeEntity(int sourceCode, int invokeCode, string invokeName, string? invokeText, byte statusCode, long? invokeSize, long? finishSize, DateTime? invokeTime, DateTime? finishTime, string? resultText, string insertName, DateTime insertTime, string updateName, DateTime updateTime) {
		SourceCode = sourceCode;
		InvokeCode = invokeCode;
		InvokeName = invokeName;
		InvokeText = invokeText;
		StatusCode = statusCode;
		InvokeSize = invokeSize;
		FinishSize = finishSize;
		InvokeTime = invokeTime;
		FinishTime = finishTime;
		ResultText = resultText;
		InsertName = insertName;
		InsertTime = insertTime;
		UpdateName = updateName;
		UpdateTime = updateTime;
	}
	/// <summary>
	/// 実行情報を生成します。
	/// </summary>
	/// <param name="recordData">読込情報</param>
	/// <returns>実行情報</returns>
	private static ReportInvokeEntity CreateData(IDataRecord recordData) {
		var sourceCode = recordData.GetInt32(0);
		var invokeCode = recordData.GetInt32(1);
		var invokeName = recordData.GetString(2);
		var invokeText = recordData.GetString(3, null);
		var statusCode = recordData.GetByte(4);
		var invokeSize = recordData.GetInt64(5, null);
		var finishSize = recordData.GetInt64(6, null);
		var invokeTime = recordData.GetDateTime(7, null);
		var finishTime = recordData.GetDateTime(8, null);
		var resultText = recordData.GetString(9, null);
		var insertName = recordData.GetString(10);
		var insertTime = recordData.GetDateTime(11);
		var updateName = recordData.GetString(12);
		var updateTime = recordData.GetDateTime(13);
		return new ReportInvokeEntity(sourceCode, invokeCode, invokeName, invokeText, statusCode, invokeSize, finishSize, invokeTime, finishTime, resultText, insertName, insertTime, updateName, updateTime);
	}
	#endregion 生成メソッド定義

	#region 抽出メソッド定義
	/// <summary>
	/// 実行一覧を抽出します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="sourceCode">基本番号</param>
	/// <returns>実行一覧</returns>
	public static List<ReportInvokeEntity> SelectList(IDbCommand invokeData, int sourceCode) {
		invokeData.SetCommandText("Report:Invoke:SelectList");
		invokeData.AddParamInt32("SourceCode", sourceCode);
		return invokeData.SelectList(CreateData);
	}
	#endregion 抽出メソッド定義

	#region 登録メソッド定義
	/// <summary>
	/// 実行情報を登録します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="sourceCode">基本番号</param>
	/// <param name="invokeCode">実行番号</param>
	/// <param name="invokeName">実行名称</param>
	/// <param name="invokeText">実行内容</param>
	/// <param name="statusCode">状態種別</param>
	/// <param name="invokeSize">実行件数</param>
	/// <param name="finishSize">実行総数</param>
	/// <param name="invokeTime">開始日時</param>
	/// <param name="finishTime">終了日時</param>
	/// <param name="resultText">結果内容</param>
	/// <param name="insertName">登録名称</param>
	/// <returns>実行情報</returns>
	private static ReportInvokeEntity InsertData(IDbCommand invokeData, int sourceCode, int invokeCode, string invokeName, string? invokeText, byte statusCode, long? invokeSize, long? finishSize, DateTime? invokeTime, DateTime? finishTime, string? resultText, string insertName) {
		static (DateTime, DateTime) ChooseData(IDataRecord recordData) {
			var insertTime = recordData.GetDateTime(0);
			var updateTime = recordData.GetDateTime(1);
			return (insertTime, updateTime);
		}
		invokeData.SetCommandText("Report:Invoke:InsertData");
		invokeData.AddParamInt32("SourceCode", sourceCode);
		invokeData.AddParamInt32("InvokeCode", invokeCode);
		invokeData.AddParamString("InvokeName", invokeName);
		invokeData.AddParamString("InvokeText", invokeText);
		invokeData.AddParamByte("StatusCode", statusCode);
		invokeData.AddParamInt64("InvokeSize", invokeSize);
		invokeData.AddParamInt64("FinishSize", finishSize);
		invokeData.AddParamDateTime("InvokeTime", invokeTime);
		invokeData.AddParamDateTime("FinishTime", finishTime);
		invokeData.AddParamString("ResultText", resultText);
		invokeData.AddParamString("InsertName", insertName);
		var (insertTime, updateTime) = invokeData.SelectData(ChooseData);
		return new ReportInvokeEntity(sourceCode, invokeCode, invokeName, invokeText, statusCode, invokeSize, finishSize, invokeTime, finishTime, resultText, insertName, insertTime, insertName, updateTime);
	}
	/// <summary>
	/// 実行情報を登録します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="sourceCode">基本番号</param>
	/// <param name="invokeCode">実行番号</param>
	/// <param name="invokeName">実行名称</param>
	/// <param name="invokeText">実行内容</param>
	/// <param name="insertName">登録名称</param>
	/// <returns>実行情報</returns>
	public static ReportInvokeEntity InsertData(IDbCommand invokeData, int sourceCode, int invokeCode, string invokeName, string? invokeText, string insertName) =>
		InsertData(invokeData, sourceCode, invokeCode, invokeName, invokeText, StatusCodeNothing, null, null, null, null, null, insertName);
	/// <summary>
	/// 実行情報を登録します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="sourceCode">基本番号</param>
	/// <param name="invokeCode">実行番号</param>
	/// <param name="invokeName">実行名称</param>
	/// <param name="invokeText">実行内容</param>
	/// <param name="finishSize">実行総数</param>
	/// <param name="insertName">登録名称</param>
	/// <returns>実行情報</returns>
	public static ReportInvokeEntity InsertData(IDbCommand invokeData, int sourceCode, int invokeCode, string invokeName, string? invokeText, long finishSize, string insertName) =>
		InsertData(invokeData, sourceCode, invokeCode, invokeName, invokeText, StatusCodeNothing, null, finishSize, null, null, null, insertName);
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
		invokeData.SetCommandText("Report:Invoke:UpdateList");
		invokeData.AddParamString("SourceName", sourceName);
		invokeData.AddParamByte("StatusCode", StatusCodeSuspend);
		invokeData.AddParamByte("Parameter1", StatusCodeNothing);
		invokeData.AddParamByte("Parameter2", StatusCodeProcess);
		invokeData.AddParamString("UpdateName", updateName);
		return invokeData.InvokeData();
	}
	/// <summary>
	/// 実行情報を更新します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="updateName">更新名称</param>
	/// <returns></returns>
	public bool InvokeData(IDbCommand invokeData, string updateName) {
		static (DateTime, DateTime) ChooseData(IDataRecord recordData) {
			var invokeTime = recordData.GetDateTime(0);
			var updateTime = recordData.GetDateTime(1);
			return (invokeTime, updateTime);
		}
		const byte statusCode = StatusCodeProcess;
		invokeData.SetCommandText("Report:Invoke:InvokeData");
		invokeData.AddParamInt32("SourceCode", SourceCode);
		invokeData.AddParamInt32("InvokeCode", InvokeCode);
		invokeData.AddParamByte("StatusCode", statusCode);
		invokeData.AddParamString("UpdateName", updateName);
		invokeData.AddParamDateTime("UpdateTime", UpdateTime);
		if (invokeData.SelectData(ChooseData, out var chooseData)) {
			StatusCode = statusCode;
			InvokeTime = chooseData.Item1;
			UpdateName = updateName;
			UpdateTime = chooseData.Item2;
			return true;
		} else {
			Console.WriteLine("{0,6}:{1,6}:{2:yyyy-MM-dd HH:mm:ss.fffffff}", SourceCode, InvokeCode, UpdateTime);
			return false;
		}
	}
	/// <summary>
	/// 実行情報を更新します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="invokeSize">実行件数</param>
	/// <param name="finishSize">実行総数</param>
	/// <param name="updateName">更新名称</param>
	/// <returns>当該情報を更新した場合、<c>True</c>を返却</returns>
	public bool UpdateData(IDbCommand invokeData, long invokeSize, long finishSize, string updateName) {
		static DateTime ChooseData(IDataRecord recordData) => recordData.GetDateTime(0);
		invokeData.SetCommandText("Report:Invoke:UpdateData");
		invokeData.AddParamInt32("SourceCode", SourceCode);
		invokeData.AddParamInt32("InvokeCode", InvokeCode);
		invokeData.AddParamInt64("InvokeSize", invokeSize);
		invokeData.AddParamInt64("FinishSize", finishSize);
		invokeData.AddParamString("UpdateName", updateName);
		invokeData.AddParamDateTime("UpdateTime", UpdateTime);
		if (invokeData.SelectData(ChooseData, out var updateTime)) {
			InvokeSize = invokeSize;
			FinishSize = finishSize;
			UpdateName = updateName;
			UpdateTime = updateTime;
			return true;
		} else {
			return false;
		}
	}
	/// <summary>
	/// 実行情報を更新します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="resultFlag">結果種別</param>
	/// <param name="resultText">結果内容</param>
	/// <param name="updateName">更新名称</param>
	/// <returns>当該情報を更新した場合、<c>True</c>を返却</returns>
	public bool FinishData(IDbCommand invokeData, bool resultFlag, string? resultText, string updateName) {
		static (DateTime, DateTime) ChooseData(IDataRecord recordData) {
			var finishTime = recordData.GetDateTime(0);
			var updateTime = recordData.GetDateTime(1);
			return (finishTime, updateTime);
		}
		var statusCode = resultFlag? StatusCodeSuccess: StatusCodeFailure;
		invokeData.SetCommandText("Report:Invoke:FinishData");
		invokeData.AddParamInt32("SourceCode", SourceCode);
		invokeData.AddParamInt32("InvokeCode", InvokeCode);
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
	/// <summary>
	/// 実行情報を更新します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="updateName">更新名称</param>
	/// <returns></returns>
	public bool CancelData(IDbCommand invokeData, string updateName) {
		static DateTime ChooseData(IDataRecord recordData) => recordData.GetDateTime(0);
		const byte statusCode = StatusCodeSuspend;
		invokeData.SetCommandText("Report:Invoke:FinishData");
		invokeData.AddParamInt32("SourceCode", SourceCode);
		invokeData.AddParamInt32("InvokeCode", InvokeCode);
		invokeData.AddParamByte("StatusCode", statusCode);
		invokeData.AddParamString("UpdateName", updateName);
		invokeData.AddParamDateTime("UpdateTime", UpdateTime);
		if (invokeData.SelectData(ChooseData, out var chooseData)) {
			StatusCode = statusCode;
			UpdateName = updateName;
			UpdateTime = chooseData;
			return true;
		} else {
			return false;
		}
	}
	#endregion 更新メソッド定義
}
