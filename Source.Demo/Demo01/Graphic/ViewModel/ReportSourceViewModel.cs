using System;
using System.Collections.ObjectModel;
using Otchitta.Example.Demo01.Constant;
using Otchitta.Example.Demo01.DataModel;

namespace Otchitta.Example.Demo01.ViewModel;

/// <summary>
/// 基本画面情報クラスです。
/// </summary>
public sealed class ReportSourceViewModel : AbstractViewModel {
	#region メンバー変数定義
	/// <summary>
	/// 基本番号
	/// </summary>
	private readonly int sourceCode;
	/// <summary>
	/// 基本名称
	/// </summary>
	private string sourceName;
	/// <summary>
	/// 基本内容
	/// </summary>
	private string? sourceText;
	/// <summary>
	/// 状態種別
	/// </summary>
	private StatusType statusCode;
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
	/// <summary>
	/// 実行一覧
	/// </summary>
	private ObservableCollection<ReportInvokeViewModel> invokeList;
	/// <summary>
	/// 結果種別
	/// </summary>
	private ResultType resultCode;
	/// <summary>
	/// 全体件数
	/// </summary>
	private int? summaryCount;
	/// <summary>
	/// 待機件数
	/// </summary>
	private int? nothingCount;
	/// <summary>
	/// 処理件数
	/// </summary>
	private int? processCount;
	/// <summary>
	/// 成功件数
	/// </summary>
	private int? successCount;
	/// <summary>
	/// 失敗件数
	/// </summary>
	private int? failureCount;
	/// <summary>
	/// 取消件数
	/// </summary>
	private int? suspendCount;
	/// <summary>
	/// 実行個数
	/// </summary>
	private long? invokeLength;
	/// <summary>
	/// 全体個数
	/// </summary>
	private long? finishLength;
	/// <summary>
	/// 実行割合
	/// </summary>
	private decimal? processRatio;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 基本名称を取得します。
	/// </summary>
	/// <value>基本名称</value>
	public string SourceName {
		get => this.sourceName;
		private set => SetValue(ref this.sourceName, value, nameof(SourceName));
	}
	/// <summary>
	/// 基本内容を取得します。
	/// </summary>
	/// <value>基本内容</value>
	public string? SourceText {
		get => this.sourceText;
		private set => SetValue(ref this.sourceText, value, nameof(SourceText));
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
	/// <summary>
	/// 実行一覧を取得します。
	/// </summary>
	/// <value>実行一覧</value>
	public ReadOnlyObservableCollection<ReportInvokeViewModel> InvokeList {
		get;
	}
	/// <summary>
	/// 結果種別を取得します。
	/// </summary>
	/// <value>結果種別</value>
	public ResultType ResultCode {
		get => this.resultCode;
		private set => SetValue(ref this.resultCode, value, nameof(ResultCode));
	}
	/// <summary>
	/// 全体件数を取得します。
	/// </summary>
	/// <value>全体件数</value>
	public int? SummaryCount {
		get => this.summaryCount;
		private set => SetValue(ref this.summaryCount, value, nameof(SummaryCount));
	}
	/// <summary>
	/// 待機件数を取得します。
	/// </summary>
	/// <value>待機件数</value>
	public int? NothingCount {
		get => this.nothingCount;
		private set => SetValue(ref this.nothingCount, value, nameof(NothingCount));
	}
	/// <summary>
	/// 処理件数を取得します。
	/// </summary>
	/// <value>処理件数</value>
	public int? ProcessCount {
		get => this.processCount;
		private set => SetValue(ref this.processCount, value, nameof(ProcessCount));
	}
	/// <summary>
	/// 成功件数を取得します。
	/// </summary>
	/// <value>成功件数</value>
	public int? SuccessCount {
		get => this.successCount;
		private set => SetValue(ref this.successCount, value, nameof(SuccessCount));
	}
	/// <summary>
	/// 失敗件数を取得します。
	/// </summary>
	/// <value>失敗件数</value>
	public int? FailureCount {
		get => this.failureCount;
		private set => SetValue(ref this.failureCount, value, nameof(FailureCount));
	}
	/// <summary>
	/// 取消件数を取得します。
	/// </summary>
	/// <value>取消件数</value>
	public int? SuspendCount {
		get => this.suspendCount;
		private set => SetValue(ref this.suspendCount, value, nameof(SuspendCount));
	}
	/// <summary>
	/// 実行個数を取得します。
	/// </summary>
	/// <value>実行個数</value>
	public long? InvokeLength {
		get => this.invokeLength;
		private set => SetValue(ref this.invokeLength, value, nameof(InvokeLength));
	}
	/// <summary>
	/// 全体個数を取得します。
	/// </summary>
	/// <value>全体個数</value>
	public long? FinishLength {
		get => this.finishLength;
		private set => SetValue(ref this.finishLength, value, nameof(FinishLength));
	}
	/// <summary>
	/// 実行割合を取得します。
	/// </summary>
	/// <value></value>
	public decimal? ProcessRatio {
		get => this.processRatio;
		private set => SetValue(ref this.processRatio, value, nameof(ProcessRatio));
	}
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 基本画面情報を生成します。
	/// </summary>
	/// <param name="doubleData">基本情報</param>
	internal ReportSourceViewModel(ReportSourceDataModel doubleData) {
		var sourceData = doubleData.SourceData;
		var invokeList = doubleData.InvokeList;
		this.sourceCode = sourceData.SourceCode;
		this.sourceName = sourceData.SourceName;
		this.sourceText = sourceData.SourceText;
		this.statusCode = (StatusType)sourceData.StatusCode;
		this.invokeTime = sourceData.InvokeTime;
		this.finishTime = sourceData.FinishTime;
		this.resultText = sourceData.ResultText;
		this.insertName = sourceData.InsertName;
		this.insertTime = sourceData.InsertTime;
		this.updateName = sourceData.UpdateName;
		this.updateTime = sourceData.UpdateTime;
		this.invokeList = new ObservableCollection<ReportInvokeViewModel>();
		this.resultCode = ResultType.Nothing;
		this.summaryCount = null;
		this.nothingCount = null;
		this.processCount = null;
		this.successCount = null;
		this.failureCount = null;
		this.suspendCount = null;
		this.invokeLength = null;
		this.finishLength = null;
		this.processRatio = null;
		UpdateInvokeList(invokeList);
		UpdateResultCode();
		UpdateExtendData();
		InvokeList = new ReadOnlyObservableCollection<ReportInvokeViewModel>(this.invokeList);
	}
	#endregion 生成メソッド定義

	#region 内部メソッド定義(実行一覧関連:UpdateInvokeList)
	/// <summary>
	/// 実行一覧を更新します。
	/// </summary>
	/// <param name="updateList">更新一覧</param>
	private void UpdateInvokeList(ReportInvokeListModel updateList) {
		// 更新処理
		foreach (var (invokeData, updateData) in updateList.CreateUpdateList(this.invokeList, ReportInvokeViewModel.ChooseInvokeCode)) {
			invokeData.UpdateInvokeData(updateData);
		}
		// 削除処理
		foreach (var invokeData in updateList.CreateDeleteList(this.invokeList, ReportInvokeViewModel.ChooseInvokeCode)) {
			this.invokeList.Remove(invokeData);
		}
		// 登録処理
		foreach (var updateData in updateList.CreateInsertList(this.invokeList, ReportInvokeViewModel.ChooseInvokeCode)) {
			this.invokeList.Add(new ReportInvokeViewModel(updateData));
		}
	}
	#endregion 内部メソッド定義(実行一覧関連:UpdateInvokeList)

	#region 内部メソッド定義(結果種別関連:ChooseResultCode/UpdateResultCode)
	/// <summary>
	/// 結果種別を選別します。
	/// <param>引数のどちらか優先度の高い情報を返却します。</param>
	/// </summary>
	/// <param name="item1">結果種別</param>
	/// <param name="item2">結果種別</param>
	/// <returns>結果種別</returns>
	private static ResultType ChooseResultCode(ResultType item1, ResultType item2) =>
		item1 < item2? item2: item1;
	/// <summary>
	/// 結果種別を取得します。
	/// </summary>
	/// <returns>結果種別</returns>
	public ResultType ChooseResultCode() {
		var resultCode = ResultType.Success; // 実行一覧が空であっても「正常」とする
		foreach (var invokeData in this.invokeList) {
			switch (invokeData.StatusCode) {
				default:                 resultCode = ChooseResultCode(resultCode, ResultType.Failure); break; // 想定外コード：失敗へ引き上げ
				case StatusType.Nothing: resultCode = ChooseResultCode(resultCode, ResultType.Warning); break; // 自身が実行済であるが「待機」は想定外：警告へ引き上げ
				case StatusType.Process: resultCode = ChooseResultCode(resultCode, ResultType.Warning); break; // 自身が実行済であるが「実行」は想定外：警告へ引き上げ
				case StatusType.Success: resultCode = ChooseResultCode(resultCode, ResultType.Success); break;
				case StatusType.Failure: resultCode = ChooseResultCode(resultCode, ResultType.Failure); break;
				case StatusType.Suspend: resultCode = ChooseResultCode(resultCode, ResultType.Warning); break; // 自身が実行済であるが「取消」は想定外：警告へ引き上げ
			}
		}
		return resultCode;
	}
	/// <summary>
	/// 結果種別を更新します。
	/// </summary>
	private void UpdateResultCode() {
		switch (this.statusCode) {
		default: // 想定外コード
		case StatusType.Nothing: // 未実行：想定外
			ResultCode = ResultType.Failure;
			break;
		case StatusType.Process: // 実行中
			ResultCode = ResultType.Nothing;
			break;
		case StatusType.Success: // 実行済
		case StatusType.Failure: // 実行済
			ResultCode = ChooseResultCode();
			break;
		case StatusType.Suspend: // 取消済
			ResultCode = ResultType.Warning; // 注意喚起の為、警告設定
			break;
		}
	}
	#endregion 内部メソッド定義(結果種別関連:ChooseResultCode/UpdateResultCode)

	#region 内部メソッド定義(拡張情報関連:UpdateExtendData)
	/// <summary>
	/// 拡張情報を更新します。
	/// </summary>
	private void UpdateExtendData() {
		var summaryCount = 0;
		var nothingCount = 0;
		var processCount = 0;
		var successCount = 0;
		var failureCount = 0;
		var suspendCount = 0;
		var invokeLength = 0L;
		var finishLength = 0L;
		switch (this.statusCode) {
		case StatusType.Nothing: // 待機：想定外
			break;
		case StatusType.Process: // 開始
		case StatusType.Success: // 成功
		case StatusType.Failure: // 失敗
			foreach (var invokeData in this.invokeList) {
				summaryCount ++;
				switch (invokeData.StatusCode) {
				case StatusType.Nothing: // 待機
					nothingCount ++;
					finishLength += invokeData.FinishSize ?? 0;
					break;
				case StatusType.Process: // 開始
					processCount ++;
					invokeLength += invokeData.InvokeSize ?? 0;
					finishLength += invokeData.FinishSize ?? 0;
					break;
				case StatusType.Success: // 成功
					successCount ++;
					invokeLength += invokeData.InvokeSize ?? 0;
					finishLength += invokeData.FinishSize ?? 0;
					break;
				case StatusType.Failure: // 失敗
					failureCount ++;
					invokeLength += invokeData.FinishSize ?? 0; // 失敗した場合、処理件数が全体件数となっていない為、全体件数で補完
					finishLength += invokeData.FinishSize ?? 0;
					break;
				case StatusType.Suspend: // 取消：想定外(取消されたら自身も取消となっている)
					suspendCount ++;
					invokeLength += invokeData.InvokeSize ?? 0;
					finishLength += invokeData.FinishSize ?? 0;
					break;
				}
			}
			break;
		case StatusType.Suspend: // 取消
			foreach (var invokeData in this.invokeList) {
				summaryCount ++;
				switch (invokeData.StatusCode) {
				case StatusType.Nothing: // 待機
					nothingCount ++;
					finishLength += invokeData.FinishSize ?? 0;
					break;
				case StatusType.Process: // 開始
					processCount ++;
					invokeLength += invokeData.InvokeSize ?? 0;
					finishLength += invokeData.FinishSize ?? 0;
					break;
				case StatusType.Success: // 成功
					successCount ++;
					invokeLength += invokeData.InvokeSize ?? 0;
					finishLength += invokeData.FinishSize ?? 0;
					break;
				case StatusType.Failure: // 失敗
					failureCount ++;
					invokeLength += invokeData.FinishSize ?? 0; // 失敗した場合、処理件数が全体件数となっていない為、全体件数で補完
					finishLength += invokeData.FinishSize ?? 0;
					break;
				case StatusType.Suspend: // 取消
					suspendCount ++;
					invokeLength += invokeData.InvokeSize ?? 0;
					finishLength += invokeData.FinishSize ?? 0;
					break;
				}
			}
			break;
		}
		if (summaryCount == 0) {
			// データなし：全てクリア
			SummaryCount = null;
			NothingCount = null;
			ProcessCount = null;
			SuccessCount = null;
			FailureCount = null;
			SuspendCount = null;
			InvokeLength = null;
			FinishLength = null;
			ProcessRatio = null;
		} else {
			// データあり：データ設定
			SummaryCount = summaryCount;
			NothingCount = nothingCount;
			ProcessCount = processCount;
			SuccessCount = successCount;
			FailureCount = failureCount;
			SuspendCount = suspendCount;
			InvokeLength = invokeLength;
			FinishLength = finishLength;
			if (finishLength == 0) {
				ProcessRatio = null;
			} else {
				ProcessRatio = (decimal)invokeLength / finishLength;
			}
		}
	}
	#endregion 内部メソッド定義(拡張情報関連:UpdateExtendData)

	#region 公開メソッド定義(更新処理関連:UpdateSourceData)
	/// <summary>
	/// 保持情報を更新します。
	/// </summary>
	/// <param name="updateData">更新情報</param>
	internal void UpdateSourceData(ReportSourceDataModel updateData) {
		var sourceData = updateData.SourceData;
		if (sourceData.SourceCode == this.sourceCode) {
			SourceName = sourceData.SourceName;
			SourceText = sourceData.SourceText;
			StatusCode = (StatusType)sourceData.StatusCode;
			InvokeTime = sourceData.InvokeTime;
			FinishTime = sourceData.FinishTime;
			ResultText = sourceData.ResultText;
			InsertName = sourceData.InsertName;
			InsertTime = sourceData.InsertTime;
			UpdateName = sourceData.UpdateName;
			UpdateTime = sourceData.UpdateTime;
			UpdateInvokeList(updateData.InvokeList);
			UpdateResultCode();
			UpdateExtendData();
		} else {
			throw new SystemException("基本画面モデルの更新にて実装ミスが発生しました。");
		}
	}
	#endregion 公開メソッド定義(更新処理関連:UpdateSourceData)

	#region 公開メソッド定義(基本番号関連:ChooseSourceCode)
	/// <summary>
	/// 基本番号を抽出します。
	/// </summary>
	/// <param name="sourceData">基本情報</param>
	/// <returns>基本番号</returns>
	public static int ChooseSourceCode(ReportSourceViewModel sourceData) => sourceData.sourceCode;
	#endregion 公開メソッド定義(基本番号関連:ChooseSourceCode)
}
