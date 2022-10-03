using System;
using System.ComponentModel;

namespace Otchitta.Example.Demo01.ViewModel;

/// <summary>
/// 基底画面情報クラスです。
/// </summary>
public abstract class AbstractViewModel : INotifyPropertyChanged {
	/// <summary>
	/// 監視処理を追加または設定します。
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// 要素変更を通知します。
	/// </summary>
	/// <param name="propertyName">要素名称</param>
	protected virtual void RaisePropertyChanged(string? propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	/// <summary>
	/// 要素情報を設定します。
	/// </summary>
	/// <param name="sourceData">要素情報</param>
	/// <param name="updateData">更新情報</param>
	/// <param name="propertyName">要素名称</param>
	/// <param name="updateHook">更新処理</param>
	/// <typeparam name="TValue">要素種別</typeparam>
	/// <returns>要素情報を更新した場合、<c>True</c>を返却</returns>
	protected virtual bool SetValue<TValue>(ref TValue sourceData, TValue updateData, string propertyName, Action? updateHook = null) {
		if (Equals(sourceData, updateData)) {
			return false;
		} else {
			sourceData = updateData;
			updateHook?.Invoke();
			RaisePropertyChanged(propertyName);
			return true;
		}
	}
}
