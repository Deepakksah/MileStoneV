using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VideoOS.Mobile.Utilities.Zip;

public class ZipEntryRenamer
{
	private int _counter = 1;

	public string CorrectFileName(SharpZipFiles file, IEnumerable<SharpZipFiles> files)
	{
		if (files.Where((SharpZipFiles f) => f.FileName == file.FileName && f.FilePathInZip == file.FilePathInZip).Count() > 1)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Path.GetFileNameWithoutExtension(file.FileName));
			stringBuilder.Append(" (").Append(_counter++).Append(')');
			stringBuilder.Append(Path.GetExtension(file.FileName));
			return stringBuilder.ToString();
		}
		return file.FileName;
	}
}
