using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Otchitta.Example.Demo01.Converter;

/// <summary>
/// 一行変換処理クラスです。
/// <para>状態種別を状態名称へ変換します。</para>
/// </summary>
public sealed class SingleLineConverter : IValueConverter {
	/// <summary>
	/// 一行へ変換します。
	/// </summary>
	/// <param name="value">要素情報</param>
	/// <param name="targetType">対象種別</param>
	/// <param name="parameter">引数情報</param>
	/// <param name="culture">地域情報</param>
	/// <returns>状態名称</returns>
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is string source) {
			var offset = source.IndexOfAny(new char[] { '\r', '\n' });
			if (offset < 0) {
				return source;
			} else {
				return source.Substring(0, offset);
			}
		} else {
			return DependencyProperty.UnsetValue;
		}
	}

	/// <summary>
	/// 複数行へ変換します。
	/// </summary>
	/// <param name="value">要素情報</param>
	/// <param name="targetType">対象種別</param>
	/// <param name="parameter">引数情報</param>
	/// <param name="culture">地域情報</param>
	/// <returns>状態種別</returns>
	/// <exception cref="InvalidOperationException">常に発生</exception>
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		throw new InvalidOperationException();
}
