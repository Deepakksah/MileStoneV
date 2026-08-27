using System.IO;
using System.Text;

namespace VideoOS.Mobile.Portable.Utilities;

public class EncodedStringWriter : StringWriter
{
	private readonly Encoding _encoding = Encoding.UTF8;

	public override Encoding Encoding => _encoding;

	public EncodedStringWriter()
	{
	}

	public EncodedStringWriter(Encoding encoding)
	{
		_encoding = encoding;
	}
}
