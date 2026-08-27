using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

[DataContract(Namespace = "")]
public class OperationableInstanceConfigurationSerializeData
{
	[DataContract(Namespace = "")]
	public class OperationableInstanceConfigurationData
	{
		[DataMember]
		public Guid AccessControlOperationableInstanceId { get; set; }

		[DataMember]
		public bool Enabled { get; set; }
	}

	private static DataContractSerializer _serializer = new DataContractSerializer(typeof(OperationableInstanceConfigurationSerializeData));

	[DataMember]
	public List<OperationableInstanceConfigurationData> OperationableInstanceConfiguration { get; set; }

	public static OperationableInstanceConfigurationSerializeData Deserialize(string serializedObject)
	{
		if (string.IsNullOrEmpty(serializedObject))
		{
			return null;
		}
		try
		{
			using StringReader input = new StringReader(serializedObject);
			using XmlReader reader = XmlReader.Create(input);
			return (OperationableInstanceConfigurationSerializeData)_serializer.ReadObject(reader);
		}
		catch (SerializationException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to deserialize operationable instance configuration: " + ex.Message);
			return null;
		}
	}

	public static string Serialize(OperationableInstanceConfigurationSerializeData obj)
	{
		if (obj == null)
		{
			return string.Empty;
		}
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			using (XmlWriter writer = XmlWriter.Create(stringBuilder, new XmlWriterSettings
			{
				OmitXmlDeclaration = true
			}))
			{
				_serializer.WriteObject(writer, obj);
			}
			return stringBuilder.ToString();
		}
		catch (SerializationException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to serialize operationable instance configuration: " + ex.Message);
			throw;
		}
	}

	public OperationableInstanceConfigurationSerializeData()
	{
		OperationableInstanceConfiguration = new List<OperationableInstanceConfigurationData>();
	}
}
