using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using Otchitta.Libraries.Common.Xml;

namespace Otchitta.Example.Demo01.Entity;

/// <summary>
/// <see cref="IDbCommand" />拡張関数クラスです。
/// </summary>
public static class CommandExtension {
	#region メンバー変数定義
	/// <summary>
	/// 構文一覧
	/// </summary>
	private static readonly Dictionary<string, string> commandList = new Dictionary<string, string>();
	#endregion メンバー変数定義

	#region 公開メソッド定義(SetCommandText)
	/// <summary>
	/// 実行構文を設定します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <param name="setting">設定名称</param>
	public static void SetCommandText(this IDbCommand command, string setting) {
		if (commandList.TryGetValue(setting, out var result)) {
			command.CommandText = result;
			command.Parameters.Clear();
		} else {
			throw new SystemException($"{setting}が設定されていません");
		}
	}
	#endregion 公開メソッド定義(SetCommandText)

	#region 公開メソッド定義(SetCommandText/GetCommandList/SetComandFile/SetCommandPath)
	/// <summary>
	/// 実行構文を設定します。
	/// </summary>
	/// <param name="setting">設定名称</param>
	/// <param name="command">実行構文</param>
	private static void SetCommandText(string setting, string command) {
		if (String.IsNullOrWhiteSpace(setting)) {
			throw new SystemException("設定名称を指定してください");
		} else if (String.IsNullOrWhiteSpace(command)) {
			throw new SystemException("実行構文を指定してください");
		} else {
			commandList[setting] = command;
		}
	}
	/// <summary>
	/// 実行構文を生成します。
	/// </summary>
	/// <param name="configData">設定情報</param>
	/// <param name="searchName">接続名称</param>
	/// <param name="resultList">構文一覧</param>
	/// <returns>設定情報が対象である場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException">設定ファイルの構成が正しくない場合</exception>
	private static bool GetCommandList(XmlNode configData, string searchName, [MaybeNullWhen(false)]out Dictionary<string, string> resultList) {
		if (configData.Name != "commands") {
			resultList = default;
			return false;
		} else if (configData.GetString("type", out var sourceName) == false) {
			resultList = default;
			return false;
		} else {
			resultList = new Dictionary<string, string>();
			foreach (var chooseData in configData.GetList("command")) {
				var commandName = chooseData.GetString("name");
				var commandText = chooseData.InnerText;
				resultList[commandName] = commandText;
			}
			return true;
		}
	}
	/// <summary>
	/// 実行構文をファイルより設定します。
	/// </summary>
	/// <param name="configFile">ファイル</param>
	/// <param name="searchName">接続名称</param>
	/// <returns><paramref name="configFile" />が対象である場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException">設定ファイルの構成が正しくない場合</exception>
	private static bool SetCommandFile(string configFile, string searchName) {
		var configData = XmlNodeUtilities.Create(configFile);
		if  (GetCommandList(configData, searchName, out var resultList)) {
			foreach (var (commandName, commandText) in resultList) {
				SetCommandText(commandName, commandText);
			}
			return true;
		} else {
			return false;
		}
	}
	/// <summary>
	/// 実行構文を指定パスより設定します。
	/// </summary>
	/// <param name="configPath">設定パス</param>
	/// <param name="searchName">接続名称</param>
	/// <returns>設定を行った場合、<c>True</c>を返却</returns>
	/// <exception cref="XmlNodeException">設定ファイルの構成が正しくない場合</exception>
	public static bool SetCommandPath(string configPath, string searchName) {
		foreach (var configFile in Directory.GetFiles(configPath, "Report.*.xml")) {
			if (SetCommandFile(configFile, searchName)) {
				return true;
			}
		}
		return false;
	}
	#endregion 公開メソッド定義(SetCommandText/GetCommandList/SetComandFile/SetCommandPath)
}
