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
public class eventType
{
	private string nameField;

	private displaynameType displaynameField;

	private capabilitiesType capabilitiesField;

	private metadataentryType[] metadataField;

	private string referenceidField;

	private string countereventreferenceidField;

	private string dynamicField;

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

	public capabilitiesType capabilities
	{
		get
		{
			return capabilitiesField;
		}
		set
		{
			capabilitiesField = value;
		}
	}

	[XmlArrayItem("data", IsNullable = false)]
	public metadataentryType[] metadata
	{
		get
		{
			return metadataField;
		}
		set
		{
			metadataField = value;
		}
	}

	[XmlAttribute]
	public string referenceid
	{
		get
		{
			return referenceidField;
		}
		set
		{
			referenceidField = value;
		}
	}

	[XmlAttribute]
	public string countereventreferenceid
	{
		get
		{
			return countereventreferenceidField;
		}
		set
		{
			countereventreferenceidField = value;
		}
	}

	[XmlAttribute]
	public string dynamic
	{
		get
		{
			return dynamicField;
		}
		set
		{
			dynamicField = value;
		}
	}

	public Guid GetReferenceGuid()
	{
		return new Guid(referenceid);
	}

	public Guid GetCounterEventReferenceGuid()
	{
		if (countereventreferenceid != null)
		{
			return new Guid(countereventreferenceid);
		}
		return new Guid(referenceid);
	}
}
