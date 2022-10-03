using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading;
using Otchitta.Example.Demo01.DataModel;
using Otchitta.Libraries.Common.Rdb;

namespace Otchitta.Example.Demo01.ViewModel;

/// <summary>
/// 主要画面情報クラスです。
/// </summary>
public sealed class MainViewModel : AbstractViewModel {
	/// <summary>
	/// データベース接続名称
	/// </summary>
	private const string DatabaseConnector = "System.Data.SqlClient";
	/// <summary>
	/// データベース接続引数
	/// </summary>
	private const string DatabaseParameter = "Data Source=localhost; Initial Catalog=data_report; User ID=data_worker; Password=xxx123XXX;";

	#region メンバー変数定義
	/// <summary>
	/// 状態種別(0:未実行 1:実行中 2:停止中)
	/// <para>内部でのみ参照可能<para>
	/// </summary>
	private int statusCode;
	/// <summary>
	/// 基本一覧
	/// </summary>
	private ObservableCollection<ReportSourceViewModel> sourceList;
	/// <summary>
	/// 操作可否(True:操作可能 False:操作不可)
	/// </summary>
	private bool acceptFlag;
	/// <summary>
	/// 監視状態
	/// <para>トグルボタンの状態</para>
	/// </summary>
	private bool lookupFlag;
	/// <summary>
	/// 読込可否(True:読込可能 False:読込不可)
	/// </summary>
	private bool reloadFlag;
	/// <summary>
	/// 読込操作
	/// </summary>
	private DelegateMenuModel? reloadMenu;
	/// <summary>
	/// 結果内容
	/// </summary>
	private string? resultText;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 基本一覧を取得します。
	/// </summary>
	/// <value>基本一覧</value>
	public ReadOnlyObservableCollection<ReportSourceViewModel> SourceList {
		get;
	}
	/// <summary>
	/// 操作可否を取得します。
	/// </summary>
	/// <value>操作可可能である場合、<c>True</c>を返却</value>
	public bool AcceptFlag {
		get => this.acceptFlag;
		private set => SetValue(ref this.acceptFlag, value, nameof(AcceptFlag));
	}
	/// <summary>
	/// 監視状態を取得または設定します。
	/// </summary>
	/// <value>監視状態</value>
	public bool LookupFlag {
		get => this.lookupFlag;
		set => SetValue(ref this.lookupFlag, value, nameof(LookupFlag), ActionLookupFlag);
	}
	/// <summary>
	/// 読込可否を取得します。
	/// </summary>
	/// <value>読込可能である場合、<c>True</c>を返却</value>
	public bool ReloadFlag {
		get => this.reloadFlag;
		private set => SetValue(ref this.reloadFlag, value, nameof(ReloadFlag), ActionReloadFlag);
	}
	/// <summary>
	/// 読込操作を取得します。
	/// </summary>
	/// <returns>読込操作</returns>
	public AbstractMenuModel ReloadMenu => this.reloadMenu ??= new DelegateMenuModel(ActionReloadMenu, AcceptReloadMenu);
	/// <summary>
	/// 結果内容を取得します。
	/// </summary>
	/// <value>結果内容</value>
	public string? ResultText {
		get => this.resultText;
		private set => SetValue(ref this.resultText, value, nameof(ResultText));
	}
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 主要画面情報を生成します。
	/// </summary>
	public MainViewModel() {
		// 共通設定
		System.Data.Common.DbProviderFactories.RegisterFactory(DatabaseConnector, System.Data.SqlClient.SqlClientFactory.Instance);
		Otchitta.Example.Demo01.Entity.CommandExtension.SetCommandPath(Otchitta.Libraries.Common.ExeFileUtilities.ExecutePath, DatabaseConnector);
		// 要素設定
		this.statusCode = 0;
		this.sourceList = new ObservableCollection<ReportSourceViewModel>();
		this.acceptFlag = true;
		this.lookupFlag = false;
		this.reloadFlag = true;
		this.reloadMenu = null;
		this.resultText = null;
		SourceList = new ReadOnlyObservableCollection<ReportSourceViewModel>(this.sourceList);
	}
	#endregion 生成メソッド定義

	#region 内部メソッド定義(監視状態関連:ActionLookupFlag)
	/// <summary>
	/// 監視状態を処理します。
	/// </summary>
	private void ActionLookupFlag() {
		ResultText = null;
		if (this.lookupFlag) {
			// 監視処理開始
			this.statusCode = 1;
			ReloadFlag = false; // 読込不可に変更
			TaskWorker.InvokeData(InvokeSourceList, Int32.MaxValue - 1);
		} else {
			// 監視処理終了
			AcceptFlag = false; // 操作不可に変更
			ReloadFlag = false; // 読込不可に変更
			this.statusCode = 2;
		}
	}
	#endregion 内部メソッド定義(監視状態関連:ActionLookupFlag)

	#region 内部メソッド定義(基本一覧関連:InvokeSourceList/UpdateSourceList/FinishSourceList)
	/// <summary>
	/// 基本一覧を監視します。
	/// </summary>
	/// <param name="count">読込回数</param>
	private void InvokeSourceList(int count) {
		try {
			using (var connection = ConnectUtilities.Create(DatabaseConnector, DatabaseParameter))
			using (var command = connection.CreateCommand()) {
				for (var index = 0; index < count; index ++) {
					// 監視終了判定
					if (this.statusCode != 1) break;
					// 取込実行待機
					if (index != 0) Thread.Sleep(1000); // 監視間隔：２秒
					// 転送処理実行
					InvokeSourceList(command);
				}
			}
			TaskWorker.InvokeView(FinishSourceList);
		} catch (Exception error) {
			TaskWorker.InvokeView(FinishSourceList, error);
		}
	}
	/// <summary>
	/// 基本一覧を転送します。
	/// </summary>
	/// <param name="command">実行処理</param>
	private void InvokeSourceList(IDbCommand command) =>
		TaskWorker.InvokeView(UpdateSourceList, ReportSourceListModel.Create(command));
	/// <summary>
	/// 基本一覧を更新します。
	/// </summary>
	/// <param name="updateList">基本一覧</param>
	private void UpdateSourceList(ReportSourceListModel updateList) {
		// 更新処理
		foreach (var (screenData, sourceData) in updateList.CreateUpdateList(this.sourceList, ReportSourceViewModel.ChooseSourceCode)) {
			screenData.UpdateSourceData(sourceData);
		}
		// 削除処理
		foreach (var screenData in updateList.CreateDeleteList(this.sourceList, ReportSourceViewModel.ChooseSourceCode)) {
			this.sourceList.Remove(screenData);
		}
		// 登録処理
		foreach (var sourceData in updateList.CreateInsertList(this.sourceList, ReportSourceViewModel.ChooseSourceCode)) {
			this.sourceList.Add(new ReportSourceViewModel(sourceData));
		}
	}
	/// <summary>
	/// 監視処理を終了します。
	/// </summary>
	private void FinishSourceList() {
		this.statusCode = 0;
		ResultText = null;
		AcceptFlag = true; // 操作可能に変更
		ReloadFlag = true; // 読込可能に変更
	}
	/// <summary>
	/// 監視処理を終了します。
	/// </summary>
	/// <param name="resultData">例外情報</param>
	private void FinishSourceList(Exception resultData) {
		this.statusCode = 0;
		ResultText = resultData.ToString();
		AcceptFlag = true; // 操作可能に変更
		ReloadFlag = true; // 読込可能に変更
	}
	#endregion 内部メソッド定義(基本一覧関連:InvokeSourceList/UpdateSourceList/FinishSourceList)

	#region 内部メソッド定義(読込操作関連:ActionReloadFlag/AcceptReloadMenu/ActionReloadMenu)
	/// <summary>
	/// 読込可否を処理します。
	/// </summary>
	private void ActionReloadFlag() => this.reloadMenu?.Notify();
	/// <summary>
	/// 読込可否を判定します。
	/// </summary>
	/// <returns>読込可能である場合、<c>True</c>を返却</returns>
	private bool AcceptReloadMenu() => this.reloadFlag;
	/// <summary>
	/// 読込操作を開始します。
	/// </summary>
	private void ActionReloadMenu() {
		this.statusCode = 1;
		AcceptFlag = false; // 操作不可に変更
		ReloadFlag = false; // 読込不可に変更
		TaskWorker.InvokeData(InvokeSourceList, 1);
	}
	#endregion 内部メソッド定義(読込操作関連:ActionReloadFlag/AcceptReloadMenu/ActionReloadMenu)
}
