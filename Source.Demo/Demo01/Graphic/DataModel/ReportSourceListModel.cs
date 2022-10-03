using System;
using System.Collections.Generic;
using System.Data;
using Otchitta.Example.Demo01.Entity.Report;

namespace Otchitta.Example.Demo01.DataModel;

/// <summary>
/// 基本一覧モデルクラスです。
/// </summary>
public sealed class ReportSourceListModel {
	#region メンバー変数定義
	/// <summary>
	/// 基本一覧
	/// </summary>
	private readonly Dictionary<int, ReportSourceDataModel> sourceList;
	#endregion メンバー変数定義

	#region 生成メソッド定義
	/// <summary>
	/// 基本一覧モデルを生成します。
	/// </summary>
	/// <param name="sourceList">基本一覧</param>
	private ReportSourceListModel(Dictionary<int, ReportSourceDataModel> sourceList) {
		this.sourceList = sourceList;
	}
	/// <summary>
	/// 基本一覧モデルを生成します。
	/// </summary>
	/// <param name="invokeData">実行処理</param>
	/// <returns>基本一覧モデル</returns>
	public static ReportSourceListModel Create(IDbCommand invokeData) {
		var resultList = new Dictionary<int, ReportSourceDataModel>();
		foreach (var selectData in ReportSourceEntity.SelectList(invokeData)) {
			resultList.Add(selectData.SourceCode, ReportSourceDataModel.Create(invokeData, selectData));
		}
		return new ReportSourceListModel(resultList);
	}
	#endregion 生成メソッド定義

	#region 公開メソッド定義
	/// <summary>
	/// 登録一覧を生成します。
	/// </summary>
	/// <param name="sourceList">基本一覧</param>
	/// <param name="sourceHook">変換処理</param>
	/// <typeparam name="TValue">基本種別</typeparam>
	/// <returns>登録一覧</returns>
	public List<ReportSourceDataModel> CreateInsertList<TValue>(IEnumerable<TValue> sourceList, Func<TValue, int> sourceHook) {
		var resultList = new List<ReportSourceDataModel>(this.sourceList.Values);
		foreach (var sourceData in sourceList) {
			var sourceCode = sourceHook(sourceData);
			resultList.RemoveAll(chooseData => chooseData.SourceCode == sourceCode);
		}
		return resultList;
	}
	/// <summary>
	/// 更新一覧を生成します。
	/// </summary>
	/// <param name="sourceList">基本一覧</param>
	/// <param name="sourceHook">変換処理</param>
	/// <typeparam name="TValue">基本種別</typeparam>
	/// <returns>更新一覧</returns>
	public List<(TValue, ReportSourceDataModel)> CreateUpdateList<TValue>(IEnumerable<TValue> sourceList, Func<TValue, int> sourceHook) {
		var resultList = new List<(TValue, ReportSourceDataModel)>();
		foreach (var sourceData in sourceList) {
			if (this.sourceList.TryGetValue(sourceHook(sourceData), out var resultData)) {
				resultList.Add((sourceData, resultData));
			}
		}
		return resultList;
	}
	/// <summary>
	/// 削除一覧を生成します。
	/// </summary>
	/// <param name="sourceList">基本一覧</param>
	/// <param name="sourceHook">変換処理</param>
	/// <typeparam name="TValue">基本種別</typeparam>
	/// <returns>削除一覧</returns>
	public List<TValue> CreateDeleteList<TValue>(IEnumerable<TValue> sourceList, Func<TValue, int> sourceHook) {
		var resultList = new List<TValue>();
		foreach (var sourceData in sourceList) {
			if (this.sourceList.TryGetValue(sourceHook(sourceData), out var chooseData) == false) {
				resultList.Add(sourceData);
			}
		}
		return resultList;
	}
	#endregion 公開メソッド定義
}
