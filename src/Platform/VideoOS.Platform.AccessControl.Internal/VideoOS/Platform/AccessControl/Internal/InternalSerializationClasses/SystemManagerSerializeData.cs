using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

[DataContract(Namespace = "")]
public class SystemManagerSerializeData : IExtensibleDataObject
{
	[DataContract(Namespace = "")]
	public class Property
	{
		[DataMember]
		public string Key { get; set; }

		[DataMember]
		public string Value { get; set; }
	}

	private static DataContractSerializer _serializer = new DataContractSerializer(typeof(SystemManagerSerializeData), ACConfigurationSerializeData.KnownTypes.Concat(RuleConfigurationSerializeData.KnownTypes).ToArray());

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public Guid PluginId { get; set; }

	[DataMember]
	public bool Enabled { get; set; }

	[DataMember]
	public List<Property> Properties { get; set; }

	[DataMember]
	public ACConfigurationSerializeData AccessControlSystemConfiguration { get; set; }

	[DataMember]
	public CategoryMappingConfigurationSerializeData AccessControlCategoryMappingConfiguration { get; set; }

	[DataMember]
	public SurveillanceItemMappingSerializeData AccessControlSurveillanceItemMappingConfiguration { get; set; }

	[DataMember]
	public OperationableInstanceConfigurationSerializeData AccessControlOperationableInstanceConfiguration { get; set; }

	[DataMember]
	public RuleConfigurationSerializeData AccessControlRuleConfiguration { get; set; }

	[DataMember]
	public bool PersonalizedLoginSupported { get; set; }

	[DataMember]
	public bool PersonalizedLoginEnabled { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public static SystemManagerSerializeData Deserialize(string serializedObject)
	{
		if (string.IsNullOrEmpty(serializedObject))
		{
			return null;
		}
		try
		{
			using StringReader input = new StringReader(serializedObject);
			using XmlReader reader = XmlReader.Create(input);
			SystemManagerSerializeData systemManagerSerializeData = (SystemManagerSerializeData)_serializer.ReadObject(reader);
			if (systemManagerSerializeData.Properties == null)
			{
				systemManagerSerializeData.Properties = new List<Property>();
			}
			return systemManagerSerializeData;
		}
		catch (SerializationException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to deserialize system manager: " + ex.Message);
			return null;
		}
	}

	public static string Serialize(SystemManagerSerializeData obj)
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
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to serialize system manager: " + ex.Message);
			throw;
		}
	}
}
