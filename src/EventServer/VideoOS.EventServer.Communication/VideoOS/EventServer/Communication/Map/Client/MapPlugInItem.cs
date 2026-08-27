using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MapPlugInItem", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server.WebService.Map")]
public class MapPlugInItem : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private MapPlugInContextMenuItem[] ContextMenuField;

	private bool FolderField;

	private string IDField;

	private Guid IconField;

	private Guid ItemTypeField;

	private string NameField;

	private string ParentIDField;

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
	public MapPlugInContextMenuItem[] ContextMenu
	{
		get
		{
			return ContextMenuField;
		}
		set
		{
			ContextMenuField = value;
		}
	}

	[DataMember]
	public bool Folder
	{
		get
		{
			return FolderField;
		}
		set
		{
			FolderField = value;
		}
	}

	[DataMember]
	public string ID
	{
		get
		{
			return IDField;
		}
		set
		{
			IDField = value;
		}
	}

	[DataMember]
	public Guid Icon
	{
		get
		{
			return IconField;
		}
		set
		{
			IconField = value;
		}
	}

	[DataMember]
	public Guid ItemType
	{
		get
		{
			return ItemTypeField;
		}
		set
		{
			ItemTypeField = value;
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
	public string ParentID
	{
		get
		{
			return ParentIDField;
		}
		set
		{
			ParentIDField = value;
		}
	}
}
