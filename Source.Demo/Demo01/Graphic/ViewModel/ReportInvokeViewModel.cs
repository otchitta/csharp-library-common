using System;
using Otchitta.Example.Demo01.Constant;
using Otchitta.Example.Demo01.Entity.Report;

namespace Otchitta.Example.Demo01.ViewModel;

/// <summary>
/// 実行画面情報クラスです。
/// </summary>
public sealed class ReportInvokeViewModel : AbstractViewModel {
	#region メンバー変数定義
	/// <summary>
	/// 実行番号
	/// </summary>
	private readonly int invokeCode;
	/// <summary>
	/// 実行名称
	/// </summary>
	private string invokeName;
	/// <summary>
	/// 実行内容
	/// </summary>
	private string? invokeText;
	/// <summary>
	/// 状態種別
	/// </summary>
	private StatusType statusCode;
	/// <summary>
	/// 実行件数
	/// </summary>
	private long? invokeSize;
	/// <summary>
	/// 実行総数
	/// </summary>
	private long? finishSize;
	/// <summary>
	/// 開始日時
	/// </summary>
	private DateTime? invokeTime;
	/// <summary>
	/// 終了日時
	/// </summary>
	private DateTime? finishTime;
	/// <summary>
	/// 結果内容
	/// </summary>
	private string? resultText;
	/// <summary>
	/// 登録名称
	/// </summary>
	private string insertName;
	/// <summary>
	/// 登録日時
	/// </summary>
	private DateTime insertTime;
	/// <summary>
	/// 更新名称
	/// </summary>
	private string updateName;
	/// <summary>
	/// 更新日時
	/// </summary>
	private DateTime updateTime;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 実行名称を取得します。
	/// </summary>
	/// <value>実行名称</value>
	public string InvokeName {
		get => this.invokeName;
		private set => SetValue(ref this.invokeName, value, nameof(InvokeName));
	}
	/// <summary>
	/// 実行内容を取得します。
	/// </summary>
	/// <value>実行内容</value>
	public string? InvokeText {
		get => this.invokeText;
		private set => SetValue(ref this.invokeText, value, nameof(InvokeText));
	}
	/// <summary>
	/// 状態種別を取得します。
	/// </summary>
	/// <value>状態種別</value>
	public StatusType StatusCode {
		get => this.statusCode;
		private set => SetValue(ref this.statusCode, value, nameof(StatusCode));
	}
	/// <summary>
	/// 実行件数を取得します。
	/// </summary>
	/// <value>実行件数</value>
	public long? InvokeSize {
		get => this.invokeSize;
		private set => SetValue(ref this.invokeSize, value, nameof(InvokeSize));
	}
	/// <summary>
	/// 実行総数を取得します。
	/// </summary>
	/// <value>実行総数</value>
	public long? FinishSize {
		get => this.finishSize;
		private set => SetValue(ref this.finishSize, value, nameof(FinishSize));
	}
	/// <summary>
	/// 開始日時を取得します。
	/// </summary>
	/// <value>開始日時</value>
	public DateTime? InvokeTime {
		get => this.invokeTime;
		private set => SetValue(ref this.invokeTime, value, nameof(InvokeTime));
	}
	/// <summary>
	/// 終了日時を取得します。
	/// </summary>
	/// <value>終了日時</value>
	public DateTime? FinishTime {
		get => this.finishTime;
		private set => SetValue(ref this.finishTime, value, nameof(FinishTime));
	}
	/// <summary>
	/// 結果内容を取得します。
	/// </summary>
	/// <value>結果内容</value>
	public string? ResultText {
		get => this.resultText;
		private set => SetValue(ref this.resultText, value, nameof(ResultText));
	}
	/// <summary>
	/// 登録名称を取得します。
	/// </summary>
	/// <value>登録名称</value>
	public string InsertName {
		get => this.insertName;
		private set => SetValue(ref this.insertName, value, nameof(InsertName));
	}
	/// <summary>
	/// 登録日時を取得します。
	/// </summary>
	/// <value>登録日時</value>
	public DateTime InsertTime {
		get => this.insertTime;
		private set => SetValue(ref this.insertTime, value, nameof(InsertTime));
	}
	/// <summary>
	/// 更新名称を取得します。
	/// </summary>
	/// <value>更新名称</value>
	public string UpdateName {
		get => this.updateName;
		private set => SetValue(ref this.updateName, value, nameof(UpdateName));
	}
	/// <summary>
	/// 更新日時を取得します。
	/// </summary>
	/// <value>更新日時</value>
	public DateTime UpdateTime {
		get => this.updateTime;
		private set => SetValue(ref this.updateTime, value, nameof(UpdateTime));
	}
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 実行画面情報を生成します。
	/// </summary>
	/// <param name="sourceData">実行情報</param>
	internal ReportInvokeViewModel(ReportInvokeEntity sourceData) {
		this.invokeCode = sourceData.InvokeCode;
		this.invokeName = sourceData.InvokeName;
		this.invokeText = sourceData.InvokeText;
		this.statusCode = (StatusType)sourceData.StatusCode;
		this.invokeSize = sourceData.InvokeSize;
		this.finishSize = sourceData.FinishSize;
		this.invokeTime = sourceData.InvokeTime;
		this.finishTime = sourceData.FinishTime;
		this.resultText = sourceData.ResultText;
		this.insertName = sourceData.InsertName;
		this.insertTime = sourceData.InsertTime;
		this.updateName = sourceData.UpdateName;
		this.updateTime = sourceData.UpdateTime;
	}
	#endregion 生成メソッド定義

	#region 公開メソッド定義
	/// <summary>
	/// 実行番号を抽出します。
	/// </summary>
	/// <param name="sourceData">実行情報</param>
	/// <returns>実行番号</returns>
	public static int ChooseInvokeCode(ReportInvokeViewModel sourceData) => sourceData.invokeCode;
	/// <summary>
	/// 保持情報を更新します。
	/// </summary>
	/// <param name="sourceData">実行情報</param>
	internal void UpdateInvokeData(ReportInvokeEntity sourceData) {
		if (this.invokeCode == sourceData.InvokeCode) {
			InvokeName = sourceData.InvokeName;
			InvokeText = sourceData.InvokeText;
			StatusCode = (StatusType)sourceData.StatusCode;
			InvokeSize = sourceData.InvokeSize;
			FinishSize = sourceData.FinishSize;
			InvokeTime = sourceData.InvokeTime;
			FinishTime = sourceData.FinishTime;
			ResultText = sourceData.ResultText;
			InsertName = sourceData.InsertName;
			InsertTime = sourceData.InsertTime;
			UpdateName = sourceData.UpdateName;
			UpdateTime = sourceData.UpdateTime;
		} else {
			throw new SystemException("実行画面モデルの更新にて実装ミスが発生しました。");
		}
	}
	#endregion 公開メソッド定義
}
