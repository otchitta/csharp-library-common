using Otchitta.Example.Demo01.Entity.Report;

namespace Otchitta.Example.Demo01.Worker;

/// <summary>
/// 実行情報クラスです。
/// </summary>
/// <typeparam name="TSource">要素種別</typeparam>
public sealed class ReportInvokeSource<TSource> {
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <value>要素情報</value>
	public TSource SourceData {
		get;
	}
	/// <summary>
	/// 実行情報を取得します。
	/// </summary>
	/// <value>実行情報</value>
	internal ReportInvokeEntity EntityData {
		get;
	}

	/// <summary>
	/// 実行情報を生成します。
	/// </summary>
	/// <param name="sourceData">要素情報</param>
	/// <param name="entityData">実行情報</param>
	internal ReportInvokeSource(TSource sourceData, ReportInvokeEntity entityData) {
		SourceData = sourceData;
		EntityData = entityData;
	}
}
