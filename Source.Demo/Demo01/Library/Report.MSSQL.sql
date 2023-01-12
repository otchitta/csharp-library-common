-- ------------------------------------------------------------------
-- データベース作成
-- ------------------------------------------------------------------
USE master;
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'data_report')
BEGIN DROP DATABASE data_report
END
CREATE DATABASE data_report;
GO

-- ------------------------------------------------------------------
-- ログインユーザ作成
-- ------------------------------------------------------------------
IF EXISTS (SELECT name FROM sys.server_principals WHERE name = N'data_worker')
BEGIN DROP LOGIN data_worker
END
CREATE LOGIN data_worker WITH PASSWORD = 'xxx123XXX';
GO

-- ------------------------------------------------------------------
-- 権限付与
-- ------------------------------------------------------------------
USE data_report;
CREATE USER data_worker FOR LOGIN data_worker;
GO

EXEC sp_addrolemember 'db_datareader', 'data_worker';
EXEC sp_addrolemember 'db_datawriter', 'data_worker';
EXEC sp_addrolemember 'db_ddladmin',   'data_worker';
GO

-- ------------------------------------------------------------------
-- テーブル削除
-- ------------------------------------------------------------------
IF EXISTS (SELECT v.name FROM sys.schemas s INNER JOIN sys.views v ON v.schema_id = s.schema_id WHERE s.name = 'dbo' AND v.name = 'v_report_source')
BEGIN DROP VIEW v_report_source
END
IF EXISTS (SELECT t.name FROM sys.schemas s INNER JOIN sys.tables t ON t.schema_id = s.schema_id WHERE s.name = 'dbo' AND t.name = 't_report_invoke')
BEGIN DROP TABLE t_report_invoke
END
IF EXISTS (SELECT t.name FROM sys.schemas s INNER JOIN sys.tables t ON t.schema_id = s.schema_id WHERE s.name = 'dbo' AND t.name = 't_report_source')
BEGIN DROP TABLE t_report_source
END

-- ------------------------------------------------------------------
-- テーブル作成
-- ------------------------------------------------------------------
-- 基本情報テーブル
-- f_source_code : 基本番号
-- f_source_name : 基本名称
-- f_source_text : 基本内容
-- f_status_code : 状態種別(1:実行 2:成功 3:失敗 4:取消)
-- f_invoke_time : 開始日時
-- f_finish_time : 終了日時
-- f_result_text : 結果内容
-- f_insert_name : 登録名称
-- f_insert_time : 登録日時
-- f_update_name : 更新名称
-- f_update_time : 更新日時
CREATE TABLE t_report_source (
    f_source_code INTEGER        NOT NULL IDENTITY(1, 1)
  , f_source_name NVARCHAR(50)   NOT NULL
  , f_source_text NVARCHAR(1000)     NULL
  , f_status_code TINYINT        NOT NULL
  , f_invoke_time DATETIME2(7)   NOT NULL
  , f_finish_time DATETIME2(7)       NULL
  , f_result_text NTEXT              NULL
  , f_insert_name NVARCHAR(50)   NOT NULL
  , f_insert_time DATETIME2(7)   NOT NULL
  , f_update_name NVARCHAR(50)   NOT NULL
  , f_update_time DATETIME2(7)   NOT NULL
  , CONSTRAINT pk_report_source PRIMARY KEY(f_source_code)
);
-- 実行情報テーブル
-- f_source_code : 基本番号
-- f_invoke_code : 実行番号
-- f_invoke_name : 実行名称
-- f_invoke_text : 実行内容
-- f_status_code : 状態種別(0:未実行 1:実行 2:成功 3:失敗 4:取消)
-- f_invoke_size : 実行件数
-- f_finish_size : 実行総数
-- f_invoke_time : 開始日時
-- f_finish_time : 終了日時
-- f_result_text : 結果内容
-- f_insert_name : 登録名称
-- f_insert_time : 登録日時
-- f_update_name : 更新名称
-- f_update_time : 更新日時
CREATE TABLE t_report_invoke (
    f_source_code INTEGER        NOT NULL
  , f_invoke_code INTEGER        NOT NULL
  , f_invoke_name NVARCHAR(50)   NOT NULL
  , f_invoke_text NVARCHAR(1000)     NULL
  , f_status_code TINYINT        NOT NULL
  , f_invoke_size BIGINT             NULL
  , f_finish_size BIGINT             NULL
  , f_invoke_time DATETIME2(7)       NULL
  , f_finish_time DATETIME2(7)       NULL
  , f_result_text NTEXT              NULL
  , f_insert_name NVARCHAR(50)   NOT NULL
  , f_insert_time DATETIME2(7)   NOT NULL
  , f_update_name NVARCHAR(50)   NOT NULL
  , f_update_time DATETIME2(7)   NOT NULL
  , CONSTRAINT pk_report_invoke PRIMARY KEY(f_source_code, f_invoke_code)
  , CONSTRAINT fk_report_invoke FOREIGN KEY(f_source_code) REFERENCES t_report_source(f_source_code)
);
GO
CREATE VIEW v_report_source AS
WITH w_report_invoke AS (
SELECT f_source_code
     , SUM(CASE WHEN f_status_code = 3 THEN f_finish_size ELSE f_invoke_size END) AS f_invoke_size
     , SUM(f_finish_size) AS f_finish_size
     , COUNT(f_source_code) AS f_summary_count
     , COUNT(CASE WHEN f_status_code = 0 THEN 1 END) AS f_nothing_count
     , COUNT(CASE WHEN f_status_code = 1 THEN 1 END) AS f_process_count
     , COUNT(CASE WHEN f_status_code = 2 THEN 1 END) AS f_success_count
     , COUNT(CASE WHEN f_status_code = 3 THEN 1 END) AS f_failure_count
     , COUNT(CASE WHEN f_status_code = 4 THEN 1 END) AS f_suspend_count
  FROM t_report_invoke
 GROUP BY f_source_code
)
SELECT t1.f_source_code
     , t1.f_source_name
     , CASE t1.f_status_code
            WHEN 1 THEN '実行'
            WHEN 2 THEN '成功'
            WHEN 3 THEN '失敗'
            WHEN 4 THEN '取消'
            ELSE        '不明(' || t1.f_source_code ||  ')'
       END AS f_status_name
     , t2.f_invoke_size
     , t2.f_finish_size
     , t1.f_invoke_time
     , t1.f_finish_time
     , t1.f_result_text
     , t2.f_summary_count
     , t2.f_nothing_count
     , t2.f_process_count
     , t2.f_success_count
     , t2.f_failure_count
     , t2.f_suspend_count
  FROM t_report_source t1
  LEFT JOIN w_report_invoke t2
    ON t2.f_source_code = t1.f_source_code;
