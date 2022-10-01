using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Otchitta.Libraries.Common.Rdb;

/// <summary>
/// 接続共通関数クラスです。
/// </summary>
public static class ConnectUtilities {
	#region 内部メソッド定義
	/// <summary>
	/// 生成処理を取得します。
	/// </summary>
	/// <param name="source">接続名称</param>
	/// <param name="result">生成処理</param>
	/// <returns><paramref name="source" />の取得に成功した場合、<c>True</c>を返却</returns>
	private static bool Phase1(string source, [MaybeNullWhen(false)]out DbProviderFactory result) =>
		DbProviderFactories.TryGetFactory(source, out result);
	/// <summary>
	/// 接続処理を生成します。
	/// </summary>
	/// <param name="source">生成処理</param>
	/// <param name="result">接続処理</param>
	/// <returns><paramref name="source" />の生成が成功した場合、<c>True</c>を返却</returns>
	private static bool Phase2(DbProviderFactory source, [MaybeNullWhen(false)]out DbConnection result) =>
		(result = source.CreateConnection()) != null;
	/// <summary>
	/// 接続引数を適用します。
	/// </summary>
	/// <param name="source">接続処理</param>
	/// <param name="values">接続引数</param>
	/// <param name="result">例外情報</param>
	/// <returns><paramref name="source" />の適用が成功した場合、<c>True</c>を返却</returns>
	private static bool Phase3(DbConnection source, string values, [MaybeNullWhen(true)]out Exception result) {
		try {
			source.ConnectionString = values;
			result = default;
			return true;
		} catch (Exception errors) {
			result = errors;
			return false;
		}
	}
	/// <summary>
	/// 接続処理を実行します。
	/// </summary>
	/// <param name="source">接続処理</param>
	/// <param name="result">例外情報</param>
	/// <returns><paramref name="source" />の実行が成功した場合、<c>True</c>を返却</returns>
	private static bool Pahse4(DbConnection source, [MaybeNullWhen(true)]out Exception result) {
		try {
			source.Open();
			result = default;
			return true;
		} catch (Exception errors) {
			result = errors;
			return false;
		}
	}
	#endregion 内部メソッド定義

	#region 公開メソッド定義
	/// <summary>
	/// 接続処理を実行します。
	/// </summary>
	/// <param name="connector">接続名称</param>
	/// <param name="parameter">接続引数</param>
	/// <returns>接続処理</returns>
	/// <exception cref="ConnectException">接続処理に失敗した場合</exception>
	public static DbConnection Create(string connector, string parameter) {
		if (Phase1(connector, out var phase1) == false) {
			// 取得に失敗した場合
			throw new ConnectException("データベース処理の特定に失敗しました", connector);
		} else if (Phase2(phase1, out var phase2) == false) {
			// 生成に失敗した場合
			throw new ConnectException("データベース処理の生成に失敗しました", connector);
		} else if (Phase3(phase2, parameter, out var phase3) == false) {
			// 適用に失敗した場合
			throw new ConnectException("データベース引数の適用に失敗しました", connector, parameter, phase3);
		} else if (Pahse4(phase2, out var phase4) == false) {
			// 接続に失敗した場合
			throw new ConnectException("データベース処理の接続に失敗しました", connector, parameter, phase4);
		} else {
			// 上記に成功した場合
			return phase2;
		}
	}
	#endregion 公開メソッド定義
}
