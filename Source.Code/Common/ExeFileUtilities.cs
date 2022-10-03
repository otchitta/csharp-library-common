using System;
using System.IO;
using System.Reflection;

namespace Otchitta.Libraries.Common;

/// <summary>
/// 実行共通関数クラスです。
/// </summary>
public static class ExeFileUtilities {
	#region メンバー変数定義
	/// <summary>
	/// 実行情報
	/// </summary>
	private static Assembly? executeData;
	/// <summary>
	/// 実行位置
	/// </summary>
	private static string? executePath;
	/// <summary>
	/// 実行名称
	/// </summary>
	private static string? executeName;
	#endregion メンバー変数定義

	#region プロパティー定義
	/// <summary>
	/// 実行情報を取得します。
	/// </summary>
	/// <returns>実行情報</returns>
	private static Assembly ExecuteData => executeData ??= Assembly.GetEntryAssembly() ?? throw new SystemException("entry assembly is not found.");
	/// <summary>
	/// 実行位置を取得します。
	/// </summary>
	/// <returns>実行位置</returns>
	public static string ExecutePath => executePath ??= Path.GetDirectoryName(ExecuteData.Location) ?? throw new SystemException("directory is not found.");
	/// <summary>
	/// 実行名称を取得します。
	/// </summary>
	/// <returns>実行名称</returns>
	public static string ExecuteName => executeName ??= Path.GetFileName(ExecuteData.Location);
	#endregion プロパティー定義

	/// <summary>
	/// 絶対位置を取得します。
	/// </summary>
	/// <param name="relativePath">相対位置</param>
	/// <returns>絶対位置</returns>
	public static string GetAbsolutePath(string relativePath) => Path.GetFullPath(Path.Combine(ExecutePath, relativePath));
	/// <summary>
	/// 実行名称を取得します。
	/// </summary>
	/// <param name="withoutExtension">拡張子付与</param>
	/// <returns>実行名称</returns>
	public static string GetExecuteName(bool withoutExtension) => withoutExtension ? Path.GetFileNameWithoutExtension(ExecuteName) : ExecuteName;
}
