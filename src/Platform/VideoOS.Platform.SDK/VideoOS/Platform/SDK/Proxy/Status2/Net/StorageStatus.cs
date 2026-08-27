using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "StorageStatus", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class StorageStatus : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid StorageIdField;

	private string NameField;

	private string PathField;

	private bool AvailableField;

	private ulong UsedSpaceInBytesField;

	private ulong FreeSpaceInBytesField;

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

	[DataMember(IsRequired = true)]
	public Guid StorageId
	{
		get
		{
			return StorageIdField;
		}
		set
		{
			StorageIdField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 1)]
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

	[DataMember(EmitDefaultValue = false, Order = 2)]
	public string Path
	{
		get
		{
			return PathField;
		}
		set
		{
			PathField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 3)]
	public bool Available
	{
		get
		{
			return AvailableField;
		}
		set
		{
			AvailableField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 4)]
	public ulong UsedSpaceInBytes
	{
		get
		{
			return UsedSpaceInBytesField;
		}
		set
		{
			UsedSpaceInBytesField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 5)]
	public ulong FreeSpaceInBytes
	{
		get
		{
			return FreeSpaceInBytesField;
		}
		set
		{
			FreeSpaceInBytesField = value;
		}
	}
}
