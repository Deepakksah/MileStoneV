using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Schema;

[Serializable]
[GeneratedCode("xsd", "4.7.2558.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class deviceType
{
	private implementationType implementationField;

	private supportedeventType[] supportedeventsField;

	private supportedstreamtypeType[] supportedstreamtypesField;

	private capabilitiesType capabilitiesField;

	private deviceptzsettingsType ptzsettingsField;

	private outputsettingsType outputsettingsField;

	private streamType[] streamsField;

	private metadataitemType[] metadataitemsField;

	private string repeatcountField;

	public implementationType implementation
	{
		get
		{
			return implementationField;
		}
		set
		{
			implementationField = value;
		}
	}

	[XmlArrayItem("event", IsNullable = false)]
	public supportedeventType[] supportedevents
	{
		get
		{
			return supportedeventsField;
		}
		set
		{
			supportedeventsField = value;
		}
	}

	[XmlArrayItem("streamtype", IsNullable = false)]
	public supportedstreamtypeType[] supportedstreamtypes
	{
		get
		{
			return supportedstreamtypesField;
		}
		set
		{
			supportedstreamtypesField = value;
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

	public deviceptzsettingsType ptzsettings
	{
		get
		{
			return ptzsettingsField;
		}
		set
		{
			ptzsettingsField = value;
		}
	}

	public outputsettingsType outputsettings
	{
		get
		{
			return outputsettingsField;
		}
		set
		{
			outputsettingsField = value;
		}
	}

	[XmlArrayItem("stream", IsNullable = false)]
	public streamType[] streams
	{
		get
		{
			return streamsField;
		}
		set
		{
			streamsField = value;
		}
	}

	[XmlArrayItem("metadataitem", IsNullable = false)]
	public metadataitemType[] metadataitems
	{
		get
		{
			return metadataitemsField;
		}
		set
		{
			metadataitemsField = value;
		}
	}

	[XmlAttribute]
	public string repeatcount
	{
		get
		{
			return repeatcountField;
		}
		set
		{
			repeatcountField = value;
		}
	}

	public streamType FindStreamByReferenceId(string referenceId)
	{
		if (streams == null)
		{
			return null;
		}
		streamType[] array = streams;
		foreach (streamType streamType2 in array)
		{
			if (string.Compare(streamType2.referenceid, referenceId, ignoreCase: true) == 0)
			{
				return streamType2;
			}
		}
		return null;
	}

	public int GetDeviceType()
	{
		return Convert.ToInt32(capabilities.hwid.Split(':', '.')[1], CultureInfo.InvariantCulture);
	}

	public int GetDeviceIndex()
	{
		return Convert.ToInt32(capabilities.hwid.Split(':', '.')[2], CultureInfo.InvariantCulture);
	}

	public eventType[] FindSupportedEvents(hardwareType hardware)
	{
		if (supportedevents == null || hardware.events == null)
		{
			return new eventType[0];
		}
		return (from s in supportedevents
			select hardware.events.FindByReferenceId(s.eventreferenceid) into s
			where s != null
			select s).ToArray();
	}
}
