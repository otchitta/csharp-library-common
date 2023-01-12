using System;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;

namespace Otchitta.Libraries.Common.Rdb;

/// <summary>
/// SQLiteの実装検証クラスです。
/// </summary>
public class SQLiteSupportTest {
	/// <summary>接続名称</summary>
	private const string Connector = "System.Data.SQLite";
	/// <summary>接続引数</summary>
	private const string Parameter = "Data Source=:MEMORY:";

	/// <summary>
	/// 事前処理を実行します。
	/// </summary>
	[OneTimeSetUp]
	public void OneTimeSetup() =>
		System.Data.Common.DbProviderFactories.RegisterFactory(Connector, System.Data.SQLite.SQLiteFactory.Instance);
	/// <summary>
	/// 事後処理を実行します。
	/// </summary>
	[OneTimeTearDown]
	public void OneTimeTearDown() =>
		System.Data.Common.DbProviderFactories.UnregisterFactory(Connector);

	/// <summary>
	/// テーブル情報を作成します。
	/// </summary>
	/// <param name="tableName">表名称</param>
	/// <param name="fieldType">列種別</param>
	/// <param name="command">実行処理</param>
	private static void CreateTable(IDbCommand command, string tableName, string fieldType) {
		command.CommandText = $"CREATE TABLE {tableName}(code INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, data {fieldType})";
		command.Parameters.Clear();
		command.ExecuteNonQuery();
	}

	private static void Execute(Action<IDbCommand> action, string tableName, string fieldType) {
		using (var connection = ConnectUtilities.Create(Connector, Parameter))
		using (var command = connection.CreateCommand()) {
			CreateTable(command, tableName, fieldType);
			action(command);
		}
	}

	/// <summary>
	/// 日時情報を抽出します。
	/// </summary>
	/// <param name="record">読込情報</param>
	/// <returns>日時情報</returns>
	private static (int, DateTime) ChooseDateTimeData(IDataRecord record) =>
		(record.GetInt32(0), record.GetDateTime(1));
	/// <summary>
	/// 日時一覧を抽出します。
	/// </summary>
	/// <param name="command">実行処理</param>
	private static List<(int, DateTime)> ChooseDateTimeList(IDbCommand command) =>
		command.SelectList(ChooseDateTimeData);

	private static void TestDateTime(IDbCommand command) {
		var source = new DateTime(2000, 1, 2, 3, 4, 5).Ticks;
		var value0 = new DateTime(source + 1);
	}
	/// <summary>
	/// 日時情報を検証します。
	/// </summary>
	[Test]
	public void TestDateTime() =>
		Execute(TestDateTime, "unit_test", "DATETIME");
}
