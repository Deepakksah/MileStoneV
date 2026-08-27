using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class Thumbnail
{
	public const string ThumbnailName = "Thumbnail";

	[JsonProperty("Data")]
	[XmlText]
	public string Binary;

	public void FromByteArray(byte[] data)
	{
		Binary = FromByteArrayS(data);
	}

	public static string FromByteArrayS(byte[] data)
	{
		return Convert.ToBase64String(data);
	}

	public byte[] ToByteArray()
	{
		return ToByteArray(Binary);
	}

	public static byte[] ToByteArray(string binaryText)
	{
		return Convert.FromBase64String(binaryText);
	}
}
