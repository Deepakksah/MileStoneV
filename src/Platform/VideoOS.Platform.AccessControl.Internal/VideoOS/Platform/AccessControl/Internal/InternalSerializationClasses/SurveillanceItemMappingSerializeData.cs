using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

[DataContract(Namespace = "")]
public class SurveillanceItemMappingSerializeData
{
	[DataContract(Namespace = "")]
	public class SurveillanceItemMappingData
	{
		[DataMember]
		public Guid AccessControlOperationableInstanceId { get; set; }

		[DataMember]
		public List<FQID> RelatedSurveillanceItems { get; set; }
	}

	private static DataContractSerializer _serializer = new DataContractSerializer(typeof(SurveillanceItemMappingSerializeData));

	[DataMember]
	public List<SurveillanceItemMappingData> SurveillanceItemMappings { get; set; }

	public static SurveillanceItemMappingSerializeData Deserialize(string serializedObject)
	{
		if (string.IsNullOrEmpty(serializedObject))
		{
			return null;
		}
		try
		{
			using StringReader input = new StringReader(serializedObject);
			using XmlReader reader = XmlReader.Create(input);
			return (SurveillanceItemMappingSerializeData)_serializer.ReadObject(reader);
		}
		catch (SerializationException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to deserialize surveillance item mapping: " + ex.Message);
			return null;
		}
	}

	public static string Serialize(SurveillanceItemMappingSerializeData obj)
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
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to serialize surveillance item mapping: " + ex.Message);
			throw;
		}
	}

	public SurveillanceItemMappingSerializeData()
	{
		SurveillanceItemMappings = new List<SurveillanceItemMappingData>();
	}
}
