using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class PresetInfo : IPresetInfo
{
	private string nameField;

	private PositionInfo positionField;

	private string shortcutField;

	private Guid idField;

	private bool lockedField;

	IPositionInfo IPresetInfo.Position
	{
		get
		{
			return Position;
		}
		set
		{
			Position = (PositionInfo)value;
		}
	}

	bool IPresetInfo.Locked
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	public PositionInfo Position
	{
		get
		{
			return positionField;
		}
		set
		{
			positionField = value;
		}
	}

	public string Shortcut
	{
		get
		{
			return shortcutField;
		}
		set
		{
			shortcutField = value;
		}
	}

	public Guid Id
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
		}
	}

	public bool Locked
	{
		get
		{
			return lockedField;
		}
		set
		{
			lockedField = value;
		}
	}
}
