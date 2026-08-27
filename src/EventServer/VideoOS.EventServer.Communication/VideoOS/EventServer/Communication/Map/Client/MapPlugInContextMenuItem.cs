using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MapPlugInContextMenuItem", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server.WebService.Map")]
public class MapPlugInContextMenuItem : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool CheckedField;

	private string ContextMenuIDField;

	private bool EnabledField;

	private byte[] IconField;

	private bool IsCheckableField;

	private MapPlugInContextMenuItem[] SubMenuItemsField;

	private string TextField;

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
	public bool Checked
	{
		get
		{
			return CheckedField;
		}
		set
		{
			CheckedField = value;
		}
	}

	[DataMember]
	public string ContextMenuID
	{
		get
		{
			return ContextMenuIDField;
		}
		set
		{
			ContextMenuIDField = value;
		}
	}

	[DataMember]
	public bool Enabled
	{
		get
		{
			return EnabledField;
		}
		set
		{
			EnabledField = value;
		}
	}

	[DataMember]
	public byte[] Icon
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
	public bool IsCheckable
	{
		get
		{
			return IsCheckableField;
		}
		set
		{
			IsCheckableField = value;
		}
	}

	[DataMember]
	public MapPlugInContextMenuItem[] SubMenuItems
	{
		get
		{
			return SubMenuItemsField;
		}
		set
		{
			SubMenuItemsField = value;
		}
	}

	[DataMember]
	public string Text
	{
		get
		{
			return TextField;
		}
		set
		{
			TextField = value;
		}
	}
}
