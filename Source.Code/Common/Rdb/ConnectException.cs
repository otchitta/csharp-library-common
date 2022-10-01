using System;

namespace Otchitta.Libraries.Common.Rdb;

/// <summary>
/// 接続例外クラスです。
/// </summary>
public class ConnectException : ServiceException {
	/// <summary>
	/// 接続名称
	/// </summary>
	private readonly string connector;
	/// <summary>
	/// 接続引数
	/// </summary>
	private readonly string? parameter;

	/// <summary>
	/// 例外内容を取得します。
	/// </summary>
	/// <returns>例外内容</returns>
	public override string Message => GetMessage();
	/// <summary>
	/// 接続名称を取得します。
	/// </summary>
	/// <value>接続名称</value>
	public string Connector => this.connector;
	/// <summary>
	/// 接続引数を取得します。
	/// </summary>
	/// <value>接続引数</value>
	public string? Parameter => this.parameter;

	/// <summary>
	/// 接続例外を生成します。
	/// </summary>
	/// <param name="message">例外内容</param>
	/// <param name="connector">接続名称</param>
	internal ConnectException(string message, string connector) : base(message) {
		this.connector = connector;
		this.parameter = default;
	}
	/// <summary>
	/// 接続例外を生成します。
	/// </summary>
	/// <param name="message">例外内容</param>
	/// <param name="connector">接続名称</param>
	/// <param name="parameter">接続名称</param>
	/// <param name="exception">原因例外</param>
	internal ConnectException(string message, string connector, string parameter, Exception exception) : base(message, exception) {
		this.connector = connector;
		this.parameter = parameter;
	}

	/// <summary>
	/// 例外内容を取得します。
	/// </summary>
	/// <returns>例外内容</returns>
	private string GetMessage() {
		var source = base.Message;
		if (String.IsNullOrEmpty(this.parameter)) {
			// 接続引数なし
			return $"{source}{Environment.NewLine}接続名称:{this.connector}";
		} else {
			// 接続引数あり
			return $"{source}{Environment.NewLine}接続名称:{this.connector}{Environment.NewLine}接続引数:{this.parameter}";
		}
	}
}
