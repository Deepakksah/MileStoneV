using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using VideoOS.Platform.AccessControl.Elements;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

[Serializable]
[DataContract(Namespace = "")]
public class ACConfigurationSerializeData
{
	public static Type[] KnownTypes = new Type[8]
	{
		typeof(ACElement),
		typeof(ACStateType),
		typeof(ACCommandType),
		typeof(ACEventType),
		typeof(ACUnitType),
		typeof(ACServerType),
		typeof(ACUnit),
		typeof(ACServer)
	};

	private static readonly IEnumerable<Type> BinaryFormatterKnowntypes = KnownTypes.Concat(new List<Type>
	{
		typeof(ACConfigurationSerializeData),
		typeof(List<ACElement>),
		typeof(ReadOnlyCollection<string>),
		typeof(ReadOnlyCollection<ACProperty>),
		typeof(List<string>),
		typeof(List<ACProperty>),
		typeof(ACProperty)
	});

	private static DataContractSerializer _serializer = new DataContractSerializer(typeof(ACConfigurationSerializeData), KnownTypes);

	[DataMember]
	public DateTime Timestamp { get; set; }

	[DataMember]
	public List<ACElement> ACElements { get; set; }

	public static ACConfigurationSerializeData Deserialize(string serializedObject)
	{
		if (string.IsNullOrEmpty(serializedObject))
		{
			return null;
		}
		try
		{
			using MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(serializedObject));
			MemoryStream memoryStream2 = new MemoryStream();
			using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
			{
				byte[] array = new byte[10485760];
				while (memoryStream.Position != memoryStream.Length)
				{
					int count = gZipStream.Read(array, 0, array.Length);
					memoryStream2.Write(array, 0, count);
				}
			}
			memoryStream2.Seek(0L, SeekOrigin.Begin);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Binder = new BinaryFormaterTypeBinder(binaryFormatter.Binder, BinaryFormatterKnowntypes);
			return (ACConfigurationSerializeData)binaryFormatter.Deserialize(memoryStream2);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to deserialize binary access control configuration: " + ex.Message);
			try
			{
				EnvironmentManager.Instance.Log(error: false, "AccessControl", "Trying to fallback to XML format configuration.");
				using StringReader input = new StringReader(serializedObject);
				using XmlReader reader = XmlReader.Create(input);
				ACConfigurationSerializeData result = (ACConfigurationSerializeData)_serializer.ReadObject(reader);
				EnvironmentManager.Instance.Log(error: false, "AccessControl", "Successfully loaded XML format configuration.");
				return result;
			}
			catch (Exception ex2)
			{
				EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to deserialize XML access control configuration: " + ex2.Message);
			}
			return null;
		}
	}

	public static string Serialize(ACConfigurationSerializeData obj)
	{
		if (obj == null)
		{
			return string.Empty;
		}
		EnvironmentManager.Instance.Log(error: false, "AccessControl", $"Total Elements: {obj.ACElements.Count}");
		foreach (IGrouping<string, ACElement> item in from g in obj.ACElements
			group g by g.GetType().Name)
		{
			string key = item.Key;
			int num = item.Count();
			EnvironmentManager.Instance.Log(error: false, "AccessControl", $"Elements Type: {key}, Count: {num}");
		}
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			using (GZipStream serializationStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
			{
				new BinaryFormatter().Serialize(serializationStream, obj);
			}
			return Convert.ToBase64String(memoryStream.ToArray());
		}
		catch (SerializationException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to serialize access control configuration: " + ex.Message);
			throw;
		}
	}
}
