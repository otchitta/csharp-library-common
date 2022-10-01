using System;

namespace Otchitta.Libraries.Common.Rdb;

/// <summary>
/// 処理例外クラスです。
/// </summary>
public class ServiceException : Exception {
	/// <summary>
	/// 処理例外を生成します。
	/// </summary>
	/// <param name="reason">例外理由</param>
	public ServiceException(string reason) : base(reason) {
		// 処理なし
	}
	/// <summary>
	/// 処理例外を生成します。
	/// </summary>
	/// <param name="reason">例外理由</param>
	/// <param name="source">原因例外</param>
	public ServiceException(string reason, Exception source) : base(reason, source) {
		// 処理なし
	}
}
