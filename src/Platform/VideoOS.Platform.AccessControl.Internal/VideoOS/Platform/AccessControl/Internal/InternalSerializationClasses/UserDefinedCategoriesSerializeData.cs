using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

[DataContract(Namespace = "")]
public class UserDefinedCategoriesSerializeData
{
	[DataContract(Namespace = "")]
	public class UserDefinedCategory
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string AccessControlTypeString { get; set; }
	}

	private static DataContractSerializer _serializer = new DataContractSerializer(typeof(UserDefinedCategoriesSerializeData));

	[DataMember]
	public List<UserDefinedCategory> UserDefinedCategories { get; set; }

	public static UserDefinedCategoriesSerializeData Deserialize(string serializedObject)
	{
		if (string.IsNullOrEmpty(serializedObject))
		{
			return null;
		}
		try
		{
			using StringReader input = new StringReader(serializedObject);
			using XmlReader reader = XmlReader.Create(input);
			return (UserDefinedCategoriesSerializeData)_serializer.ReadObject(reader);
		}
		catch (SerializationException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to deserialize user defined categories: " + ex.Message);
			return null;
		}
	}

	public static string Serialize(UserDefinedCategoriesSerializeData obj)
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
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to serialize user defined categories: " + ex.Message);
			throw;
		}
	}

	public UserDefinedCategoriesSerializeData()
	{
		UserDefinedCategories = new List<UserDefinedCategory>();
	}
}
