-- DROP VIEW V_report_source;
-- DROP TABLE t_report_invoke;
-- DROP TABLE t_report_source;

-- 基本情報テーブル
-- f_source_code : 基本番号
-- f_source_name : 基本名称
-- f_source_text : 基本内容
-- f_status_code : 状態種別(1:実行中 2:実行済)
-- f_invoke_time : 開始日時
-- f_finish_time : 終了日時
-- f_result_text : 結果内容
-- f_insert_name : 登録名称
-- f_insert_time : 登録日時
-- f_update_name : 更新名称
-- f_update_time : 更新日時
CREATE TABLE t_report_source (
    f_source_code INTEGER        NOT NULL PRIMARY KEY AUTOINCREMENT
  , f_source_name NVARCHAR(50)   NOT NULL
  , f_source_text NVARCHAR(1000)     NULL
  , f_status_code INTEGER        NOT NULL
  , f_invoke_time TIMESTAMP      NOT NULL
  , f_finish_time TIMESTAMP          NULL
  , f_result_text NTEXT              NULL
  , f_insert_name NVARCHAR(50)   NOT NULL
  , f_insert_time TIMESTAMP      NOT NULL
  , f_update_name NVARCHAR(50)   NOT NULL
  , f_update_time TIMESTAMP      NOT NULL
);
-- 実行情報テーブル
-- f_source_code : 基本番号
-- f_invoke_code : 実行番号
-- f_invoke_name : 実行名称
-- f_invoke_text : 実行内容
-- f_status_code : 状態種別(0:未実行 1:実行中 2:実行済)
-- f_invoke_size : 実行件数
-- f_finish_size : 実行総数
-- f_invoke_time : 開始日時
-- f_finish_time : 終了日時
-- f_result_flag : 結果種別(NULL:未確定 TRUE:成功 FALSE:失敗)
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
  , f_status_code INTEGER        NOT NULL
  , f_invoke_size INTEGER            NULL
  , f_finish_size INTEGER            NULL
  , f_invoke_time TIMESTAMP          NULL
  , f_finish_time TIMESTAMP          NULL
  , f_result_flag BOOLEAN            NULL
  , f_result_text NTEXT              NULL
  , f_insert_name NVARCHAR(50)   NOT NULL
  , f_insert_time TIMESTAMP      NOT NULL
  , f_update_name NVARCHAR(50)   NOT NULL
  , f_update_time TIMESTAMP      NOT NULL
  , PRIMARY KEY(f_source_code, f_invoke_code)
  , FOREIGN KEY(f_source_code) REFERENCES t_report_source(f_source_code)
);
CREATE VIEW v_report_source AS
WITH w_report_invoke AS (
SELECT f_source_code
     , SUM(CASE WHEN f_result_flag = 0 THEN f_finish_size ELSE f_invoke_size END) AS f_invoke_size
     , SUM(f_finish_size) AS f_finish_size
     , COUNT(f_source_code) AS f_summary_count
     , COUNT(CASE WHEN f_status_code = 0 THEN 1 END) AS f_nothing_count
     , COUNT(CASE WHEN f_status_code = 1 THEN 1 END) AS f_process_count
     , COUNT(CASE WHEN f_status_code = 2 AND f_result_flag = 1 THEN 1 END) AS f_success_count
     , COUNT(CASE WHEN f_status_code = 2 AND f_result_flag = 0 THEN 1 END) AS f_failure_count
  FROM t_report_invoke
 GROUP BY f_source_code
)
SELECT t1.f_source_code
     , t1.f_source_name
     , CASE t1.f_status_code
            WHEN 1 THEN '実行中'
            WHEN 2 THEN '実行済'
            ELSE        '不明(' || t1.f_source_code || ')'
       END AS f_status_name
     , CASE WHEN t1.f_status_code = 2
            THEN CASE WHEN t1.f_result_text IS NULL THEN '成功' ELSE '失敗' END
            ELSE NULL
       END AS f_result_name
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
  FROM t_report_source t1
  LEFT JOIN w_report_invoke t2
    ON t2.f_source_code = t1.f_source_code;
