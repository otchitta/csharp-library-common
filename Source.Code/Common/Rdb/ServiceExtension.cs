using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Otchitta.Libraries.Common.Rdb;

/// <summary>
/// 処理拡張関数クラスです。
/// </summary>
public static class ServiceExtension {
	#region IDbCommand関連
	#region 公開メソッド定義(AddParam～)
	/// <summary>
	/// 引数情報を設定します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <param name="name">引数名称</param>
	/// <param name="type">引数種別</param>
	/// <param name="data">引数情報</param>
	private static void AddParamObject(IDbCommand command, string name, DbType type, object? data) {
		var parameter = command.CreateParameter();
		parameter.ParameterName = name;
		parameter.DbType = type;
		parameter.Value = data ?? DBNull.Value;
		command.Parameters.Add(parameter);
	}
	/// <summary>
	/// 引数情報を設定します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <param name="name">引数名称</param>
	/// <param name="data">引数情報</param>
	public static void AddParamString(this IDbCommand command, string name, string? data) =>
		AddParamObject(command, name, DbType.String, data);
	/// <summary>
	/// 引数情報を設定します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <param name="name">引数名称</param>
	/// <param name="data">引数情報</param>
	public static void AddParamBoolean(this IDbCommand command, string name, bool? data) =>
		AddParamObject(command, name, DbType.Boolean, data);
	/// <summary>
	/// 引数情報を設定します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <param name="name">引数名称</param>
	/// <param name="data">引数情報</param>
	public static void AddParamByte(this IDbCommand command, string name, byte? data) =>
		AddParamObject(command, name, DbType.Byte, data);
	/// <summary>
	/// 引数情報を設定します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <param name="name">引数名称</param>
	/// <param name="data">引数情報</param>
	public static void AddParamInt32(this IDbCommand command, string name, int? data) =>
		AddParamObject(command, name, DbType.Int32, data);
	/// <summary>
	/// 引数情報を設定します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <param name="name">引数名称</param>
	/// <param name="data">引数情報</param>
	public static void AddParamInt64(this IDbCommand command, string name, long? data) =>
		AddParamObject(command, name, DbType.Int64, data);
	/// <summary>
	/// 引数情報を設定します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <param name="name">引数名称</param>
	/// <param name="data">引数情報</param>
	public static void AddParamDateTime(this IDbCommand command, string name, DateTime? data) =>
		AddParamObject(command, name, DbType.DateTime2, data);
	/// <summary>
	/// 引数情報を設定します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <param name="name">引数名称</param>
	/// <param name="data">引数情報</param>
	public static void AddParamObject(this IDbCommand command, string name, object? data) {
		var parameter = command.CreateParameter();
		parameter.ParameterName = name;
		parameter.Value = data ?? DBNull.Value;
		command.Parameters.Add(parameter);
	}
	#endregion 公開メソッド定義(AddParam～)

	#region 内部メソッド定義(CreateText/ExecuteReader/ExecuteNonQuery)
	/// <summary>
	/// 例外内容を生成します。
	/// </summary>
	/// <param name="process">処理名称</param>
	/// <param name="command">実行処理</param>
	/// <returns>例外内容</returns>
	private static string CreateText(string process, IDbCommand command) {
		var result = new System.Text.StringBuilder();
		result.AppendLine($"{process}にて例外が発生しました。");
		result.Append($"実行構文:{command.CommandText}");
		var parameters = command.Parameters;
		for (var index = 0; index < parameters.Count; index ++) {
			var parameter = parameters[index];
			result.AppendLine();
			if (parameter is IDbDataParameter value1) {
				result.Append($"引数{index + 1:0000}:{value1.ParameterName}={value1.Value}");
			} else {
				result.Append($"引数{index + 1:0000}:{parameter}");
			}
		}
		return result.ToString();
	}
	/// <summary>
	/// 抽出処理を実行します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <returns>読込処理</returns>
	/// <exception cref="CommandException">抽出処理に失敗した場合</exception>
	private static IDataReader ExecuteReader(IDbCommand command) {
		try {
			return command.ExecuteReader();
		} catch (Exception error) {
			throw new CommandException(CreateText("抽出処理", command), error);
		}
	}
	/// <summary>
	/// 実行処理を実行します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <returns>処理件数</returns>
	/// <exception cref="CommandException">実行処理に失敗した場合</exception>
	private static int ExecuteNonQuery(IDbCommand command) {
		try {
			return command.ExecuteNonQuery();
		} catch (Exception error) {
			throw new CommandException(CreateText("実行処理", command), error);
		}
	}
	#endregion 内部メソッド定義(CreateText/ExecuteReader/ExecuteNonQuery)

	#region 公開メソッド定義(SelectData)
	/// <summary>
	/// 要素情報を抽出します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <param name="converter">変換処理</param>
	/// <param name="result">要素情報</param>
	/// <typeparam name="TResult">要素種別</typeparam>
	/// <returns>要素情報が存在した場合、<c>True</c>を返却</returns>
	/// <exception cref="CommandException">抽出処理に失敗した場合</exception>
	public static bool SelectData<TResult>(this IDbCommand command, Func<IDataRecord, TResult> converter, [MaybeNullWhen(false)]out TResult result) {
		using (var reader = ExecuteReader(command)) {
			if (reader.Read()) {
				result = converter(reader);
				return true;
			} else {
				result = default;
				return false;
			}
		}
	}
	/// <summary>
	/// 要素情報を抽出します。
	/// <para>要素情報が存在しない場合、<c>default</c>(クラスであれば<c>null</c>)を返却します。</para>
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <param name="converter">変換処理</param>
	/// <typeparam name="TResult">要素種別</typeparam>
	/// <returns>要素情報</returns>
	/// <exception cref="CommandException">抽出処理に失敗した場合</exception>
	public static TResult? SelectData<TResult>(this IDbCommand command, Func<IDataRecord, TResult> converter) =>
		SelectData(command, converter, out var result)? result: default;
	#endregion 公開メソッド定義(SelectData)

	#region 公開メソッド定義(SelectList)
	/// <summary>
	/// 要素一覧を抽出します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <param name="converter">変換処理</param>
	/// <typeparam name="TResult">要素種別</typeparam>
	/// <returns>要素一覧</returns>
	/// <exception cref="CommandException">抽出処理に失敗した場合</exception>
	public static List<TResult> SelectList<TResult>(this IDbCommand command, Func<IDataRecord, TResult> converter) {
		using (var reader = ExecuteReader(command)) {
			var result = new List<TResult>();
			while (reader.Read()) {
				result.Add(converter(reader));
			}
			return result;
		}
	}
	#endregion 公開メソッド定義(SelectList)

	#region 公開メソッド定義(InvokeData)
	/// <summary>
	/// 実行処理を実行します。
	/// </summary>
	/// <param name="command">実行処理</param>
	/// <returns>処理件数</returns>
	/// <exception cref="CommandException">実行処理に失敗した場合</exception>
	public static int InvokeData(this IDbCommand command) => ExecuteNonQuery(command);
	#endregion 公開メソッド定義(InvokeData)
	#endregion IDbCommand関連

	#region IDataRecord関連
	#region 公開メソッド定義(Get～)
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	[return: NotNullIfNotNull("defaultData")]
	public static string? GetString(this IDataRecord record, int ordinal, string? defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetString(ordinal);

	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static char GetChar(this IDataRecord record, int ordinal, char defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetChar(ordinal);
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static char? GetChar(this IDataRecord record, int ordinal, char? defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetChar(ordinal);

	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static bool GetBoolean(this IDataRecord record, int ordinal, bool defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetBoolean(ordinal);
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static bool? GetBoolean(this IDataRecord record, int ordinal, bool? defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetBoolean(ordinal);

	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static byte GetByte(this IDataRecord record, int ordinal, byte defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetByte(ordinal);
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static byte? GetByte(this IDataRecord record, int ordinal, byte? defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetByte(ordinal);

	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static short GetInt16(this IDataRecord record, int ordinal, short defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetInt16(ordinal);
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static short? GetInt16(this IDataRecord record, int ordinal, short? defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetInt16(ordinal);

	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static int GetInt32(this IDataRecord record, int ordinal, int defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetInt32(ordinal);
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static int? GetInt32(this IDataRecord record, int ordinal, int? defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetInt32(ordinal);

	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static long GetInt64(this IDataRecord record, int ordinal, long defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetInt64(ordinal);
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static long? GetInt64(this IDataRecord record, int ordinal, long? defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetInt64(ordinal);

	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static decimal GetDecimal(this IDataRecord record, int ordinal, decimal defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetDecimal(ordinal);
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static decimal? GetDecimal(this IDataRecord record, int ordinal, decimal? defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetDecimal(ordinal);

	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static float GetFloat(this IDataRecord record, int ordinal, float defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetFloat(ordinal);
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static float? GetFloat(this IDataRecord record, int ordinal, float? defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetFloat(ordinal);

	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static double GetDouble(this IDataRecord record, int ordinal, double defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetDouble(ordinal);
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static double? GetDouble(this IDataRecord record, int ordinal, double? defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetDouble(ordinal);

	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static DateTime GetDateTime(this IDataRecord record, int ordinal, DateTime defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetDateTime(ordinal);
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <param name="ordinal">要素番号</param>
	/// <param name="defaultData">既定情報</param>
	/// <returns>要素情報</returns>
	public static DateTime? GetDateTime(this IDataRecord record, int ordinal, DateTime? defaultData) =>
		record.IsDBNull(ordinal)? defaultData: record.GetDateTime(ordinal);
	#endregion 公開メソッド定義(Get～)
	#endregion IDataRecord関連
}
