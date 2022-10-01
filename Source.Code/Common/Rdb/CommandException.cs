using System;

namespace Otchitta.Libraries.Common.Rdb;

/// <summary>
/// 実行例外クラスです。
/// </summary>
public class CommandException : ServiceException {
	/// <summary>
	/// 実行例外を生成します。
	/// </summary>
	/// <param name="reason">例外理由</param>
	public CommandException(string reason) : base(reason) {
		// 処理なし
	}
	/// <summary>
	/// 実行例外を生成します。
	/// </summary>
	/// <param name="reason">例外理由</param>
	/// <param name="source">原因例外</param>
	public CommandException(string reason, Exception source) : base(reason, source) {
		// 処理なし
	}
}
