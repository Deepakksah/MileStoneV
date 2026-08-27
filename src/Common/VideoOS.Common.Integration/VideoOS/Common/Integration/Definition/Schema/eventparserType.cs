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
public class eventparserType
{
	private implementationType implementationField;

	private notificationtype notificationtypeField;

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

	[XmlAttribute]
	public notificationtype notificationtype
	{
		get
		{
			return notificationtypeField;
		}
		set
		{
			notificationtypeField = value;
		}
	}
}
