using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

[DataContract(Namespace = "")]
public class CategoryMappingConfigurationSerializeData
{
	[DataContract(Namespace = "")]
	public class CategoryMapping
	{
		[DataMember]
		public Guid AccessControlTypeId { get; set; }

		[DataMember]
		public List<Guid> CategoryIds { get; set; }
	}

	private static DataContractSerializer _serializer = new DataContractSerializer(typeof(CategoryMappingConfigurationSerializeData));

	[DataMember]
	public List<CategoryMapping> CategoryMappings { get; set; }

	public static CategoryMappingConfigurationSerializeData Deserialize(string serializedObject)
	{
		if (string.IsNullOrEmpty(serializedObject))
		{
			return null;
		}
		try
		{
			using StringReader input = new StringReader(serializedObject);
			using XmlReader reader = XmlReader.Create(input);
			return (CategoryMappingConfigurationSerializeData)_serializer.ReadObject(reader);
		}
		catch (SerializationException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to deserialize category mapping configuration: " + ex.Message);
			return null;
		}
	}

	public static string Serialize(CategoryMappingConfigurationSerializeData obj)
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
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to serialize category mapping configuration: " + ex.Message);
			throw;
		}
	}

	public CategoryMappingConfigurationSerializeData()
	{
		CategoryMappings = new List<CategoryMapping>();
	}
}
