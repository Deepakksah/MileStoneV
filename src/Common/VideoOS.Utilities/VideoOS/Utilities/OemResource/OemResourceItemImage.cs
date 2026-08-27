using System;
using System.Drawing;
using System.Formats.Nrbf;
using System.IO;
using System.Xml;

namespace VideoOS.Utilities.OemResource;

public class OemResourceItemImage : OemResourceItemBase
{
	public const string ResourceTypeStringConst = "System.Drawing.Image";

	public Image ResourceValue
	{
		get
		{
			byte[] buffer;
			using (MemoryStream memoryStream = new MemoryStream(_valueBytes))
			{
				try
				{
					buffer = (byte[])NrbfDecoder.DecodeClassRecord((Stream)memoryStream, (PayloadOptions)null, false).GetArrayRecord("Data").GetArray(typeof(byte[]), true);
				}
				catch (Exception)
				{
					throw new InvalidDataException("Disallowed type deserialization attempt!");
				}
			}
			return new Bitmap(new MemoryStream(buffer));
		}
	}

	public override object ResourceValueAsObject
	{
		get
		{
			return ResourceValue;
		}
		set
		{
			throw new NotImplementedException("OemResourceItemImage is read only due to deprecation of BinaryFormatter from .Net 9");
		}
	}

	public override string ResourceTypeString => "System.Drawing.Image";

	public OemResourceItemImage()
	{
	}

	public OemResourceItemImage(XmlNode oemResourceItemNode)
		: base(oemResourceItemNode)
	{
	}
}
