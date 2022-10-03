namespace Otchitta.Example.Demo01.Constant;

/// <summary>
/// 状態種別列挙体です。
/// </summary>
public enum StatusType : byte {
	/// <summary>待機</summary>
	Nothing = 0,
	/// <summary>開始</summary>
	Process = 1,
	/// <summary>成功</summary>
	Success = 2,
	/// <summary>失敗</summary>
	Failure = 3,
	/// <summary>取消</summary>
	Suspend = 4
}
