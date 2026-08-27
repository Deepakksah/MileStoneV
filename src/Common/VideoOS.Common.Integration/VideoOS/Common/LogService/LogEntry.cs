using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Common.LogService;

[Serializable]
[GeneratedCode("wsdl", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectLogService")]
public class LogEntry
{
	private string tokenField;

	private string userIdField;

	private string userLocationField;

	private string objectTypeField;

	private string objectIdField;

	private string objectNameField;

	private string objectLocationIdField;

	private string objectLocationNameField;

	private DateTime customTimeStampField;

	private DateTime receivedTimeStampField;

	private string versionField;

	private string applicationField;

	private string componentField;

	private string moduleField;

	private string groupField;

	private string categoryField;

	private Status statusField;

	private Severity severityField;

	private string messageIdField;

	private string messageTextField;

	private string messageCultureField;

	private MessageVariable[] messageVariablesField;

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string Token
	{
		get
		{
			return tokenField;
		}
		set
		{
			tokenField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string UserId
	{
		get
		{
			return userIdField;
		}
		set
		{
			userIdField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string UserLocation
	{
		get
		{
			return userLocationField;
		}
		set
		{
			userLocationField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string ObjectType
	{
		get
		{
			return objectTypeField;
		}
		set
		{
			objectTypeField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string ObjectId
	{
		get
		{
			return objectIdField;
		}
		set
		{
			objectIdField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string ObjectName
	{
		get
		{
			return objectNameField;
		}
		set
		{
			objectNameField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string ObjectLocationId
	{
		get
		{
			return objectLocationIdField;
		}
		set
		{
			objectLocationIdField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string ObjectLocationName
	{
		get
		{
			return objectLocationNameField;
		}
		set
		{
			objectLocationNameField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public DateTime CustomTimeStamp
	{
		get
		{
			return customTimeStampField;
		}
		set
		{
			customTimeStampField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public DateTime ReceivedTimeStamp
	{
		get
		{
			return receivedTimeStampField;
		}
		set
		{
			receivedTimeStampField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string Version
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

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string Application
	{
		get
		{
			return applicationField;
		}
		set
		{
			applicationField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string Component
	{
		get
		{
			return componentField;
		}
		set
		{
			componentField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string Module
	{
		get
		{
			return moduleField;
		}
		set
		{
			moduleField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string Group
	{
		get
		{
			return groupField;
		}
		set
		{
			groupField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string Category
	{
		get
		{
			return categoryField;
		}
		set
		{
			categoryField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public Status Status
	{
		get
		{
			return statusField;
		}
		set
		{
			statusField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public Severity Severity
	{
		get
		{
			return severityField;
		}
		set
		{
			severityField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string MessageId
	{
		get
		{
			return messageIdField;
		}
		set
		{
			messageIdField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string MessageText
	{
		get
		{
			return messageTextField;
		}
		set
		{
			messageTextField = value;
		}
	}

	[XmlElement(Form = XmlSchemaForm.Unqualified)]
	public string MessageCulture
	{
		get
		{
			return messageCultureField;
		}
		set
		{
			messageCultureField = value;
		}
	}

	[XmlElement("MessageVariables", Form = XmlSchemaForm.Unqualified)]
	public MessageVariable[] MessageVariables
	{
		get
		{
			return messageVariablesField;
		}
		set
		{
			messageVariablesField = value;
		}
	}
}
