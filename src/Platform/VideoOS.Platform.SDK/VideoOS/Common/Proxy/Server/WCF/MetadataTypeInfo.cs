using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MetadataTypeInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class MetadataTypeInfo : IExtensibleDataObject, IMetadataTypeInfo
{
	private ExtensionDataObject extensionDataField;

	private Guid DisplayIdField;

	private string DisplayNameField;

	private Guid IdField;

	private string NameField;

	private int ValidTimeField;

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
	public Guid DisplayId
	{
		get
		{
			return DisplayIdField;
		}
		set
		{
			DisplayIdField = value;
		}
	}

	[DataMember]
	public string DisplayName
	{
		get
		{
			return DisplayNameField;
		}
		set
		{
			DisplayNameField = value;
		}
	}

	[DataMember]
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
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
	public int ValidTime
	{
		get
		{
			return ValidTimeField;
		}
		set
		{
			ValidTimeField = value;
		}
	}

	Guid IMetadataTypeInfo.Id
	{
		get
		{
			return Id;
		}
		set
		{
			Id = value;
		}
	}

	string IMetadataTypeInfo.Name
	{
		get
		{
			return Name;
		}
		set
		{
			Name = value;
		}
	}

	Guid IMetadataTypeInfo.DisplayId
	{
		get
		{
			return DisplayId;
		}
		set
		{
			DisplayId = value;
		}
	}

	string IMetadataTypeInfo.DisplayName
	{
		get
		{
			return DisplayName;
		}
		set
		{
			DisplayName = value;
		}
	}

	int IMetadataTypeInfo.ValidTime
	{
		get
		{
			return ValidTime;
		}
		set
		{
			ValidTime = value;
		}
	}
}
