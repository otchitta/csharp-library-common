using System;
using System.Xml;

namespace Otchitta.Libraries.Common.Xml;

/// <summary>
/// <see cref="XmlNode" />用共通関数クラスです。
/// </summary>
public static class XmlNodeUtilities {
	/// <summary>
	/// 指定ファイルより<see cref="XmlNode" />を読込みます。
	/// </summary>
	/// <param name="sourceFile">ファイル</param>
	/// <returns>読込情報</returns>
	/// <exception cref="System.Xml.XmlException">There is a load or parse error in the XML. In this case, a System.IO.FileNotFoundException
	/// is raised.</exception>
	/// <exception cref="System.ArgumentException">filename is a zero-length string, contains only white space, or contains one
	/// or more invalid characters as defined by System.IO.Path.InvalidPathChars.</exception>
	/// <exception cref="System.ArgumentNullException">filename is null.</exception>
	/// <exception cref="System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
	/// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
	/// <exception cref="System.IO.IOException">An I/O error occurred while opening the file.</exception>
	/// <exception cref="System.UnauthorizedAccessException">filename specified a file that is read-only. -or- This operation is not supported
	/// on the current platform. -or- filename specified a directory. -or- The caller
	/// does not have the required permission.</exception>
	/// <exception cref="System.IO.FileNotFoundException">The file specified in filename was not found.</exception>
	/// <exception cref="System.NotSupportedException">filename is in an invalid format.</exception>
	/// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
	/// <exception cref="SystemException">読込に失敗した場合</exception>
	public static XmlNode Create(string sourceFile) {
		var document = new XmlDocument();
		document.Load(sourceFile);
		return document.DocumentElement ?? throw new SystemException($"XMLファイルの読込みに失敗しました。{Environment.NewLine}ファイル={sourceFile}");
	}
}
