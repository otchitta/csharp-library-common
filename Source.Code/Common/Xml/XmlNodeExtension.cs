using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;

namespace Otchitta.Libraries.Common.Xml;

/// <summary>
/// <see cref="XmlNode" />用拡張関数クラスです。
/// </summary>
public static class XmlNodeExtension {
	#region 内部メソッド定義(GetList/GetString)
	/// <summary>
	/// 下位一覧を取得します。
	/// </summary>
	/// <param name="elementList">要素一覧</param>
	/// <param name="elementName">下位名称</param>
	/// <returns>下位一覧</returns>
	private static List<XmlNode> GetList(XmlNodeList elementList, string elementName) {
		var result = new List<XmlNode>();
		for (var index = 0; index < elementList.Count; index ++) {
			var choose = elementList[index];
			if (choose != null && choose.Name == elementName) {
				result.Add(choose);
			}
		}
		return result;
	}
	/// <summary>
	/// 下位情報を取得します。
	/// </summary>
	/// <param name="elementList">要素一覧</param>
	/// <param name="elementName">下位名称</param>
	/// <param name="includeData">下位情報</param>
	/// <returns><paramref name="elementName" />が存在した場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が複数存在する場合</exception>
	private static bool GetData(XmlNodeList elementList, string elementName, [MaybeNullWhen(false)]out XmlNode includeData) {
		includeData = null;
		for (var index = 0; index < elementList.Count; index ++) {
			var choose = elementList[index];
			if (choose != null && choose.Name == elementName) {
				if (includeData == null) {
					includeData = choose;
				} else {
					throw new XmlNodeException($"{elementName}タグが{includeData.ParentNode?.Name}タグに複数定義されています", choose);
				}
			}
		}
		return includeData != null;
	}
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="includeData">属性情報</param>
	/// <returns><paramref name="includeData" />が存在した場合、<c>True</c>を返却</returns>
	private static bool GetString(XmlAttribute? elementData, [MaybeNullWhen(false)]out string includeData) =>
		(includeData = elementData?.Value) != null;
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementList">要素一覧</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="includeData">属性情報</param>
	/// <returns><paramref name="elementName" />が存在した場合、<c>True</c>を返却</returns>
	private static bool GetString(XmlAttributeCollection? elementList, string elementName, [MaybeNullWhen(false)]out string includeData) =>
		GetString(elementList?[elementName], out includeData);
	#endregion 内部メソッド定義(GetList/GetString)

	#region 公開メソッド定義(GetList)
	/// <summary>
	/// 下位一覧を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">要素名称</param>
	/// <returns>下位一覧</returns>
	public static List<XmlNode> GetList(this XmlNode elementData, string elementName) =>
		GetList(elementData.ChildNodes, elementName);
	/// <summary>
	/// 下位一覧を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">要素名称</param>
	/// <param name="convertHook">変換処理</param>
	/// <typeparam name="TResult">下位種別</typeparam>
	/// <returns>下位一覧</returns>
	public static List<TResult> GetList<TResult>(this XmlNode elementData, string elementName, Func<XmlNode, TResult> convertHook) {
		var result = new List<TResult>();
		foreach (var choose in GetList(elementData.ChildNodes, elementName)) {
			result.Add(convertHook(choose));
		}
		return result;
	}
	#endregion 公開メソッド定義(GetList)

	#region 公開メソッド定義(GetData)
	/// <summary>
	/// 下位情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">下位名称</param>
	/// <param name="includeData">下位情報</param>
	/// <returns><paramref name="elementName" />が存在した場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が複数存在する場合</exception>
	public static bool GetData(this XmlNode elementData, string elementName, [MaybeNullWhen(false)]out XmlNode includeData) {
		var elementList = elementData.ChildNodes;
		includeData = null;
		for (var index = 0; index < elementList.Count; index ++) {
			var choose = elementList[index];
			if (choose != null && choose.Name == elementName) {
				if (includeData == null) {
					includeData = choose;
				} else {
					throw new XmlNodeException($"{elementName}タグが{elementData.Name}タグに複数存在します", elementData);
				}
			}
		}
		return includeData != null;
	}
	/// <summary>
	/// 下位情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">下位名称</param>
	/// <returns>下位情報</returns>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が複数存在する場合</exception>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が存在しない場合</exception>
	public static XmlNode GetData(this XmlNode elementData, string elementName) =>
		GetData(elementData, elementName, out var includeData)? includeData: throw new XmlNodeException($"{elementName}タグが{elementData.Name}タグに存在しません", elementData);
	/// <summary>
	/// 下位情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">下位名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>下位情報</returns>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が複数存在する場合</exception>
	[return: NotNullIfNotNull("defaultData")]
	public static XmlNode? GetData(this XmlNode elementData, string elementName, XmlNode? defaultData) =>
		GetData(elementData, elementName, out var includeData)? includeData: defaultData;
	#endregion 公開メソッド定義(GetData)

	#region 公開メソッド定義(GetString)
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="includeData">属性情報</param>
	/// <returns><paramref name="elementName" />が存在した場合、<c>True</c>を返却</returns>
	public static bool GetString(this XmlNode elementData, string elementName, [MaybeNullWhen(false)]out string includeData) =>
		GetString(elementData.Attributes, elementName, out includeData);
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が存在しない場合</exception>
	public static string GetString(this XmlNode elementData, string elementName) =>
		GetString(elementData, elementName, out var includeData)? includeData: throw new XmlNodeException($"{elementName}属性が{elementData.Name}タグに存在しません", elementData);
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	[return: NotNullIfNotNull("defaultData")]
	public static string? GetString(this XmlNode elementData, string elementName, string defaultData) =>
		GetString(elementData, elementName, out var includeData)? includeData: defaultData;
	#endregion 公開メソッド定義(GetString)

	#region 公開メソッド定義(GetBoolean)
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="includeData">属性情報</param>
	/// <returns><paramref name="elementName" />が存在した場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static bool GetBoolean(this XmlNode elementData, string elementName, [MaybeNullWhen(false)]out bool includeData) {
		if (GetString(elementData, elementName, out var includeText) == false) {
			includeData = default;
			return false;
		} else if (Boolean.TryParse(includeText, out includeData)) {
			return true;
		} else {
			throw new XmlNodeException($"{elementData.Name}タグの{elementName}属性がバイト値ではありません", elementData, includeText);
		}
	}
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が存在しない場合</exception>
	public static bool GetBoolean(this XmlNode elementData, string elementName) =>
		GetBoolean(elementData, elementName, out var includeData)? includeData: throw new XmlNodeException($"{elementName}属性が{elementData.Name}タグに存在しません", elementData);
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static bool GetBoolean(this XmlNode elementData, string elementName, bool defaultData) =>
		GetBoolean(elementData, elementName, out var includeData)? includeData: defaultData;
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static bool? GetBoolean(this XmlNode elementData, string elementName, bool? defaultData) =>
		GetBoolean(elementData, elementName, out var includeData)? includeData: defaultData;
	#endregion 公開メソッド定義(GetBoolean)

	#region 公開メソッド定義(GetByte)
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="includeData">属性情報</param>
	/// <returns><paramref name="elementName" />が存在した場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static bool GetByte(this XmlNode elementData, string elementName, [MaybeNullWhen(false)]out byte includeData) {
		if (GetString(elementData, elementName, out var includeText) == false) {
			includeData = default;
			return false;
		} else if (Byte.TryParse(includeText, out includeData)) {
			return true;
		} else {
			throw new XmlNodeException($"{elementData.Name}タグの{elementName}属性がバイト値ではありません", elementData, includeText);
		}
	}
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が存在しない場合</exception>
	public static byte GetByte(this XmlNode elementData, string elementName) =>
		GetByte(elementData, elementName, out var includeData)? includeData: throw new XmlNodeException($"{elementName}属性が{elementData.Name}タグに存在しません", elementData);
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static byte GetByte(this XmlNode elementData, string elementName, byte defaultData) =>
		GetByte(elementData, elementName, out var includeData)? includeData: defaultData;
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static byte? GetByte(this XmlNode elementData, string elementName, byte? defaultData) =>
		GetByte(elementData, elementName, out var includeData)? includeData: defaultData;
	#endregion 公開メソッド定義(GetByte)

	#region 公開メソッド定義(GetInt16)
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="includeData">属性情報</param>
	/// <returns><paramref name="elementName" />が存在した場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static bool GetInt16(this XmlNode elementData, string elementName, [MaybeNullWhen(false)]out short includeData) {
		if (GetString(elementData, elementName, out var includeText) == false) {
			includeData = default;
			return false;
		} else if (Int16.TryParse(includeText, out includeData)) {
			return true;
		} else {
			throw new XmlNodeException($"{elementData.Name}タグの{elementName}属性が短整数値ではありません", elementData, includeText);
		}
	}
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が存在しない場合</exception>
	public static short GetInt16(this XmlNode elementData, string elementName) =>
		GetInt16(elementData, elementName, out var includeData)? includeData: throw new XmlNodeException($"{elementName}属性が{elementData.Name}タグに存在しません", elementData);
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static short GetInt16(this XmlNode elementData, string elementName, short defaultData) =>
		GetInt16(elementData, elementName, out var includeData)? includeData: defaultData;
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static short? GetInt16(this XmlNode elementData, string elementName, short? defaultData) =>
		GetInt16(elementData, elementName, out var includeData)? includeData: defaultData;
	#endregion 公開メソッド定義(GetInt16)

	#region 公開メソッド定義(GetInt32)
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="includeData">属性情報</param>
	/// <returns><paramref name="elementName" />が存在した場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static bool GetInt32(this XmlNode elementData, string elementName, [MaybeNullWhen(false)]out int includeData) {
		if (GetString(elementData, elementName, out var includeText) == false) {
			includeData = default;
			return false;
		} else if (Int32.TryParse(includeText, out includeData)) {
			return true;
		} else {
			throw new XmlNodeException($"{elementData.Name}タグの{elementName}属性が整数値ではありません", elementData, includeText);
		}
	}
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が存在しない場合</exception>
	public static int GetInt32(this XmlNode elementData, string elementName) =>
		GetInt32(elementData, elementName, out var includeData)? includeData: throw new XmlNodeException($"{elementName}属性が{elementData.Name}タグに存在しません", elementData);
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static int GetInt32(this XmlNode elementData, string elementName, int defaultData) =>
		GetInt32(elementData, elementName, out var includeData)? includeData: defaultData;
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static int? GetInt32(this XmlNode elementData, string elementName, int? defaultData) =>
		GetInt32(elementData, elementName, out var includeData)? includeData: defaultData;
	#endregion 公開メソッド定義(GetInt32)

	#region 公開メソッド定義(GetInt64)
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="includeData">属性情報</param>
	/// <returns><paramref name="elementName" />が存在した場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static bool GetInt64(this XmlNode elementData, string elementName, [MaybeNullWhen(false)]out long includeData) {
		if (GetString(elementData, elementName, out var includeText) == false) {
			includeData = default;
			return false;
		} else if (Int64.TryParse(includeText, out includeData)) {
			return true;
		} else {
			throw new XmlNodeException($"{elementData.Name}タグの{elementName}属性が長整数値ではありません", elementData, includeText);
		}
	}
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が存在しない場合</exception>
	public static long GetInt64(this XmlNode elementData, string elementName) =>
		GetInt64(elementData, elementName, out var includeData)? includeData: throw new XmlNodeException($"{elementName}属性が{elementData.Name}タグに存在しません", elementData);
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static long GetInt64(this XmlNode elementData, string elementName, long defaultData) =>
		GetInt64(elementData, elementName, out var includeData)? includeData: defaultData;
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static long? GetInt64(this XmlNode elementData, string elementName, long? defaultData) =>
		GetInt64(elementData, elementName, out var includeData)? includeData: defaultData;
	#endregion 公開メソッド定義(GetInt64)

	#region 公開メソッド定義(GetTimeSpan)
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="includeData">属性情報</param>
	/// <returns><paramref name="elementName" />が存在した場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static bool GetTimeSpan(this XmlNode elementData, string elementName, [MaybeNullWhen(false)]out TimeSpan includeData) {
		if (GetString(elementData, elementName, out var includeText) == false) {
			includeData = default;
			return false;
		} else if (TimeSpan.TryParse(includeText, out includeData)) {
			return true;
		} else {
			throw new XmlNodeException($"{elementData.Name}タグの{elementName}属性が期間情報ではありません", elementData, includeText);
		}
	}
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が存在しない場合</exception>
	public static TimeSpan GetTimeSpan(this XmlNode elementData, string elementName) =>
		GetTimeSpan(elementData, elementName, out var includeData)? includeData: throw new XmlNodeException($"{elementName}属性が{elementData.Name}タグに存在しません", elementData);
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static TimeSpan GetTimeSpan(this XmlNode elementData, string elementName, TimeSpan defaultData) =>
		GetTimeSpan(elementData, elementName, out var includeData)? includeData: defaultData;
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static TimeSpan? GetTimeSpan(this XmlNode elementData, string elementName, TimeSpan? defaultData) =>
		GetTimeSpan(elementData, elementName, out var includeData)? includeData: defaultData;

	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="patternText">属性書式</param>
	/// <param name="includeData">属性情報</param>
	/// <returns><paramref name="elementName" />が存在した場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static bool GetTimeSpan(this XmlNode elementData, string elementName, string patternText, [MaybeNullWhen(false)]out TimeSpan includeData) {
		if (GetString(elementData, elementName, out var includeText) == false) {
			includeData = default;
			return false;
		} else if (TimeSpan.TryParseExact(includeText, patternText, null, out includeData)) {
			return true;
		} else {
			throw new XmlNodeException($"{elementData.Name}タグの{elementName}属性が期間情報ではありません", elementData, includeText);
		}
	}
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="patternText">属性書式</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が存在しない場合</exception>
	public static TimeSpan GetTimeSpan(this XmlNode elementData, string elementName, string patternText) =>
		GetTimeSpan(elementData, elementName, patternText, out var includeData)? includeData: throw new XmlNodeException($"{elementName}属性が{elementData.Name}タグに存在しません", elementData);
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="patternText">属性書式</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static TimeSpan GetTimeSpan(this XmlNode elementData, string elementName, string patternText, TimeSpan defaultData) =>
		GetTimeSpan(elementData, elementName, patternText, out var includeData)? includeData: defaultData;
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="patternText">属性書式</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static TimeSpan? GetTimeSpan(this XmlNode elementData, string elementName, string patternText, TimeSpan? defaultData) =>
		GetTimeSpan(elementData, elementName, patternText, out var includeData)? includeData: defaultData;
	#endregion 公開メソッド定義(GetTimeSpan)

	#region 公開メソッド定義(GetDateTime)
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="includeData">属性情報</param>
	/// <returns><paramref name="elementName" />が存在した場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static bool GetDateTime(this XmlNode elementData, string elementName, [MaybeNullWhen(false)]out DateTime includeData) {
		if (GetString(elementData, elementName, out var includeText) == false) {
			includeData = default;
			return false;
		} else if (DateTime.TryParse(includeText, out includeData)) {
			return true;
		} else {
			throw new XmlNodeException($"{elementData.Name}タグの{elementName}属性が日時情報ではありません", elementData, includeText);
		}
	}
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が存在しない場合</exception>
	public static DateTime GetDateTime(this XmlNode elementData, string elementName) =>
		GetDateTime(elementData, elementName, out var includeData)? includeData: throw new XmlNodeException($"{elementName}属性が{elementData.Name}タグに存在しません", elementData);
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static DateTime GetDateTime(this XmlNode elementData, string elementName, DateTime defaultData) =>
		GetDateTime(elementData, elementName, out var includeData)? includeData: defaultData;
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static DateTime? GetDateTime(this XmlNode elementData, string elementName, DateTime? defaultData) =>
		GetDateTime(elementData, elementName, out var includeData)? includeData: defaultData;

	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="patternText">属性書式</param>
	/// <param name="includeData">属性情報</param>
	/// <returns><paramref name="elementName" />が存在した場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static bool GetDateTime(this XmlNode elementData, string elementName, string patternText, [MaybeNullWhen(false)]out DateTime includeData) {
		if (GetString(elementData, elementName, out var includeText) == false) {
			includeData = default;
			return false;
		} else if (DateTime.TryParseExact(includeText, patternText, null, DateTimeStyles.None, out includeData)) {
			return true;
		} else {
			throw new XmlNodeException($"{elementData.Name}タグの{elementName}属性が日時情報ではありません", elementData, includeText);
		}
	}
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="patternText">属性書式</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	/// <exception cref="XmlNodeException"><paramref name="elementName" />が存在しない場合</exception>
	public static DateTime GetDateTime(this XmlNode elementData, string elementName, string patternText) =>
		GetDateTime(elementData, elementName, patternText, out var includeData)? includeData: throw new XmlNodeException($"{elementName}属性が{elementData.Name}タグに存在しません", elementData);
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="patternText">属性書式</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static DateTime GetDateTime(this XmlNode elementData, string elementName, string patternText, DateTime defaultData) =>
		GetDateTime(elementData, elementName, patternText, out var includeData)? includeData: defaultData;
	/// <summary>
	/// 属性情報を取得します。
	/// </summary>
	/// <param name="elementData">要素情報</param>
	/// <param name="elementName">属性名称</param>
	/// <param name="patternText">属性書式</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>属性情報</returns>
	/// <exception cref="XmlNodeException">属性情報の形式が正しくない場合</exception>
	public static DateTime? GetDateTime(this XmlNode elementData, string elementName, string patternText, DateTime? defaultData) =>
		GetDateTime(elementData, elementName, patternText, out var includeData)? includeData: defaultData;
	#endregion 公開メソッド定義(GetDateTime)
}
