using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Otchitta.Example.Demo01.Entity.Report;

namespace Otchitta.Example.Demo01.DataModel;

/// <summary>
/// 実行一覧モデルクラスです。
/// </summary>
public sealed class ReportInvokeListModel {
	#region メンバー変数定義
	/// <summary>
	/// 実行一覧
	/// </summary>
	private readonly Dictionary<int, ReportInvokeEntity> invokeList;
	#endregion メンバー変数定義

	#region 生成メソッド定義
	/// <summary>
	/// 実行一覧モデルを生成します。
	/// </summary>
	/// <param name="invokeList">実行一覧</param>
	private ReportInvokeListModel(Dictionary<int, ReportInvokeEntity> invokeList) {
		this.invokeList = invokeList;
	}
	/// <summary>
	/// 実行一覧モデルを生成します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <param name="sourceCode">基本番号</param>
	/// <returns>実行一覧モデル</returns>
	public static ReportInvokeListModel Create(IDbCommand invokeData, int sourceCode) {
		var resultList = new Dictionary<int, ReportInvokeEntity>();
		foreach (var selectData in ReportInvokeEntity.SelectList(invokeData, sourceCode)) {
			resultList.Add(selectData.InvokeCode, selectData);
		}
		return new ReportInvokeListModel(resultList);
	}
	#endregion 生成メソッド定義

	#region 公開メソッド定義(CreateInsertList/CreateUpdateList/CreateDeleteList)
	/// <summary>
	/// 登録一覧を生成します。
	/// </summary>
	/// <param name="invokeList">実行一覧</param>
	/// <param name="invokeHook">変換処理</param>
	/// <typeparam name="TValue">実行種別</typeparam>
	/// <returns>登録一覧</returns>
	public List<ReportInvokeEntity> CreateInsertList<TValue>(IEnumerable<TValue> invokeList, Func<TValue, int> invokeHook) {
		var resultList = new List<ReportInvokeEntity>(this.invokeList.Values);
		foreach (var invokeData in invokeList) {
			var invokeCode = invokeHook(invokeData);
			resultList.RemoveAll(chooseData => chooseData.InvokeCode == invokeCode);
		}
		return resultList;
	}
	/// <summary>
	/// 更新一覧を生成します。
	/// </summary>
	/// <param name="invokeList">実行一覧</param>
	/// <param name="invokeHook">変換処理</param>
	/// <typeparam name="TValue">実行種別</typeparam>
	/// <returns>更新一覧</returns>
	public List<(TValue, ReportInvokeEntity)> CreateUpdateList<TValue>(IEnumerable<TValue> invokeList, Func<TValue, int> invokeHook) {
		var resultList = new List<(TValue, ReportInvokeEntity)>();
		foreach (var invokeData in invokeList) {
			if (this.invokeList.TryGetValue(invokeHook(invokeData), out var resultData)) {
				resultList.Add((invokeData, resultData));
			}
		}
		return resultList;
	}
	/// <summary>
	/// 削除一覧を生成します。
	/// </summary>
	/// <param name="invokeList">実行一覧</param>
	/// <param name="invokeHook">変換処理</param>
	/// <typeparam name="TValue">実行種別</typeparam>
	/// <returns>削除一覧</returns>
	public List<TValue> CreateDeleteList<TValue>(IEnumerable<TValue> invokeList, Func<TValue, int> invokeHook) {
		var resultList = new List<TValue>();
		foreach (var invokeData in invokeList) {
			if (this.invokeList.TryGetValue(invokeHook(invokeData), out var chooseData) == false) {
				resultList.Add(invokeData);
			}
		}
		return resultList;
	}
	#endregion 公開メソッド定義(CreateInsertList/CreateUpdateList/CreateDeleteList)

	#region 公開メソッド定義(ChooseResultType)
	/// <summary>
	/// 実行一覧を生成します。
	/// </summary>
	/// <returns>実行一覧</returns>
	public ReadOnlyCollection<ReportInvokeEntity> CreateInvokeList() =>
		new ReadOnlyCollection<ReportInvokeEntity>(new List<ReportInvokeEntity>(this.invokeList.Values));
	#endregion 公開メソッド定義(ChooseResultType)
}
