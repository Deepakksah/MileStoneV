using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace VideoOS.Mobile.Utilities.Zip;

public class SharpZipFiles
{
	private string _filePathName;

	private string _filePathInZip;

	public CompressionMethod CompressionMethod { get; set; }

	public string FilePathName
	{
		get
		{
			return _filePathName;
		}
		set
		{
			_filePathName = value;
			FileName = Path.GetFileName(_filePathName);
		}
	}

	public string FilePathInZip
	{
		get
		{
			return _filePathInZip ?? (".\\" + Path.GetDirectoryName(FilePathName));
		}
		set
		{
			_filePathInZip = value;
		}
	}

	public string FileName { get; private set; }
}
