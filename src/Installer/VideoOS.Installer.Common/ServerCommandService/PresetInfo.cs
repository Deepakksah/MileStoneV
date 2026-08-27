using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PresetInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class PresetInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid IdField;

	private bool LockedField;

	private string NameField;

	private PositionInfo PositionField;

	private string ShortcutField;

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
	public bool Locked
	{
		get
		{
			return LockedField;
		}
		set
		{
			LockedField = value;
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
	public PositionInfo Position
	{
		get
		{
			return PositionField;
		}
		set
		{
			PositionField = value;
		}
	}

	[DataMember]
	public string Shortcut
	{
		get
		{
			return ShortcutField;
		}
		set
		{
			ShortcutField = value;
		}
	}
}
