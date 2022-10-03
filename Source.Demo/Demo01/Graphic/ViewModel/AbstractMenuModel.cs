using System;
using System.Windows.Input;

namespace Otchitta.Example.Demo01.ViewModel;

/// <summary>
/// 基底画面操作クラスです。
/// </summary>
public abstract class AbstractMenuModel : ICommand {
	/// <summary>
	/// 監視一覧
	/// </summary>
	private EventHandler? listenList;
	/// <summary>
	/// 実行可否変更監視処理を追加または削除します。
	/// </summary>
	event EventHandler? ICommand.CanExecuteChanged {
		add    => this.listenList += value;
		remove => this.listenList -= value;
	}

	/// <summary>
	/// 操作可否変更を通知します。
	/// </summary>
	protected virtual void Notify() =>
		this.listenList?.Invoke(this, EventArgs.Empty);
	/// <summary>
	/// 操作可否を判定します。
	/// </summary>
	/// <param name="parameter">引数情報</param>
	/// <returns>操作可能である場合、<c>True</c>を返却</returns>
	protected abstract bool Accept(object? parameter);
	/// <summary>
	/// 操作処理を実行します。
	/// </summary>
	/// <param name="parameter">引数情報</param>
	protected abstract void Invoke(object? parameter);

	/// <summary>
	/// 操作可否を判定します。
	/// </summary>
	/// <param name="parameter">引数情報</param>
	/// <returns>操作可能である場合、<c>True</c>を返却</returns>
	bool ICommand.CanExecute(object? parameter) => Accept(parameter);
	/// <summary>
	/// 操作処理を実行します。
	/// </summary>
	/// <param name="parameter">引数情報</param>
	void ICommand.Execute(object? parameter) => Invoke(parameter);
}
