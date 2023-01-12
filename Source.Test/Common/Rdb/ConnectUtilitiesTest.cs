using System;
using System.Data;
using System.Data.Common;
using NUnit.Framework;
using static NUnit.Framework.Assert;
using SuccessFactory = System.Data.SQLite.SQLiteFactory;

namespace Otchitta.Libraries.Common.Rdb;

/// <summary>
/// <see cref="ConnectUtilities" />検証クラスです。
/// <para>成功処処理はSQLiteを代用する</para>
/// </summary>
public static class ConnectUtilitiesTest {
	/// <summary>
	/// 成功接続名称
	/// </summary>
	private const string SuccessConnector = "System.Data.SQLite";
	/// <summary>
	/// 失敗接続名称
	/// </summary>
	private const string FailureConnector = "Otchitta";
	/// <summary>
	/// 成功接続名称
	/// </summary>
	private const string SuccessParameter = "Data Source=:MEMORY:";
	/// <summary>
	/// 失敗接続名称
	/// </summary>
	private const string FailureParameter = "Data Source=::";

	/// <summary>
	/// <see cref="ConnectUtilities.Create(string, string)" />検証クラスです。
	/// </summary>
	public class TestCreate {
		#region メンバー変数定義
		/// <summary>生成処理</summary>
		private DbProviderFactory processor = new MockFactory();
		/// <summary>接続名称</summary>
		private string connector = SuccessConnector;
		/// <summary>接続引数</summary>
		private string parameter = SuccessParameter;
		#endregion メンバー変数定義

		#region 内部メソッド定義
		/// <summary>
		/// <see cref="ConnectUtilities.Create(string, string)" />を実行します。
		/// </summary>
		private void Execute01() {
			DbProviderFactories.RegisterFactory(SuccessConnector, processor);
			try {
				using (var connection = ConnectUtilities.Create(this.connector, this.parameter)) {
					Pass("データベース接続成功");
				}
			} finally {
				DbProviderFactories.UnregisterFactory(SuccessConnector);
			}
		}
		/// <summary>
		/// 失敗処理を実行します。
		/// </summary>
		/// <param name="processor">生成処理</param>
		/// <param name="connector">接続名称</param>
		/// <param name="parameter">接続引数</param>
		/// <param name="expect1">想定情報：例外文言の先頭部分</param>
		/// <param name="expect2">想定情報：接続引数の省略種別(<c>True</c>:省略)</param>
		/// <remarks>
		/// 1.例外のInnerExceptionは検証しない(ライブラリによって異なる為、当該処理でしても意味をなさない)
		/// </remarks>
		private void Failure01(DbProviderFactory processor, string connector, string parameter, string expect1, bool expect2) {
			this.processor = processor;
			this.connector = connector;
			this.parameter = parameter;
			var actual = Throws<ConnectException>(Execute01);
			AreEqual(connector, actual?.Connector);
			if (expect2) {
				var message = $"{expect1}{Environment.NewLine}接続名称:{connector}";
				AreEqual(message, actual?.Message);
				IsNull(actual?.Parameter);
			} else {
				var message = $"{expect1}{Environment.NewLine}接続名称:{connector}{Environment.NewLine}接続引数:{parameter}";
				AreEqual(message, actual?.Message);
				AreEqual(parameter, actual?.Parameter);
			}
		}
		/// <summary>
		/// 失敗処理を実行します。
		/// </summary>
		/// <param name="connector">接続名称</param>
		/// <param name="parameter">接続引数</param>
		/// <param name="expect1">想定情報：例外文言の先頭部分</param>
		/// <param name="expect2">想定情報：接続引数の省略種別(True:省略)</param>
		private void Failure01(string connector, string parameter, string expect1, bool expect2) =>
			Failure01(SuccessFactory.Instance, connector, parameter, expect1, expect2);
		#endregion 内部メソッド定義

		#region 公開メソッド定義
		/// <summary>
		/// 成功処理を検証します。
		/// </summary>
		[Test]
		public void Success01() {
			this.processor = SuccessFactory.Instance;
			this.connector = SuccessConnector;
			this.parameter = SuccessParameter;
			Execute01();
		}
		/// <summary>
		/// 失敗処理を検証します。
		/// <para>例外文言として「データベース処理の特定に失敗しました」が発生するケースとします</para>
		/// </summary>
		[Test]
		public void Failure01() =>
			Failure01(SuccessFactory.Instance, FailureConnector, SuccessParameter, "データベース処理の特定に失敗しました", true);
		/// <summary>
		/// 失敗処理を検証します。
		/// <para>例外文言として「データベース処理の生成に失敗しました」が発生するケースとします</para>
		/// </summary>
		[Test]
		public void Failure02() =>
			Failure01(FailureFactory.Failure1, SuccessConnector, SuccessParameter, "データベース処理の生成に失敗しました", true);
		/// <summary>
		/// 失敗処理を検証します。
		/// <para>例外文言として「データベース引数の適用に失敗しました」が発生するケースとします</para>
		/// </summary>
		[Test]
		public void Failure03() =>
			Failure01(FailureFactory.Failure2, SuccessConnector, SuccessParameter, "データベース引数の適用に失敗しました", false);
		/// <summary>
		/// 失敗処理を検証します。
		/// <para>例外文言として「データベース処理の接続に失敗しました」が発生するケースとします</para>
		/// </summary>
		[Test]
		public void Failure04() =>
			Failure01(SuccessFactory.Instance, SuccessConnector, FailureParameter, "データベース処理の接続に失敗しました", false);
		#endregion 公開メソッド定義
	}

	#region 非公開クラス定義
	/// <summary>
	/// 失敗情報生成処理クラスです。
	/// </summary>
	private static class FailureFactory {
		/// <summary>
		/// 接続処理なしインスタンスを取得します。
		/// </summary>
		/// <returns>接続処理なしインスタンス</returns>
		public static DbProviderFactory Failure1 => new MockFactory();
		/// <summary>
		/// 適用例外インスタンスを取得します。
		/// </summary>
		/// <returns>適用例外インスタンス</returns>
		public static DbProviderFactory Failure2 => new MockFactory() {
			Connection = new ExceptionConnection() { SetConnectionStringException = true }
		};
	}

	/// <summary>
	/// モック生成処理クラスです。
	/// </summary>
	private class MockFactory : DbProviderFactory {
		/// <summary>接続処理を取得または設定します。</summary>
		/// <value>接続処理</value>
		public DbConnection? Connection {
			get;
			set;
		}
		/// <summary>
		/// 接続処理を生成します。
		/// </summary>
		/// <returns>接続処理</returns>
		public override DbConnection? CreateConnection() => Connection;
	}
	/// <summary>
	/// 例外接続処理クラスです。
	/// </summary>
	private class ExceptionConnection : DbConnection {
		#region プロパティー定義
		/// <summary>
		/// 接続文字列設定時の例外発行種別です。
		/// </summary>
		/// <value>接続文字列設定時の例外発行種別</value>
		public bool SetConnectionStringException {
			get;
			set;
		}
		/// <summary>
		/// 接続時の例外発行種別です。
		/// </summary>
		/// <value>接続時の例外発行種別</value>
		public bool OpenException {
			get;
			set;
		}
		#endregion プロパティー定義

		#region 継承メソッド定義
		#nullable disable
		/// <summary>
		/// 接続文字列を取得または設定します。
		/// </summary>
		/// <returns>接続文字列</returns>
		public override string ConnectionString {
			get { throw new NotImplementedException(); }
			set { if (SetConnectionStringException) throw new NotImplementedException(); }
		}
		#nullable restore
		/// <summary>
		/// データベース名を取得します。
		/// </summary>
		/// <returns>データベース名</returns>
		public override string Database => throw new NotImplementedException();
		/// <summary>
		/// データソースを取得します。
		/// </summary>
		/// <returns>データソース</returns>
		public override string DataSource => throw new NotImplementedException();
		/// <summary>
		/// サーバーバージョンを取得します。
		/// </summary>
		/// <returns>サーバーバージョン</returns>
		public override string ServerVersion => throw new NotImplementedException();
		/// <summary>
		/// 接続状態を取得します。
		/// </summary>
		/// <returns>接続状態</returns>
		public override ConnectionState State => throw new NotImplementedException();

		/// <summary>
		/// データベースを変更しhます。
		/// </summary>
		/// <param name="databaseName">データベース</param>
		public override void ChangeDatabase(string databaseName) => throw new NotImplementedException();
		/// <summary>
		/// 切断処理を実行します。
		/// </summary>
		public override void Close() => throw new NotImplementedException();
		/// <summary>
		/// 接続処理を実行します。
		/// </summary>
		public override void Open() => throw new NotImplementedException();
		/// <summary>
		/// トランザクションを開始します。
		/// </summary>
		/// <param name="isolationLevel">制約種別</param>
		/// <returns>トランザクション</returns>
		protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => throw new NotImplementedException();
		/// <summary>
		/// DBコマンドを生成します。
		/// </summary>
		/// <returns>DBコマンド</returns>
		protected override DbCommand CreateDbCommand() => throw new NotImplementedException();
		#endregion 継承メソッド定義
	}
	#endregion 非公開クラス定義
}
