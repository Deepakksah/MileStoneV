using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.Messaging;

[Serializable]
[DataContract(Name = "ExternalMessage", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Messaging")]
[KnownType(typeof(Alarm))]
[KnownType(typeof(EventHeader))]
[KnownType(typeof(EventData))]
[KnownType(typeof(EventSource))]
[KnownType(typeof(FQID))]
[KnownType(typeof(ServerId))]
[KnownType(typeof(FolderType))]
public sealed class ExternalMessage : IExtensibleDataObject
{
	[DataMember]
	private string _messageId;

	[DataMember]
	private FQID _relatedFQID;

	[DataMember]
	private string _reason;

	private ExtensionDataObject extensionDataField;

	private object _deserializedData;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	public string MessageId => _messageId;

	public FQID RelatedFQID
	{
		get
		{
			return _relatedFQID;
		}
		set
		{
			_relatedFQID = value;
		}
	}

	public object Data
	{
		get
		{
			if (_deserializedData == null && DataClass != "" && DataClassContent != null)
			{
				StringReader stringReader = null;
				try
				{
					stringReader = new StringReader(DataClassContent);
					XmlReader xmlReader = new XmlTextReader(stringReader);
					stringReader = null;
					DataContractSerializer dataContractSerializer = new DataContractSerializer(Type.GetType(DataClass));
					_deserializedData = dataContractSerializer.ReadObject(xmlReader);
					xmlReader.Close();
				}
				catch (Exception ex)
				{
					EnvironmentManager.Instance.Log(error: true, "ExtenalMessage.Deserialize", "Unable to deserialize class:" + DataClass + ", Exception:" + ex.Message);
				}
				finally
				{
					stringReader?.Dispose();
				}
			}
			return _deserializedData;
		}
		set
		{
			_deserializedData = value;
			if (value != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
				try
				{
					DataClass = value.GetType().AssemblyQualifiedName;
					using XmlTextWriter writer = new XmlTextWriter(stringWriter);
					stringWriter = null;
					new DataContractSerializer(value.GetType()).WriteObject(writer, value);
					DataClassContent = stringBuilder.ToString();
					return;
				}
				catch (Exception ex)
				{
					EnvironmentManager.Instance.Log(error: true, "ExternalMessage.Serialize", "Unable to serialize class:" + DataClass + ", Exception:" + ex.Message);
					return;
				}
				finally
				{
					stringWriter?.Dispose();
				}
			}
			DataClass = "";
			DataClassContent = "";
		}
	}

	public string Reason
	{
		get
		{
			return _reason;
		}
		set
		{
			_reason = value;
		}
	}

	[DataMember]
	public FQID SourceEndPoint { get; set; }

	[DataMember]
	public EndPointType SourceEndPointType { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public FQID Source { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public FQID Destination { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public string DataClass { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public string DataClassContent { get; set; }

	public ExternalMessage(string messageId)
	{
		_messageId = messageId;
	}

	public ExternalMessage(string messageId, FQID relatedFQID)
	{
		_messageId = messageId;
		_relatedFQID = relatedFQID;
	}
}
