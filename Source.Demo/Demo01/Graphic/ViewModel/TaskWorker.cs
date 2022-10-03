using System;

namespace Otchitta.Example.Demo01.ViewModel;

/// <summary>
/// タスク共通関数クラスです。
/// </summary>
public static class TaskWorker {
	#region 情報メソッド定義
	/// <summary>
	/// 情報処理を実行します。
	/// </summary>
	/// <param name="action">情報処理</param>
	public static void InvokeData(Action action) {
		System.Threading.Thread thread = new System.Threading.Thread(action.Invoke);
		thread.IsBackground = true;
		thread.Start();
	}
	/// <summary>
	/// 情報処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	public static void InvokeData<P1>(Action<P1> action, P1 value1) {
		InvokeData(() => action(value1));
	}
	/// <summary>
	/// 情報処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	public static void InvokeData<P1, P2>(Action<P1, P2> action, P1 value1, P2 value2) {
		InvokeData(() => action(value1, value2));
	}
	/// <summary>
	/// 情報処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	public static void InvokeData<P1, P2, P3>(Action<P1, P2, P3> action, P1 value1, P2 value2, P3 value3) {
		InvokeData(() => action(value1, value2, value3));
	}
	/// <summary>
	/// 情報処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <typeparam name="P4">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	/// <param name="value4">引数情報</param>
	public static void InvokeData<P1, P2, P3, P4>(Action<P1, P2, P3, P4> action, P1 value1, P2 value2, P3 value3, P4 value4) {
		InvokeData(() => action(value1, value2, value3, value4));
	}
	/// <summary>
	/// 情報処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <typeparam name="P4">引数種別</typeparam>
	/// <typeparam name="P5">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	/// <param name="value4">引数情報</param>
	/// <param name="value5">引数情報</param>
	public static void InvokeData<P1, P2, P3, P4, P5>(Action<P1, P2, P3, P4, P5> action, P1 value1, P2 value2, P3 value3, P4 value4, P5 value5) {
		InvokeData(() => action(value1, value2, value3, value4, value5));
	}
	/// <summary>
	/// 情報処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <typeparam name="P4">引数種別</typeparam>
	/// <typeparam name="P5">引数種別</typeparam>
	/// <typeparam name="P6">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	/// <param name="value4">引数情報</param>
	/// <param name="value5">引数情報</param>
	/// <param name="value6">引数情報</param>
	public static void InvokeData<P1, P2, P3, P4, P5, P6>(Action<P1, P2, P3, P4, P5, P6> action, P1 value1, P2 value2, P3 value3, P4 value4, P5 value5, P6 value6) {
		InvokeData(() => action(value1, value2, value3, value4, value5, value6));
	}
	/// <summary>
	/// 情報処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <typeparam name="P4">引数種別</typeparam>
	/// <typeparam name="P5">引数種別</typeparam>
	/// <typeparam name="P6">引数種別</typeparam>
	/// <typeparam name="P7">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	/// <param name="value4">引数情報</param>
	/// <param name="value5">引数情報</param>
	/// <param name="value6">引数情報</param>
	/// <param name="value7">引数情報</param>
	public static void InvokeData<P1, P2, P3, P4, P5, P6, P7>(Action<P1, P2, P3, P4, P5, P6, P7> action, P1 value1, P2 value2, P3 value3, P4 value4, P5 value5, P6 value6, P7 value7) {
		InvokeData(() => action(value1, value2, value3, value4, value5, value6, value7));
	}
	/// <summary>
	/// 情報処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <typeparam name="P4">引数種別</typeparam>
	/// <typeparam name="P5">引数種別</typeparam>
	/// <typeparam name="P6">引数種別</typeparam>
	/// <typeparam name="P7">引数種別</typeparam>
	/// <typeparam name="P8">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	/// <param name="value4">引数情報</param>
	/// <param name="value5">引数情報</param>
	/// <param name="value6">引数情報</param>
	/// <param name="value7">引数情報</param>
	/// <param name="value8">引数情報</param>
	public static void InvokeData<P1, P2, P3, P4, P5, P6, P7, P8>(Action<P1, P2, P3, P4, P5, P6, P7, P8> action, P1 value1, P2 value2, P3 value3, P4 value4, P5 value5, P6 value6, P7 value7, P8 value8) {
		InvokeData(() => action(value1, value2, value3, value4, value5, value6, value7, value8));
	}
	/// <summary>
	/// 情報処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <typeparam name="P4">引数種別</typeparam>
	/// <typeparam name="P5">引数種別</typeparam>
	/// <typeparam name="P6">引数種別</typeparam>
	/// <typeparam name="P7">引数種別</typeparam>
	/// <typeparam name="P8">引数種別</typeparam>
	/// <typeparam name="P9">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	/// <param name="value4">引数情報</param>
	/// <param name="value5">引数情報</param>
	/// <param name="value6">引数情報</param>
	/// <param name="value7">引数情報</param>
	/// <param name="value8">引数情報</param>
	/// <param name="value9">引数情報</param>
	public static void InvokeData<P1, P2, P3, P4, P5, P6, P7, P8, P9>(Action<P1, P2, P3, P4, P5, P6, P7, P8, P9> action, P1 value1, P2 value2, P3 value3, P4 value4, P5 value5, P6 value6, P7 value7, P8 value8, P9 value9) {
		InvokeData(() => action(value1, value2, value3, value4, value5, value6, value7, value8, value9));
	}
	#endregion 情報メソッド定義

	#region 画面メソッド定義
	/// <summary>
	/// 画面処理を終了します。
	/// </summary>
	public static void FinishView() {
		System.Windows.Application.Current.Shutdown();
	}
	/// <summary>
	/// 画面処理を実行します。
	/// </summary>
	/// <param name="action">画面処理</param>
	public static void InvokeView(Action action) {
		System.Windows.Application.Current.Dispatcher.BeginInvoke(action);
	}
	/// <summary>
	/// 画面処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	public static void InvokeView<P1>(Action<P1> action, P1 value1) {
		InvokeView(() => action(value1));
	}
	/// <summary>
	/// 画面処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	public static void InvokeView<P1, P2>(Action<P1, P2> action, P1 value1, P2 value2) {
		InvokeView(() => action(value1, value2));
	}
	/// <summary>
	/// 画面処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	public static void InvokeView<P1, P2, P3>(Action<P1, P2, P3> action, P1 value1, P2 value2, P3 value3) {
		InvokeView(() => action(value1, value2, value3));
	}
	/// <summary>
	/// 画面処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <typeparam name="P4">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	/// <param name="value4">引数情報</param>
	public static void InvokeView<P1, P2, P3, P4>(Action<P1, P2, P3, P4> action, P1 value1, P2 value2, P3 value3, P4 value4) {
		InvokeView(() => action(value1, value2, value3, value4));
	}
	/// <summary>
	/// 画面処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <typeparam name="P4">引数種別</typeparam>
	/// <typeparam name="P5">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	/// <param name="value4">引数情報</param>
	/// <param name="value5">引数情報</param>
	public static void InvokeView<P1, P2, P3, P4, P5>(Action<P1, P2, P3, P4, P5> action, P1 value1, P2 value2, P3 value3, P4 value4, P5 value5) {
		InvokeView(() => action(value1, value2, value3, value4, value5));
	}
	/// <summary>
	/// 画面処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <typeparam name="P4">引数種別</typeparam>
	/// <typeparam name="P5">引数種別</typeparam>
	/// <typeparam name="P6">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	/// <param name="value4">引数情報</param>
	/// <param name="value5">引数情報</param>
	/// <param name="value6">引数情報</param>
	public static void InvokeView<P1, P2, P3, P4, P5, P6>(Action<P1, P2, P3, P4, P5, P6> action, P1 value1, P2 value2, P3 value3, P4 value4, P5 value5, P6 value6) {
		InvokeView(() => action(value1, value2, value3, value4, value5, value6));
	}
	/// <summary>
	/// 画面処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <typeparam name="P4">引数種別</typeparam>
	/// <typeparam name="P5">引数種別</typeparam>
	/// <typeparam name="P6">引数種別</typeparam>
	/// <typeparam name="P7">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	/// <param name="value4">引数情報</param>
	/// <param name="value5">引数情報</param>
	/// <param name="value6">引数情報</param>
	/// <param name="value7">引数情報</param>
	public static void InvokeView<P1, P2, P3, P4, P5, P6, P7>(Action<P1, P2, P3, P4, P5, P6, P7> action, P1 value1, P2 value2, P3 value3, P4 value4, P5 value5, P6 value6, P7 value7) {
		InvokeView(() => action(value1, value2, value3, value4, value5, value6, value7));
	}
	/// <summary>
	/// 画面処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <typeparam name="P4">引数種別</typeparam>
	/// <typeparam name="P5">引数種別</typeparam>
	/// <typeparam name="P6">引数種別</typeparam>
	/// <typeparam name="P7">引数種別</typeparam>
	/// <typeparam name="P8">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	/// <param name="value4">引数情報</param>
	/// <param name="value5">引数情報</param>
	/// <param name="value6">引数情報</param>
	/// <param name="value7">引数情報</param>
	/// <param name="value8">引数情報</param>
	public static void InvokeView<P1, P2, P3, P4, P5, P6, P7, P8>(Action<P1, P2, P3, P4, P5, P6, P7, P8> action, P1 value1, P2 value2, P3 value3, P4 value4, P5 value5, P6 value6, P7 value7, P8 value8) {
		InvokeView(() => action(value1, value2, value3, value4, value5, value6, value7, value8));
	}
	/// <summary>
	/// 画面処理を実行します。
	/// </summary>
	/// <typeparam name="P1">引数種別</typeparam>
	/// <typeparam name="P2">引数種別</typeparam>
	/// <typeparam name="P3">引数種別</typeparam>
	/// <typeparam name="P4">引数種別</typeparam>
	/// <typeparam name="P5">引数種別</typeparam>
	/// <typeparam name="P6">引数種別</typeparam>
	/// <typeparam name="P7">引数種別</typeparam>
	/// <typeparam name="P8">引数種別</typeparam>
	/// <typeparam name="P9">引数種別</typeparam>
	/// <param name="action">実行処理</param>
	/// <param name="value1">引数情報</param>
	/// <param name="value2">引数情報</param>
	/// <param name="value3">引数情報</param>
	/// <param name="value4">引数情報</param>
	/// <param name="value5">引数情報</param>
	/// <param name="value6">引数情報</param>
	/// <param name="value7">引数情報</param>
	/// <param name="value8">引数情報</param>
	/// <param name="value9">引数情報</param>
	public static void InvokeView<P1, P2, P3, P4, P5, P6, P7, P8, P9>(Action<P1, P2, P3, P4, P5, P6, P7, P8, P9> action, P1 value1, P2 value2, P3 value3, P4 value4, P5 value5, P6 value6, P7 value7, P8 value8, P9 value9) {
		InvokeView(() => action(value1, value2, value3, value4, value5, value6, value7, value8, value9));
	}
	#endregion 画面メソッド定義
}
