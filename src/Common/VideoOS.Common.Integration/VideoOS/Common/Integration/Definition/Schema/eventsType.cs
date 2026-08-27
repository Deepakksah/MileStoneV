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
public class eventsType
{
	private eventType[] eventField;

	private string maxcountField;

	[XmlElement("event")]
	public eventType[] @event
	{
		get
		{
			return eventField;
		}
		set
		{
			eventField = value;
		}
	}

	[XmlAttribute(DataType = "integer")]
	public string maxcount
	{
		get
		{
			return maxcountField;
		}
		set
		{
			maxcountField = value;
		}
	}

	public eventType FindByReferenceId(string referenceid)
	{
		eventType[] array = @event;
		foreach (eventType eventType2 in array)
		{
			if (eventType2.referenceid == referenceid)
			{
				return eventType2;
			}
		}
		return null;
	}
}
