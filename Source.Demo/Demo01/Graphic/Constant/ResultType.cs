namespace Otchitta.Example.Demo01.Constant;

/// <summary>
/// 結果種別列挙体です。
/// </summary>
public enum ResultType : byte {
	/// <summary>除外</summary>
	Nothing = 0,
	/// <summary>成功</summary>
	Success = 1,
	/// <summary>警告</summary>
	Warning = 2,
	/// <summary>失敗</summary>
	Failure = 3
}
