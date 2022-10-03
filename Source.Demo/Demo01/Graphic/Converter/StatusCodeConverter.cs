using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Otchitta.Example.Demo01.Constant;

namespace Otchitta.Example.Demo01.Converter;

/// <summary>
/// 状態種別変換処理クラスです。
/// <para>状態種別を状態名称へ変換します。</para>
/// </summary>
public sealed class StatusCodeConverter : IValueConverter {
	/// <summary>
	/// 状態名称へ変換します。
	/// </summary>
	/// <param name="value">要素情報</param>
	/// <param name="targetType">対象種別</param>
	/// <param name="parameter">引数情報</param>
	/// <param name="culture">地域情報</param>
	/// <returns>状態名称</returns>
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		if (value is byte cache1) {
			switch (cache1) {
				default: return $"不明{cache1:00}";
				case 00: return "未実行";
				case 01: return "実行中";
				case 02: return "成功";
				case 03: return "失敗";
				case 04: return "取消";
			}
		} else if (value is StatusType cache2) {
			switch (cache2) {
				default: return $"不明{cache2}";
				case StatusType.Nothing: return "未実行";
				case StatusType.Process: return "実行中";
				case StatusType.Success: return "成功";
				case StatusType.Failure: return "失敗";
				case StatusType.Suspend: return "取消";
			}
		} else {
			return DependencyProperty.UnsetValue;
		}
	}

	/// <summary>
	/// 状態種別へ変換します。
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
