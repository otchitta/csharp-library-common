using System;
using System.Xml;

namespace Otchitta.Libraries.Common.Xml;

/// <summary>
/// <see cref="XmlNode" />用例外クラスです。
/// </summary>
public class XmlNodeException : Exception {
	/// <summary>
	/// 要素情報
	/// </summary>
	private readonly XmlNode? element;
	/// <summary>
	/// 属性情報
	/// </summary>
	private readonly string? include;

	/// <summary>
	/// 例外内容を取得します。
	/// </summary>
	/// <returns>例外内容</returns>
	public override string Message => GetMessage();
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	public XmlNode? Element => this.element;
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	public string? Include => this.include;

	/// <summary>
	/// <see cref="XmlNode" />用例外を生成します。
	/// </summary>
	/// <param name="message">例外内容</param>
	/// <param name="element">要素情報</param>
	internal XmlNodeException(string message, XmlNode element) : base(message) {
		this.element = element;
		this.include = default;
	}
	/// <summary>
	/// <see cref="XmlNode" />用例外を生成します。
	/// </summary>
	/// <param name="message">例外内容</param>
	/// <param name="element">要素情報</param>
	/// <param name="include">属性情報</param>
	internal  XmlNodeException(string message, XmlNode element, string include) : base(message) {
		this.element = element;
		this.include = include;
	}

	/// <summary>
	/// 階層情報へ変換します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <returns>階層情報</returns>
	private static string ToLevelName(XmlNode source) {
		var choose = source.ParentNode;
		if (choose == null) {
			return source.Name;
		} else {
			return $"{ToLevelName(choose)}/{source.Name}";
		}
	}

	/// <summary>
	/// 例外内容を取得します。
	/// </summary>
	/// <returns>例外内容</returns>
	private string GetMessage() {
		var source = base.Message;
		if (this.element == null) {
			return source;
		} else if (String.IsNullOrEmpty(this.include)) {
			// 属性情報なし
			return $"{source}{Environment.NewLine}階層情報:{ToLevelName(this.element)}";
		} else {
			// 属性情報あり
			return $"{source}{Environment.NewLine}階層情報:{ToLevelName(this.element)}{Environment.NewLine}属性情報:{this.include}";
		}
	}
}
