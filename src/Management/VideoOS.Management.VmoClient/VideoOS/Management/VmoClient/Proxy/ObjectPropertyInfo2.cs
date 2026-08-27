using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using VideoOS.Management.Client.Proxy;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ObjectPropertyInfo2", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
[KnownType(typeof(GenericObjectInfo2[]))]
[KnownType(typeof(GenericObjectInfo2))]
[KnownType(typeof(ObjectPropertyInfo2[]))]
[KnownType(typeof(RelationPropertyInfo[]))]
[KnownType(typeof(RelationPropertyInfo))]
[KnownType(typeof(CustomPropertyInfo[]))]
[KnownType(typeof(CustomPropertyInfo))]
[KnownType(typeof(GenericObjectKey))]
[KnownType(typeof(ObjectPropertyInfo[]))]
[KnownType(typeof(ObjectPropertyInfo))]
[KnownType(typeof(ServerGroupInfo[]))]
[KnownType(typeof(ServerGroupInfo))]
[KnownType(typeof(ServerGroupOptions))]
[KnownType(typeof(PluginEventInfo[]))]
[KnownType(typeof(PluginEventInfo))]
[KnownType(typeof(TaskInfo[]))]
[KnownType(typeof(TaskInfo))]
[KnownType(typeof(AsyncStatusInfo))]
[KnownType(typeof(ExceptionStatusInfo))]
[KnownType(typeof(KeyValueInfo[]))]
[KnownType(typeof(KeyValueInfo))]
[KnownType(typeof(BasicUserInfo[]))]
[KnownType(typeof(BasicUserInfo))]
[KnownType(typeof(ObjectPropertyInfo2[][]))]
[KnownType(typeof(CustomPropertyInfo[][]))]
[KnownType(typeof(MarkedDataReferenceInfo2))]
[KnownType(typeof(VMOExceptionDetail))]
[KnownType(typeof(MessageParameter))]
[KnownType(typeof(ServerAddress[]))]
[KnownType(typeof(ServerAddress))]
[KnownType(typeof(ServerAddressType))]
[KnownType(typeof(MarkedDataDefinitions.MarkedDataTypes))]
[KnownType(typeof(MediaStatusEntry))]
[KnownType(typeof(MediaDataEntry[]))]
[KnownType(typeof(MediaDataEntry))]
[KnownType(typeof(ExceptionDetail))]
[KnownType(typeof(Dictionary<int, MessageParameter>))]
[KnownType(typeof(string[]))]
[KnownType(typeof(Guid[]))]
[KnownType(typeof(EncryptionMethod))]
[DebuggerDisplay("ObjectPropertyInfo2; Name: {NameField}, Value: {ValueField}")]
public class ObjectPropertyInfo2 : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string NameField;

	private object ValueField;

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

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}

	[DataMember]
	public object Value
	{
		get
		{
			return ValueField;
		}
		set
		{
			ValueField = value;
		}
	}
}
