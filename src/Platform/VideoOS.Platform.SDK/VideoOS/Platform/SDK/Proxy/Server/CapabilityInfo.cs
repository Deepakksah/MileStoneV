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
public class CapabilityInfo : ICapabilityInfo
{
	private Guid capabilityIdField;

	private string nameField;

	private bool absoluteField;

	private bool relativeField;

	private bool startField;

	private bool stopField;

	private bool speedField;

	private bool automaticField;

	public Guid CapabilityId
	{
		get
		{
			return capabilityIdField;
		}
		set
		{
			capabilityIdField = value;
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

	public bool Absolute
	{
		get
		{
			return absoluteField;
		}
		set
		{
			absoluteField = value;
		}
	}

	public bool Relative
	{
		get
		{
			return relativeField;
		}
		set
		{
			relativeField = value;
		}
	}

	public bool Start
	{
		get
		{
			return startField;
		}
		set
		{
			startField = value;
		}
	}

	public bool Stop
	{
		get
		{
			return stopField;
		}
		set
		{
			stopField = value;
		}
	}

	public bool Speed
	{
		get
		{
			return speedField;
		}
		set
		{
			speedField = value;
		}
	}

	public bool Automatic
	{
		get
		{
			return automaticField;
		}
		set
		{
			automaticField = value;
		}
	}
}
