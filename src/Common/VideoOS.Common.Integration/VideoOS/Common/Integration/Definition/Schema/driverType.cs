using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Schema;

[Serializable]
[GeneratedCode("xsd", "4.7.2558.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class driverType
{
	private string versionField;

	private string nameField;

	private string numberField;

	private firmwareType firmwareField;

	private proxyprocessType proxyprocessField;

	private deviceconnectionType deviceconnectionField;

	private displaynameType displaynameField;

	private string idField;

	private string versionidField;

	public string version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}

	public string name
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

	[XmlElement(DataType = "integer")]
	public string number
	{
		get
		{
			return numberField;
		}
		set
		{
			numberField = value;
		}
	}

	public firmwareType firmware
	{
		get
		{
			return firmwareField;
		}
		set
		{
			firmwareField = value;
		}
	}

	public proxyprocessType proxyprocess
	{
		get
		{
			return proxyprocessField;
		}
		set
		{
			proxyprocessField = value;
		}
	}

	public deviceconnectionType deviceconnection
	{
		get
		{
			return deviceconnectionField;
		}
		set
		{
			deviceconnectionField = value;
		}
	}

	public displaynameType displayname
	{
		get
		{
			return displaynameField;
		}
		set
		{
			displaynameField = value;
		}
	}

	[XmlAttribute]
	public string id
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

	[XmlAttribute]
	public string versionid
	{
		get
		{
			return versionidField;
		}
		set
		{
			versionidField = value;
		}
	}
}
