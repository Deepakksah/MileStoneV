using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace VideoOS.Platform.Client.Hid;

public class HidConfigurationPropertySerializer
{
	private static readonly Type[] KnownTypes = new Type[4]
	{
		typeof(HidStringConfigurationProperty),
		typeof(HidEnumConfigurationProperty),
		typeof(HidIntConfigurationProperty),
		typeof(HidBoolConfigurationProperty)
	};

	private static readonly DataContractSerializer Serializer = new DataContractSerializer(typeof(HidConfigurationPropertyCollection), KnownTypes);

	public static string Serialize(HidConfigurationPropertyCollection collection)
	{
		using MemoryStream memoryStream = new MemoryStream();
		Serializer.WriteObject(memoryStream, collection);
		return Encoding.UTF8.GetString(memoryStream.ToArray());
	}

	public static HidConfigurationPropertyCollection Deserialize(string xml)
	{
		using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
		return (HidConfigurationPropertyCollection)Serializer.ReadObject(stream);
	}
}
