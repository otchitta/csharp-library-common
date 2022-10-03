using System;

namespace Otchitta.Example.Demo01.ViewModel;

/// <summary>
/// <see cref="Delegate" />利用画面操作クラスです。
/// </summary>
public sealed class DelegateMenuModel : AbstractMenuModel {
	/// <summary>
	/// 操作判定
	/// </summary>
	private Predicate<object?> accept;
	/// <summary>
	/// 操作処理
	/// </summary>
	private Action<object?> invoke;

	/// <summary>
	/// <see cref="Delegate" />利用画面操作を生成します。
	/// </summary>
	/// <param name="invoke">操作処理</param>
	/// <param name="accept">操作判定</param>
	public DelegateMenuModel(Action<object?> invoke, Predicate<object?> accept) {
		this.invoke = invoke;
		this.accept = accept;
	}
	/// <summary>
	/// <see cref="Delegate" />利用画面操作を生成します。
	/// </summary>
	/// <param name="invoke">操作処理</param>
	/// <param name="accept">操作判定</param>
	public DelegateMenuModel(Action invoke, Func<bool> accept) {
		this.invoke = parameter => invoke();
		this.accept = parameter => accept();
	}
	/// <summary>
	/// <see cref="Delegate" />利用画面操作を生成します。
	/// </summary>
	/// <param name="invoke">操作処理</param>
	public DelegateMenuModel(Action invoke) {
		this.invoke = parameter => invoke();
		this.accept = parameter => true;
	}

	/// <summary>
	/// 操作可否変更を通知します。
	/// </summary>
	public new void Notify() => base.Notify();

	/// <summary>
	/// 操作可否を判定します。
	/// </summary>
	/// <param name="parameter">引数情報</param>
	/// <returns>操作可能である場合、<c>True</c>を返却</returns>
	protected override bool Accept(object? parameter) => this.accept(parameter);
	/// <summary>
	/// 操作処理を実行します。
	/// </summary>
	/// <param name="parameter">引数情報</param>
	protected override void Invoke(object? parameter) => this.invoke(parameter);
}
